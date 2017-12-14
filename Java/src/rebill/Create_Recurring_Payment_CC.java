/**
* BluePay Java Sample code.
*
* This code sample creates a recurring payment charging $15.00 per month for one year.
* If using TEST mode, odd dollar amounts will return
* an approval and even dollar amounts will return a decline.
*
*/

package rebill;
import bluepay.*;
import java.util.HashMap; 

public class Create_Recurring_Payment_CC {

  public static void main(String[] args) {

    String ACCOUNT_ID = "Merchant's Account ID Here";
    String SECRET_KEY = "Merchant's Secret Key Here";
    String MODE = "TEST"; 

    BluePay rebill = new BluePay(
        ACCOUNT_ID,
        SECRET_KEY,
        MODE
    );

    // Set Customer Information
    HashMap<String, String> customerParams = new HashMap<>();
    customerParams.put("firstName", "Bob");
    customerParams.put("lastName", "Tester");
    customerParams.put("address1", "123 Test St.");
    customerParams.put("address2", "Apt #500");
    customerParams.put("city", "Testville");
    customerParams.put("state", "IL");
    customerParams.put("zip", "54321");
    customerParams.put("country", "USA");
    customerParams.put("phone", "123-123-12345");
    customerParams.put("email", "test@bluepay.com");
    rebill.setCustomerInformation(customerParams);

    // Set Credit Card Information
    HashMap<String, String> ccParams = new HashMap<>();
    ccParams.put("cardNumber", "4111111111111111");
    ccParams.put("expirationDate", "1225");
    ccParams.put("cvv2", "123");
    rebill.setCCInformation(ccParams);

    // Set recurring payment
    HashMap<String, String> rebillParams = new HashMap<>();
    rebillParams.put("firstDate", "2015-01-01"); // Rebill Start Date: Jan. 1, 2015
    rebillParams.put("expr", "1 MONTH"); // Rebill Frequency: 1 MONTH
    rebillParams.put("cycles", "12"); // Rebill # of Cycles: 12
    rebillParams.put("amount", "15.00"); // Rebill Amount: $15.00
    rebill.setRebillingInformation(rebillParams);

    // Sets a Card Authorization at $0.00
    HashMap<String, String> authParams = new HashMap<>();
    authParams.put("amount", "0.00");
    rebill.auth(authParams);

    // Makes the API Request with BluePay
    try {
      rebill.process();
    } catch (Exception ex) {
      System.out.println("Exception: " + ex.toString());
      System.exit(1);
    }

    // If transaction was successful reads the responses from BluePay
    if (rebill.isSuccessful()) {
      System.out.println("Transaction Status: " + rebill.getStatus());
      System.out.println("Transaction ID: " + rebill.getTransID());
      System.out.println("Rebill ID: " + rebill.getRebillingID());
      System.out.println("Transaction Message: " + rebill.getMessage());
      System.out.println("AVS Result: " + rebill.getAVS());
      System.out.println("CVV2: " + rebill.getCVV2());
      System.out.println("Masked Payment Account: " + rebill.getMaskedPaymentAccount());
      System.out.println("Card Type: " + rebill.getCardType());
      System.out.println("Authorization Code: " + rebill.getAuthCode());
    } else {
      System.out.println("Error: " + rebill.getMessage());
    }
  }
}