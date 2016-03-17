/**
* BluePay Java Sample code.
*
* This code sample runs a report that grabs a single transaction
* from the BluePay gateway based on certain criteria.
* See comments below on the details of the report.
* If using TEST mode, only TEST transactions will be returned.
*/

package BluePayPayment.Get_Data_BP10Emu;
import BluePayPayment.BluePayPayment_BP10Emu;

public class STQ {

  public static void main(String[] args) {
 
    String ACCOUNT_ID = "MERCHANT'S ACCOUNT ID HERE";
    String SECRET_KEY = "MERCHANT'S SECRET KEY HERE";
    String MODE = "TEST";
     
    // Merchant's Account ID
        // Merchant's Secret Key
        // Transaction Mode: TEST (can also be LIVE)
    BluePayPayment_BP10Emu stq = new BluePayPayment_BP10Emu(
        ACCOUNT_ID,
        SECRET_KEY,
        MODE);
     
    // Report Start Date: Jan. 1, 2013
    // Report End Date: Jan. 15, 2013
    // Do not include errored transactions? Yes
    stq.getSingleTransQuery(
        "2013-01-01",
        "2013-01-15",
        "1");
   
    // Query by a specific Transaction ID
    stq.queryByTransactionID("ENTER A TRANSACTION ID HERE");
    try {
      stq.process();
      // Outputs response from BluePay gateway
      System.out.println(stq.getResponse());
    } catch (Exception ex) {
      System.out.println("Exception: " + ex.toString());
      System.exit(1);
    }
  }
}