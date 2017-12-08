/**
* BluePay Java Sample code.
*
* This code sample runs a $0.00 Credit Card Auth transaction
* against a customer using test payment information, sets up
* a rebilling cycle, and also shows how to cancel that rebilling cycle. See comments below
* on the details of the initial setup of the rebilling cycle.
*/

package rebill;
import bluepay.*;
import java.util.HashMap; 

public class Cancel_Recurring_Payment {
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
    customerParams.put("address1", "122 Test St.");
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

    // Returns true if transaction was successful
    if (rebill.isSuccessful()) {

      // Creates a rebill cancellation
      BluePay rebillCancel = new BluePay(
        ACCOUNT_ID,
        SECRET_KEY,
        MODE
      );
         
      //  Find rebill by id and cancel rebilling cycle 
      rebillCancel.cancelRebill(rebill.getRebillingID());
      
      //  Makes the API Request to cancel rebill
      try {
        rebillCancel.process();
        } catch (Exception ex) {
        System.out.println("Exception: " + ex.toString());
        System.exit(1);
        }

        // Reads the response from BluePay
        System.out.println("Rebill Status: " + rebillCancel.getRebillStatus());
        System.out.println("Rebill ID: " + rebill.getRebillingID());
        System.out.println("Rebill Creation Date: " + rebillCancel.getRebillCreationDate());
        System.out.println("Rebill Next Date: " + rebillCancel.getRebillNextDate());
        System.out.println("Rebill Last Date: " + rebillCancel.getRebillLastDate());
        System.out.println("Rebill Schedule Expression: " + rebillCancel.getRebillSchedExpr());
        System.out.println("Rebill Cycles Remaining: " + rebillCancel.getRebillCyclesRemain());
        System.out.println("Rebill Amount: " + rebillCancel.getRebillAmount());
        System.out.println("Rebill Next Amount: " + rebillCancel.getRebillNextAmount());
        } else {
          System.out.println(rebill.getMessage());
        }
  }
}