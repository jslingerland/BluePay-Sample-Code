package sample;
import bluepay.*;
import java.util.HashMap; 
import java.security.NoSuchAlgorithmException;

public class URL_Generator_Method_Sample {

  public static void main(String[] args) {
     
    String ACCOUNT_ID = "Merchant's Account ID Here";
    String SECRET_KEY = "Merchant's Secret Key Here";
    String MODE = "TEST";
     
    BluePay testAccount = new BluePay(
      ACCOUNT_ID,
      SECRET_KEY,
      MODE
    );

    try {
  HashMap<String, String> urlParams = new HashMap<>();
  urlParams.put("merchantName", "Test Merchant");
  urlParams.put("returnURL", "www.google.com");
  urlParams.put("transactionType", "SALE"); 
  urlParams.put("acceptDiscover", "Yes"); 
  urlParams.put("acceptAmex", "Yes"); 
  urlParams.put("amount", "99.99"); 
  urlParams.put("protectAmount", "Yes");
  urlParams.put("rebilling", "Yes");
  urlParams.put("rebProtect", "Yes"); 
  urlParams.put("rebAmount", "50"); 
  urlParams.put("rebCycles", "12"); 
  urlParams.put("rebStartDate", "1 MONTH"); 
  urlParams.put("rebFrequency", "1 MONTH"); 
  urlParams.put("customID1", "MyCustomID1.1234"); 
  urlParams.put("protectCustomID1", "Yes"); 
  urlParams.put("customID2", "MyCustomID2.12345678910"); 
  urlParams.put("protectCustomID2", "Yes"); 
  urlParams.put("paymentTemplate", "mobileform01"); 
  urlParams.put("receiptTemplate", "defaultres2"); 
  urlParams.put("receiptTempRemoteURL", "");
  String generatedURL = testAccount.generateURL(urlParams);
  System.out.println("Hosted Payment Form URL: " + generatedURL);

} catch (NoSuchAlgorithmException e) {
  	  e.printStackTrace();
}
  }
}