/**
* BluePay Java Sample code.
*
* This code sample shows a very based approach
* on handling data that is posted to a script running
* a merchant's server after a transaction is processed
* through their BluePay gateway account.
*/

package get_data;
import bluepay.*;

import java.io.*;
import javax.servlet.*;
import javax.servlet.http.*;
import java.security.NoSuchAlgorithmException;

public class Transaction_Notification extends HttpServlet {
  
  protected void doPost(HttpServletRequest request, HttpServletResponse response) 
  throws ServletException, IOException {

    String ACCOUNT_ID = "Merchant's Account ID Here";
    String SECRET_KEY = "Merchant's Secret Key Here";
    String MODE = "TEST";

    BluePay tps = new BluePay(
          ACCOUNT_ID,
          SECRET_KEY,
          MODE);

    // get POST parameters
    String TRANS_ID = request.getParameter("trans_id");
    String TRANS_STATUS = request.getParameter("trans_status");
    String TRANS_TYPE = request.getParameter("trans_type");
    String AMOUNT = request.getParameter("amount");
    String REBILL_ID = request.getParameter("rebill_id");
    String REBILL_AMOUNT = request.getParameter("reb_amount");
    String REBILL_STATUS = request.getParameter("status");
    String TPS_HASH_TYPE = request.getParameter("TPS_HASH_TYPE");
    String BP_STAMP = request.getParameter("BP_STAMP");
    String BP_STAMP_DEF = request.getParameter("BP_STAMP_DEF");
 
    // calculate expected bp_stamp
    String bpStampString = "";
    String[] bpStampFields = BP_STAMP_DEF.split("%20");
    for (String field : bpStampFields) {
      bpStampString += request.getParameter(field);
    }
    bpStampString = java.net.URLDecoder.decode(bpStampString, "UTF-8");
    String expectedStamp = "";
    try { 
        expectedStamp = tps.generateTPS(bpStampString, TPS_HASH_TYPE).toUpperCase();
    } catch (NoSuchAlgorithmException e) { 
      e.printStackTrace(); 
    }
    
    // check if expected bp_stamp = actual bp_stamp
    if (expectedStamp == BP_STAMP) {
    // output response
      System.out.println("Transaction ID: " + TRANS_ID);
      System.out.println("Transaction Status: " + TRANS_STATUS);
      System.out.println("Transaction Type: " + TRANS_TYPE);
      System.out.println("Transaction Amount: " + AMOUNT);
      System.out.println("Rebill ID: " + REBILL_ID);
      System.out.println("Rebill Amount: " + REBILL_AMOUNT);
      System.out.println("Rebill Status: " + REBILL_STATUS);
    } else {
        System.out.println("ERROR IN RECEIVING DATA FROM BLUEPAY");
    }
  }
}