//
// BluePay C++ Sample code.
//
// This code sample runs a $3.00 Credit Card Sale transaction
// against a customer using test payment information.
// If using TEST mode, odd dollar amounts will return
// an approval and even dollar amounts will return a decline.
//

#include "Charge_Customer_CC.h"
#include "../bluepay-cpp/BluePay.h"
#include <iostream>
using namespace std;

void chargeCustomerCCLv2Lv3() {

    string accountId = "Merchant's Account ID Here";
    string secretKey = "Merchant's Secret Key Here";
    string mode = "TEST";
    
    BluePay payment(
    	accountId, 
	    secretKey, 
	    mode 
    );

    payment.setCustomerInformation(
	    "Bob", // First Name
	    "Tester", // Last Name
	    "123 Test St.", // Address1
	    "Apt #500", // Address2
	    "Testville", // City
	    "IL", // State
	    "54321", // Zip
	    "USA", // Country
	    "1231231234", // Phone Number
	    "test@bluepay.com" // Email Address
	  );

    payment.setCCInformation(
	    "4111111111111111", // Card Number
	    "1225", // Card Expire
	    "123" // Card CVV2
    );

    // Set Level 2 Information
    payment.setInvoiceId("123456789");
    payment.setAmountTax("0.91");

    // Add Line Item for Level3 Processing
    std::map<std::string, std::string> item1Params = {
      { "quantity", "1" }, // The number of units of item. Max: 5 digits.
      { "unitCost", "3.00" }, // The cost per unit of item. Max: 9 digits decimal.
      { "descriptor", "test1" }, // Description of the item purchased. Max: 26 character.
      { "commodityCode", "123412341234" }, // Commodity Codes can be found at http://www.census.gov/svsd/www/cfsdat/2002data/cfs021200.pdf. Max: 12 characters.
      { "productCode", "432143214321" }, // Merchant-defined code for the product or service being purchased. Max: 12 characters.
      { "measureUnits", "EA" }, // The unit of measure of the item purchase. Normally EA. Max: 3 characters.
      { "taxRate", "7%" }, // Tax rate for the item. Max: 4 digits.
      { "taxAmount", "0.21" }, // Tax amount for the item. unit_cost * quantity * tax_rate = tax_amount. Max: 9 digits.
      { "itemDiscount", "0.00" }, // The amount of any discounts on the item. Max: 12 digits.
      { "lineItemTotal", "3.21" } // The total amount for the item including taxes and discounts.
    };
    payment.addLineItem(item1Params);

    std::map<std::string, std::string> item2Params = {
      { "quantity", "2" },
      { "unitCost", "5.00" },
      { "descriptor", "test2" },
      { "commodityCode", "123412341234" },
      { "productCode", "098709870987" },
      { "measureUnits", "EA" },
      { "taxRate", "7%" },
      { "taxAmount", "0.70" },
      { "itemDiscount", "0.00" },
      { "lineItemTotal", "10.70" }
    };
    payment.addLineItem(item2Params);

    payment.sale("13.91"); // Sale Amount: $13.91

    // Makes the API Request with Blue
    payment.process();

    // Reads the responses from BluePay if transaction was approved
    if (payment.isSuccessfulTransaction()){
        cout << string("Transaction Status: ") + payment.getResult() + "\n";
        cout << string("Transaction Message: ") + payment.getMessage() + "\n";
        cout << string("Transaction ID: ") + payment.getTransId() + "\n";
        cout << string("AVS Result: ") + payment.getAvs() + "\n";
        cout << string("CVV2 Result: ") + payment.getCvv2() + "\n";
        cout << string("Masked Payment Account: ") + payment.getMaskedPaymentAccount() + "\n";
        cout << string("Card Type: ") + payment.getCardType() + "\n";
        cout << string("Authorization Code: ") + payment.getAuthCode() + "\n";
    }
    else {
        cout << string("Error: ") + payment.getMessage();
    }
}
