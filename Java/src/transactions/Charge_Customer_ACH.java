/**
* BluePay Java Sample code.
*
* This code sample runs a $3.00 ACH Sale transaction
* against a customer using test payment information.
*
*/

package transactions;
import bluepay.*;
import java.util.HashMap; 

public class Charge_Customer_ACH {

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
    customerParams.put("address1", "1234 Test St.");
    customerParams.put("address2", "Apt #500");
    customerParams.put("city", "Testville");
    customerParams.put("state", "IL");
    customerParams.put("zip", "54321");
    customerParams.put("country", "USA");
    customerParams.put("phone", "123-123-12345");
    customerParams.put("email", "test@bluepay.com");
    payment.setCustomerInformation(customerParams);

    // Set Credit Card Information
    HashMap<String, String> setACHParams = new HashMap<>();
    setACHParams.put("routingNum", "123123123"); // Routing Number: 123123123
    setACHParams.put("accountNum", "0523421"); // Account Number: 0523421
    setACHParams.put("accountType", "C"); // Account Type: Checking
    setACHParams.put("docType", "WEB"); // ACH Document Type: WEB
    payment.setACHInformation(setACHParams);

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
      System.out.println("Transaction Status: " + payment.getStatus());
      System.out.println("Transaction ID: " + payment.getTransID());
      System.out.println("Transaction Message: " + payment.getMessage());
      System.out.println("Masked Payment Account: " + payment.getMaskedPaymentAccount());
      System.out.println("Bank Name:" + payment.getBankName());
    } else {
      System.out.println("Error: " + payment.getMessage());
    }
  }
}