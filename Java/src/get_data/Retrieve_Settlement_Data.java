/**
* BluePay Java Sample code.
*
* This code sample runs a report that grabs data from the
* BluePay gateway based on certain criteria. This will ONLY return
* transactions that have already settled. See comments below
* on the details of the report.
* If using TEST mode, only TEST transactions will be returned.
*/

package get_data;
import bluepay.*;
import java.util.HashMap; 

public class Retrieve_Settlement_Data {

  public static void main(String[] args) {
     
    String ACCOUNT_ID = "Merchant's Account ID Here";
    String SECRET_KEY = "Merchant's Secret Key Here";
    String MODE = "TEST";
     
    BluePay report = new BluePay(
        ACCOUNT_ID,
        SECRET_KEY,
        MODE
    );
         
    // Set report parameters
    HashMap<String, String> reportParams = new HashMap<>();
    reportParams.put("reportStart", "2015-01-01"); // YYYY-MM-DD
    reportParams.put("reportEnd", "2015-05-30"); // YYYY-MM-DD
    reportParams.put("subaccountsSearched", "1"); // Also search subaccounts? Yes
    reportParams.put("doNotEscape", "1"); // Output response without commas? Yes 
    reportParams.put("excludeErrors", "1"); // Do not include errored transactions? Yes
    report.getSettledTransactionReport(reportParams);

    //  Makes the API Request
    try {
      report.process();
      // Reads the response from BluePay
      System.out.println(report.getResponse());
    } catch (Exception ex) {
      System.out.println("Exception: " + ex.toString());
      System.exit(1);
    }
  }
}