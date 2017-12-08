//
// BluePay C++ Sample code.
//
// This code sample runs a $0.00 Credit Card Auth transaction
// against a customer using test payment information.
// Once the rebilling cycle is created, this sample shows how to
// get information back on this rebilling cycle.
// See comments below on the details of the initial setup of the
// rebilling cycle.

#include "Get_Recurring_Payment_Status.h"
#include "../bluepay-cpp/BluePay.h"
#include <iostream>
using namespace std;

void getRecurringPaymentStatus(){
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
		"15.00", // Rebill Amount: $3.50
		"2015-01-01", // Rebill Start Date: Jan. 5, 2015
		"1 MONTH", // Rebill Frequency: 1 MONTH
		"12" // Rebill # of Cycles: 5
	);

	// Auth Amount: $0.00
	rebill.auth("0.00");
    
    // Makes the API Request with Blue
	rebill.process();

	// If transaction was approved..
	if (rebill.isSuccessfulTransaction()){
		
		BluePay rebillStatus(
			accountId,
			secretKey,
			mode
		);

		// Find the rebill by ID and get rebill status 
		rebillStatus.getRebillStatus(rebill.getRebillId());

		// Makes the API Request to get the rebill status
		rebillStatus.process();

		// Reads the responses from BluePay if transaction was approved
		cout << string("Rebill Status: ") + rebillStatus.getStatus() + "\n";
		cout << string("Rebill ID: ") + rebillStatus.getRebillId() + "\n";
		cout << string("Rebill Creation Date: ") + rebillStatus.getCreationDate() + "\n";
		cout << string("Rebill Next Date: ") + rebillStatus.getNextDate() + "\n";
		cout << string("Rebill Last Date: ") + rebillStatus.getLastDate() + "\n";
		cout << string("Rebill Schedule Expression: ") + rebillStatus.getSchedExpr() + "\n";
		cout << string("Rebill Cycles Remaining: ") + rebillStatus.getCyclesRemain() + "\n";
		cout << string("Rebill Amount: ") + rebillStatus.getRebillAmount() + "\n";
	}
	else {
	    cout << string("Error: ") + rebill.getMessage();
	}

}