//
// BluePay C++ Sample code.
//
// This code sample runs a $3.00 ACH sales transaction
// against a customer using test payment information.


#include "Charge_Customer_ACH.h"
#include "../bluepay-cpp/BluePay.h"
#include <iostream>
using namespace std;

void chargeCustomerACH(){
	
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

	payment.setACHInformation(
		"123123123", // Routing Number
		"0523421", // Account Number
		"C", // Account Type: Checking
		"WEB" // ACH Document Type: WEB
	);

	// Sale Amount: $3.00
	payment.sale("3.00");

	// Makes the API Request with Blue
	payment.process();

	// Reads the responses from BluePay if transaction was approved
	if (payment.isSuccessfulTransaction()){
		cout << string("Transaction ID: ") + payment.getTransId() + "\n";
		cout << string("Message: ") + payment.getMessage() + "\n";
		cout << string("Status: ") + payment.getResult() + "\n";
		cout << string("Masked Payment Account: ") + payment.getMaskedPaymentAccount() + "\n";
		cout << string("Bank Name: ") + payment.getBank() + "\n";
	}
	else {
	    cout << string("Error: ") + payment.getMessage();
	}
}