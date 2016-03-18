/**
* BluePay Java Sample code.
*
* Charges a customer $3.00 using the payment and customer information from a previous transaction. 
* If using TEST mode, odd dollar amounts will return
* an approval and even dollar amounts will return a decline.
*/

package transactions;
import bluepay.*;
import java.util.HashMap; 

public class How_To_Use_Token {

  public static void main(String[] args) {
  
    String ACCOUNT_ID = "Merchant's Account ID Here";
    String SECRET_KEY = "Merchant's Secret Key Here";
    String MODE = "TEST";
    String TOKEN = "Transaction ID here"; 

    BluePay payment = new BluePay(
      ACCOUNT_ID,
      SECRET_KEY,
      MODE
    );

    // Sale Amount: $3.00
    HashMap<String, String> saleParams = new HashMap<>();
    saleParams.put("amount", "3.00"); 
    saleParams.put("transactionID", TOKEN); // The transaction ID of a previous sale
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
      System.out.println("AVS Result: " + payment.getAVS());
      System.out.println("CVV2: " + payment.getCVV2());
      System.out.println("Masked Payment Account: " + payment.getMaskedPaymentAccount());
      System.out.println("Card Type: " + payment.getCardType());   
      System.out.println("Authorization Code: " + payment.getAuthCode());
    } else {
      System.out.println("Error: " + payment.getMessage());
    }
  }
}