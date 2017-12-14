//
// BluePay C++ Sample code.
//
// This code sample creates a recurring payment charging $15.00 per month for one year.

#include "Create_Recurring_Payment_CC.h"
#include "../bluepay-cpp/BluePay.h"
#include <iostream>
using namespace std;

void createRecurringPaymentCC(){

    string accountId = "Merchant's Account ID Here";
    string secretKey = "Merchant's Secret Key Here";
    string mode = "TEST";

    BluePay rebill(
    	accountId, 
	    secretKey, 
	    mode 
    );

    rebill.setCustomerInformation(
	    "Bob", // First Name
	    "Tester", // Last Name
	    "1234 Test St.", // Address1
	    "Apt #500", // Address2
	    "Testville", // City
	    "IL", // State
	    "54321", // Zip
	    "USA", // Country
	    "1231231234", // Phone Number
	    "test@bluepay.com" // Email Address
	  );

    rebill.setCCInformation(
	    "4111111111111111", // Card Number
	    "1225", // Card Expire
	    "123" // Card CVV2
    );

	rebill.setRebillingInformation(
		"15.00", // Rebill Amount
		"2015-01-01", // Rebill Start Date
		"1 MONTH", // Rebill Frequency
		"12" // Rebill number of Cycles
	);

	// Auth Amount: $0.00
	rebill.auth("0.00");
    
    // Makes the API Request with Blue
	rebill.process();

	// Reads the responses from BluePay if transaction was approved
	if (rebill.isSuccessfulTransaction()){
	    cout << string("Transaction ID: ") + rebill.getTransId() + "\n";
		cout << string("Rebill ID: ") + rebill.getRebillId() + "\n";
	    cout << string("Transaction Status: ") + rebill.getResult() + "\n";
	    cout << string("Transaction Message: ") + rebill.getMessage() + "\n";
	    cout << string("AVS Result: ") + rebill.getAvs() + "\n";
	    cout << string("CVV2 Result: ") + rebill.getCvv2() + "\n";
	    cout << string("Masked Payment Account: ") + rebill.getMaskedPaymentAccount() + "\n";
	    cout << string("Card Type: ") + rebill.getCardType() + "\n";
	    cout << string("Authorization Code: ") + rebill.getAuthCode() + "\n";
	}
	else {
	    cout << string("Error: ") + rebill.getMessage();
	}

}