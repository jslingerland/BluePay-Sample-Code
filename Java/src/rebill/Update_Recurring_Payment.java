/**
* BluePay Java Sample code.
*
* This code sample runs a $0.00 Credit Card Auth transaction
* against a customer using test payment information.
* Once the rebilling cycle is created, this sample shows how to
* update the rebilling cycle. See comments below
* on the details of the initial setup of the rebilling cycle as well as the
* updated rebilling cycle.
*/

package rebill;
import bluepay.*;
import java.util.HashMap; 

public class Update_Recurring_Payment {

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

    //  Makes the API Request to create a rebill
    try {
      rebill.process();
    } catch (Exception ex) {
      System.out.println("Exception: " + ex.toString());
      System.exit(1);
    }

    // Creates a payment information update
    BluePay paymentInformationUpdate = new BluePay(
        ACCOUNT_ID,
        SECRET_KEY,
        MODE
      );

      // Sets an updated credit card expiration date
      HashMap<String, String> ccParams2 = new HashMap<>();
      ccParams2.put("expirationDate", "1229");
      paymentInformationUpdate.setCCInformation(ccParams2);

      // Stores new card expiration date
      HashMap<String, String> authParams2 = new HashMap<>();
      authParams2.put("amount", "0.00");
      authParams2.put("transactionID", rebill.getTransID()); // the id of the rebill to update
      paymentInformationUpdate.auth(authParams2);

      // Makes the API Request to update the payment information
      try {
        paymentInformationUpdate.process();
      } catch (Exception ex) {
        System.out.println("Exception: " + ex.toString());
        System.exit(1);
      }
   
      // Creates a request to update the rebill
      BluePay rebillUpdate = new BluePay(
          ACCOUNT_ID,
          SECRET_KEY,
          MODE
      );

      // Updates the rebill
      HashMap<String, String> rebillUpdateParams = new HashMap<>();
      rebillUpdateParams.put("rebillID", rebill.getRebillingID()); // The ID of the rebill to be updated.
      rebillUpdateParams.put("templateID", paymentInformationUpdate.getTransID()); // Updates the payment information portion of the rebilling cycle with the new card expiration date entered above 
      rebillUpdateParams.put("nextDate", "2015-03-01");
      rebillUpdateParams.put("expr", "1 MONTH");
      rebillUpdateParams.put("cycles", "8");
      rebillUpdateParams.put("rebillAmount", "5.15");
      rebillUpdateParams.put("nextAmount", "1.50");
      rebillUpdate.updateRebill(rebillUpdateParams);

      // Makes the API Request to update the rebill
      try {
        rebillUpdate.process();
      } catch (Exception ex) {
        System.out.println("Exception: " + ex.toString());
        System.exit(1);
      }

      // Reads the response from BluePay
      if (rebill.isSuccessful()) {
        System.out.println("Rebill Status: " + rebillUpdate.getRebillStatus());
        System.out.println("Rebill ID: " + rebillUpdate.getRebillingID());
        System.out.println("Rebill Creation Date: " + rebillUpdate.getRebillCreationDate());
        System.out.println("Rebill Next Date: " + rebillUpdate.getRebillNextDate());
        System.out.println("Rebill Last Date: " + rebillUpdate.getRebillLastDate());
        System.out.println("Rebill Schedule Expression: " + rebillUpdate.getRebillSchedExpr());
        System.out.println("Rebill Cycles Remaining: " + rebillUpdate.getRebillCyclesRemain());
        System.out.println("Rebill Amount: " + rebillUpdate.getRebillAmount());
        System.out.println("Rebill Next Amount: " + rebillUpdate.getRebillNextAmount()
          );
        } else {
          System.out.println(rebill.getMessage());
        }

  }
}