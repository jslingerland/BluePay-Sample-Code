//
// BluePay C++ Sample code.
//
// This code sample runs a $0.00 Credit Card Auth transaction
// against a customer using test payment information.
// Once the rebilling cycle is created, this sample shows how to
// update the rebilling cycle. See comments below
// on the details of the initial setup of the rebilling cycle as well as the
// updated rebilling cycle.


#include "Update_Recurring_Payment.h"
#include "BluePay.h"
using namespace std;

void updateRecurringPayment(){
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
	    "12345 Test St.", // Address1
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
	
		BluePay rebillUpdate(
			accountId,
			secretKey,
			mode
		);	

		// Updates the rebill 
		rebillUpdate.updateRebillingInformation(
			rebill.getRebillId(), // The ID of the rebill to be updated 
			"2015-03-01", // Rebill Start Date: March 1, 2015
			"1 MONTH", // Rebill Frequency: 1 MONTH
			"8", // Rebill # of Cycles: 8
			"5.15", // Rebill Amount: $5.15
			"1.50" // Rebill Next Amount: $1.50
		);

		// Makes the API Request with Blue
		rebillUpdate.process();

		// Reads the responses from BluePay 
		cout << string("Rebill Status: ") + rebill.getResult() + "\n";
		cout << string("Rebill ID: ") + rebillUpdate.getRebillId() + "\n";
		cout << string("Rebill Creation Date: ") + rebillUpdate.getCreationDate() + "\n";
		cout << string("Rebill Next Date: ") + rebillUpdate.getNextDate() + "\n";
		cout << string("Rebill Last Date: ") + rebillUpdate.getLastDate() + "\n";
		cout << string("Rebill Schedule Expression: ") + rebillUpdate.getSchedExpr() + "\n";
		cout << string("Rebill Cycles Remaining: ") + rebillUpdate.getCyclesRemain() + "\n";
		cout << string("Rebill Amount: ") + rebillUpdate.getRebillAmount() + "\n";

	} 
	else {
	    cout << string("Error: ") + rebill.getMessage();
	}
}