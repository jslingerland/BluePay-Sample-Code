/**
* BluePay Java Sample code.
*
* This code sample runs a $3.00 Credit Card Sale transaction
* against a customer using test payment information from a
* previously generated token.
* If using TEST mode, odd dollar amounts will return
* an approval and even dollar amounts will return a decline.
*/

package BluePayPayment.Transactions_BP10Emu;
import BluePayPayment.BluePayPayment_BP10Emu;

public class How_To_Use_Token {

  public static void main(String[] args) {
  
    String ACCOUNT_ID = "MERCHANT'S ACCOUNT ID HERE";
    String SECRET_KEY = "MERCHANT'S SECRET KEY HERE";
    String MODE = "TEST";
    String TOKEN = "TOKEN HERE";

    // Merchant's Account ID
    // Merchant's Secret Key
    // Transaction Mode: TEST (can also be LIVE)
    BluePayPayment_BP10Emu payment = new BluePayPayment_BP10Emu(
      ACCOUNT_ID,
      SECRET_KEY,
      MODE);

    // Sale Amount: $3.00
    payment.sale("3.00", TOKEN);
    
    try {
      payment.process();
    } catch (Exception ex) {
      System.out.println("Exception: " + ex.toString());
      System.exit(1);
    }

    if(payment.isError()) {
      System.out.println("Error: " + payment.getMessage());
    } else if(payment.isApproved() || payment.isDeclined()) {
      // Outputs response from BluePay gateway
      System.out.println("Transaction Status: " + payment.getStatus());
      System.out.println("Transaction ID: " + payment.getTransID());
      System.out.println("Transaction Message: " + payment.getMessage());
      System.out.println("AVS Result: " + payment.getAVS());
      System.out.println("CVV2: " + payment.getCVV2());
      System.out.println("Masked Payment Account: " + payment.getMaskedPaymentAccount());
      System.out.println("Card Type: " + payment.getCardType());   
      System.out.println("Authorization Code: " + payment.getAuthCode());
    }
    else {
      System.out.println("ERROR!");
    }
  }
}