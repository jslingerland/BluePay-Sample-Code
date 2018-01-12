/***
* BluePay Java Sample code.
*
* This code sample runs a $3.00 Credit Card Sale transaction
* against a customer using test payment information. If
* approved, a 2nd transaction is run to update the first transaction 
* to $5.75, $2.75 more than the original $3.00.
* If using TEST mode, odd dollar amounts will return
* an approval and even dollar amounts will return a decline.
*/

package transactions;
import bluepay.*;
import java.util.HashMap; 

public class Update_Transaction {

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

    // Sets sale amount: $3.00
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
 
      // Creates an update transaction from previous sale
      BluePay paymentUpdate = new BluePay(
          ACCOUNT_ID,
          SECRET_KEY,
          MODE
      );
     
      // Attempts to update above sale transaction
      HashMap<String, String> updateParams = new HashMap<>();
      updateParams.put("amount", "5.75");  // add $2.75 to previous amount
      updateParams.put("transactionID", payment.getTransID()); // id of previous transaction to update
      paymentUpdate.update(updateParams);

      // Makes the API Request to process update
      try {
        paymentUpdate.process();
      } catch (Exception ex) {
        System.out.println("Exception: " + ex.toString());
        System.exit(1);
      }

      // Reads the response from BluePay
      System.out.println("Transaction Status: " + paymentUpdate.getStatus());
      System.out.println("Transaction Message: " + paymentUpdate.getMessage());
      System.out.println("Transaction ID: " + paymentUpdate.getTransID());
      System.out.println("AVS Result: " + paymentUpdate.getAVS());
      System.out.println("CVV2: " + paymentUpdate.getCVV2());
      System.out.println("Masked Payment Account: " + paymentUpdate.getMaskedPaymentAccount());
      System.out.println("Card Type: " + paymentUpdate.getCardType());    
      System.out.println("Authorization Code: " + paymentUpdate.getAuthCode());
    } else {
      System.out.println("Error: " + payment.getMessage());
    }
  }
}