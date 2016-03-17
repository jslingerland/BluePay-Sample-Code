//
// BluePay C++ Sample code.
//
// This code sample runs a $0.00 Credit Card Auth transaction
// against a customer using test payment information, sets up
// a rebilling cycle, and also shows how to cancel that rebilling cycle.
// See comments below on the details of the initial setup of the
// rebilling cycle.
 
#include "Cancel_Recurring_Payment.h"
#include "BluePay.h"
using namespace std;

void cancelRecurringPayment(){

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
	    "1215", // Card Expire
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

	// Reads the responses from BluePay if transaction was approved
	if (rebill.isSuccessfulTransaction()){
		
		BluePay rebillCancel(
			accountId,
			secretKey,
			mode
		);

		// Find rebill by id and cancel rebilling cycle 
		rebillCancel.cancelRebilling(rebill.getRebillId());

  		// Makes the API request to cancel the rebill
		rebillCancel.process();

		// Reads the responses from Bluepay
		cout << string("Rebill Status: ") + rebillCancel.getStatus() + "\n";
		cout << string("Rebill ID: ") + rebillCancel.getRebillId() + "\n";
		cout << string("Rebill Creation Date: ") + rebillCancel.getCreationDate() + "\n";
		cout << string("Rebill Next Date: ") + rebillCancel.getNextDate() + "\n";
		cout << string("Rebill Last Date: ") + rebillCancel.getLastDate() + "\n";
		cout << string("Rebill Schedule Expression: ") + rebillCancel.getSchedExpr() + "\n";
		cout << string("Rebill Cycles Remaining: ") + rebillCancel.getCyclesRemain() + "\n";
		cout << string("Rebill Amount: ") + rebillCancel.getRebillAmount() + "\n";

	}
	else {
	    cout << string("Error: ") + rebill.getMessage();
	}
}