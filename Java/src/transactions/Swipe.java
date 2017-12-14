/**
* BluePay Java Sample code.
*
* This code sample runs a $3.00 sales transaction using the payment information obtained from a credit card swipe.
* If using TEST mode, odd dollar amounts will return an approval and even dollar amounts will return a decline.*
*/

package transactions;
import bluepay.*;
import java.util.HashMap; 

public class Swipe {
  
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

    // Set payment information for a swiped credit card transaction
    payment.swipe("%B4111111111111111^TEST/BLUEPAY^2511101100001100000000667000000?;4111111111111111=251110110000667?");

    // Set sale amount: $3.00
    HashMap<String, String> saleParams = new HashMap<>();
    saleParams.put("amount", "3.00");
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
      System.out.println("AVS Response: " + payment.getAVS());
      System.out.println("CVV2 Response: " + payment.getCVV2());
      System.out.println("Masked Payment Account: " + payment.getMaskedPaymentAccount());
      System.out.println("Card Type: " + payment.getCardType());    
      System.out.println("Authorization Code: " + payment.getAuthCode());
    } else {
      System.out.println("Error: " + payment.getMessage());
    }
  }
}
