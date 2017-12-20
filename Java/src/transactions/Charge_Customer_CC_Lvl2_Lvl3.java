/**
* BluePay Java Sample code.
*
* This code sample runs a $3.00 Credit Card Sale transaction
* against a customer using test payment information.
* If using TEST mode, odd dollar amounts will return
* an approval and even dollar amounts will return a decline.
*
*/

package transactions;
import bluepay.*;
import java.util.HashMap; 

public class Charge_Customer_CC_Lvl2_Lvl3 {
  
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
    customerParams.put("address1", "123 Test St.");
    customerParams.put("address2", "Apt #500");
    customerParams.put("city", "Testville");
    customerParams.put("state", "IL");
    customerParams.put("zip", "54321");
    customerParams.put("country", "USA");
    customerParams.put("phone", "123-123-12345");
    customerParams.put("email", "test@bluepay.com");
    payment.setCustomerInformation(customerParams);

    // Set Credit Card Information
    HashMap<String, String> ccParams = new HashMap<>();
    ccParams.put("cardNumber", "4111111111111111");
    ccParams.put("expirationDate", "1225");
    ccParams.put("cvv2", "123");
    payment.setCCInformation(ccParams);
    
    // Set Level 2 Information
    payment.setInvoiceID("123456789");
    payment.setAmountTax("0.91");
    
    	// Add Line Item for Level3 Processing
    HashMap<String, String> item1Params = new HashMap<>();
    item1Params.put("quantity", "1"); // The number of units of item. Max: 5 digits.
    item1Params.put("unitCost", "3.00"); // The cost per unit of item. Max: 9 digits decimal.
    item1Params.put("descriptor", "test1"); // Description of the item purchased. Max: 26 character.
    item1Params.put("commodityCode", "123412341234"); // Commodity Codes can be found at http://www.census.gov/svsd/www/cfsdat/2002data/cfs021200.pdf. Max: 12 characters.
    item1Params.put("productCode", "432143214321"); // Merchant-defined code for the product or service being purchased. Max: 12 characters.
    item1Params.put("measureUnits", "EA"); // The unit of measure of the item purchase. Normally EA. Max: 3 characters.
    item1Params.put("taxRate", "7%"); // Tax rate for the item. Max: 4 digits.
    item1Params.put("taxAmount", "0.21"); // Tax amount for the item. unit_cost * quantity * tax_rate = tax_amount. Max: 9 digits.
    item1Params.put("itemDiscount", "0.00"); // The amount of any discounts on the item. Max: 12 digits.
    item1Params.put("lineItemTotal", "3.21"); // The total amount for the item including taxes and discounts.
    payment.addLineItem(item1Params);
    
    HashMap<String, String> item2Params = new HashMap<>();
    item2Params.put("quantity", "2");
    item2Params.put("unitCost", "5.00");
    item2Params.put("descriptor", "test2");
    item2Params.put("commodityCode", "123412341234");
    item2Params.put("productCode", "098709870987");
    item2Params.put("measureUnits", "EA");
    item2Params.put("taxRate", "7%");
    item2Params.put("taxAmount", "0.70");
    item2Params.put("itemDiscount", "0.00");
    item2Params.put("lineItemTotal", "10.70");
    payment.addLineItem(item2Params);
    
    // Set sale amount: $13.91
    HashMap<String, String> saleParams = new HashMap<>();
    saleParams.put("amount", "13.91");
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
      System.out.println("Transaction Message: " + payment.getMessage());
      System.out.println("Transaction ID: " + payment.getTransID());
      System.out.println("AVS Response: " + payment.getAVS());
      System.out.println("CVV2 Response: " + payment.getCVV2());
      System.out.println("Masked Payment Account: " + payment.getMaskedPaymentAccount());
      System.out.println("Card Type: " + payment.getCardType());    
      System.out.println("Authorization Code: " + payment.getAuthCode());
    } else {
      System.out.println("Error: " + payment.getMessage());
    }
  }
}