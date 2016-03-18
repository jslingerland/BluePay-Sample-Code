//
// BluePay C++ Sample code.
//
// This code sample creates a recurring payment charging $15.00 per month for one year. 

#include "Create_Recurring_Payment_ACH.h"
#include "../bluepay-cpp/BluePay.h"
#include <iostream>
using namespace std;

void createRecurringPaymentACH(){
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
	    "123 Test St.", // Address1
	    "Apt #500", // Address2
	    "Testville", // City
	    "IL", // State
	    "54321", // Zip
	    "USA", // Country
	    "1231231234", // Phone Number
	    "test@bluepay.com" // Email Address
	  );

    rebill.setACHInformation(
    	"123123123", // Routing Number
    	"0523421", // Account Number
    	"C", // Account Type: Checking
    	"WEB" // ACH Document Type: WEB
    );

	rebill.setRebillingInformation(
		"15.00", // Rebill Amount
		"2015-01-01", // Rebill Start Date
		"1 MONTH", // Rebill Frequency
		"12" // Rebill # of Cycles
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
	    cout << string("Masked Payment Account: ") + rebill.getMaskedPaymentAccount() + "\n";
	    cout << string("Bank Name: ") + rebill.getBank() + "\n";
	}
	else {
	    cout << string("Error: ") + rebill.getMessage();
	}	
}