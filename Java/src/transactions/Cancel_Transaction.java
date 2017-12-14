/**
* BluePay Java Sample code.
*
* This code sample runs a $3.00 Credit Card Sale transaction
* against a customer using test payment information.
* If approved, a 2nd transaction is run to cancel this transaction
* If using TEST mode, odd dollar amounts will return
* an approval and even dollar amounts will return a decline.
*/

package transactions;
import bluepay.*;
import java.util.HashMap; 

public class Cancel_Transaction {

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
    customerParams.put("address2", "Apt #501");
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
   
    //  If transaction was successful creates a new payment cancelation 
    if (payment.isSuccessful()) {

      BluePay paymentCancel = new BluePay(
          ACCOUNT_ID,
          SECRET_KEY,
          MODE
        );
     
      // Finds the previous payment by ID and attempts to void it
      paymentCancel.voidTransaction(payment.getTransID());
      
      // Makes the API Request to void the payment
      try {
        paymentCancel.process();
      } catch (Exception ex) {
        System.out.println("Exception: " + ex.toString());
        System.exit(1);
      }
      
      // Reads the response from BluePay
      System.out.println("Transaction Status: " + paymentCancel.getStatus());
      System.out.println("Transaction Message: " + paymentCancel.getMessage());
      System.out.println("Transaction ID: " + paymentCancel.getTransID());
      System.out.println("AVS Result: " + paymentCancel.getAVS());
      System.out.println("CVV2: " + paymentCancel.getCVV2());
      System.out.println("Masked Payment Account: " + paymentCancel.getMaskedPaymentAccount());
      System.out.println("Card Type: " + paymentCancel.getCardType());    
      System.out.println("Authorization Code: " + paymentCancel.getAuthCode());
    } else {
      System.out.println(payment.getMessage());
    }
  }
}