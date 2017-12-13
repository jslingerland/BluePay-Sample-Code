/**
* BluePay Java Sample code.
*
* This code sample runs a $15.00 Credit Card Sale transaction
* against a customer using test payment information.
* Optional transaction data is also sent.
* If using TEST mode, odd dollar amounts will return
* an approval and even dollar amounts will return a decline.
*/

package transactions;
import bluepay.*;
import java.util.HashMap; 

public class Customer_Defined_Data {

  public static void main(String[] args) {

    String ACCOUNT_ID = "Merchant's Account ID Here";
    String SECRET_KEY = "Merchant's Secret Key Here";
    String MODE = "TEST";

    BluePay payment = new BluePay(
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
    payment.setCustomerInformation(customerParams);

    // Set Credit Card Information
    HashMap<String, String> ccParams = new HashMap<>();
    ccParams.put("cardNumber", "4111111111111111");
    ccParams.put("expirationDate", "1225");
    ccParams.put("cvv2", "123");
    payment.setCCInformation(ccParams);

    // Optional fields users can set
    payment.setCustomID1("12345"); // Custom ID1: 12345
    payment.setCustomID2("09866"); // Custom ID2: 09866
    payment.setInvoiceID("500000"); // Invoice ID: 50000
    payment.setOrderID("10023145"); // Order ID: 10023145
    payment.setAmountTip("1.00"); // Tip Amount: $1.00
    payment.setAmountTax("2.00"); // Tax Amount: $2.00
    payment.setAmountFood("10.00"); // Food Amount: $10.00
    payment.setAmountMisc("2.00"); // Miscellaneous Amount: $2.00
    
    // Sets sale amount: $15.00
    HashMap<String, String> saleParams = new HashMap<>();
    saleParams.put("amount", "15.00");
    payment.sale(saleParams);

    // Makes the API Request with BluePay
    try {
      payment.process();
    } catch (Exception ex) {
      System.out.println("Exception: " + ex.toString());
      System.exit(1);
    }

    // If transaction was successful reads the responses from BluePay
    if (payment.isSuccessful()) {
      System.out.println("Transaction Status: " + payment.getStatus());
      System.out.println("Transaction Message: " + payment.getMessage());
      System.out.println("Transaction ID: " + payment.getTransID());
      System.out.println("AVS Result: " + payment.getAVS());
      System.out.println("CVV2: " + payment.getCVV2());
      System.out.println("Masked Payment Account: " + payment.getMaskedPaymentAccount());
      System.out.println("Card Type: " + payment.getCardType());    
      System.out.println("Authorization Code: " + payment.getAuthCode());
    } else {
      System.out.println("Error: " + payment.getMessage());
    }
  }
}