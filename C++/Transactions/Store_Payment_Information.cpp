//
// BluePay C++ Sample code.
//
// This code sample runs a $0.00 Credit Card Authorization.
// This stores the customer's payment information securely in
// BluePay to be used for further transactions.


#include "Store_Payment_Information.h"
#include "../bluepay-cpp/BluePay.h"
#include <iostream>
using namespace std;

void storePaymentInformation(){
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

    //payment.setStoredParameters(
    //        "F",//Stored Indicator
    //        "C",//Stored Type
    //        "TESTID526957756"//Stored Id
    //      );

    payment.setCCInformation(
	    "4111111111111111", // Card Number
	    "1225", // Card Expire
	    "123" // Card CVV2
    );

	// Auth Amount: $0.00
	payment.auth("0.00");
    
    // Makes the API request with BluePay
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
        //cout << string("Stored ID: ") + payment.getStoredId() + "\n";
    }
    else {
        cout << string("Error: ") + payment.getMessage();
    }
}
