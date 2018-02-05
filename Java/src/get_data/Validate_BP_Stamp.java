/**
* BluePay Java Sample code.
*
* This code sample reads the values from a BP10emu redirect
* and authenticates the message using the the BP_STAMP
* provided in the response. Point the REDIRECT_URL of your 
* BP10emu request to the location of this script on your server.
*
*/

package get_data;
import bluepay.*;

import java.io.IOException;
import java.io.PrintWriter;
import java.security.NoSuchAlgorithmException;
import java.util.Map;

import javax.servlet.ServletException;
import javax.servlet.annotation.WebServlet;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;


@WebServlet("/validateBPStamp")
public class ValidBPStamp extends HttpServlet {
  private static final long serialVersionUID = 1L;

  protected void doGet(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
    PrintWriter pw = response.getWriter();
    String ACCOUNT_ID = "Merchant's Account ID Here";
    String SECRET_KEY = "Merchant's Secret Key Here";
    String MODE = "TEST";
    
    Map<String, String[]> responseParams = request.getParameterMap();
    
    if(responseParams.containsKey("BP_STAMP")) { // Check whether BP_STAMP is provided
        BluePay bp = new BluePay(
                ACCOUNT_ID,
                SECRET_KEY,
                MODE);
        
        String bpStampString = "";
        String[] bpStampFields = responseParams.get("BP_STAMP_DEF")[0].split(" "); // Split BP_STAMP_DEF on whitespace
        for (String field : bpStampFields) {;
            bpStampString += responseParams.get(field)[0]; // Concatenate values used to calculate expected BP_STAMP
        }
        String expectedStamp = "";
        try { 
            expectedStamp = bp.generateTPS(bpStampString, responseParams.get("TPS_HASH_TYPE")[0]).toUpperCase(); // Calculate expected BP_STAMP using hash function specified in response
        } catch (NoSuchAlgorithmException e) { 
          e.printStackTrace(); 
        }
        if (expectedStamp.equals(responseParams.get("BP_STAMP")[0])) { // Compare expected BP_STAMP with received BP_STAMP
            // Validate BP_STAMP and reads the response results
            pw.print("VALID BP_STAMP: TRUE\n");
            for (Map.Entry<String, String[]> param : responseParams.entrySet()) {
              pw.print(param.getKey() + ": " + param.getValue()[0] + "\n");
            }
        } else {
            pw.print("ERROR: BP_STAMP VALUES DO NOT MATCH\n");
        }
    } else {
      pw.print("ERROR: BP_STAMP NOT FOUND. CHECK MESSAGE & RESPONSEVERSION\n");
    }
  }
}