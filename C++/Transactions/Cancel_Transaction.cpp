//
// BluePay C++ Sample code.
//
// This code sample runs a $3.00 Credit Card Sale transaction
// against a customer using test payment information.
// If approved, a 2nd transaction is run to cancel this transaction.
// If using TEST mode, odd dollar amounts will return
// an approval and even dollar amounts will return a decline.

#include "Cancel_Transaction.h"
#include "../bluepay-cpp/BluePay.h"
#include <iostream>
using namespace std;

void cancelTransaction(){
	string accountId = "Merchant's Account ID Here";
	string secretKey = "Merchant's Secret Key Here";
	string mode = "TEST";
	
	string result;

    BluePay payment(
    	accountId, 
	    secretKey, 
	    mode 
    );

    payment.setCustomerInformation(
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

    payment.setCCInformation(
	    "4111111111111111", // Card Number
	    "1225", // Card Expire
	    "123" // Card CVV2
    );

	// Sale Amount: $3.00
	payment.sale("3.00");

	// Makes the API request with BluePay
	payment.process();

	// If transaction was approved...
	if (payment.isSuccessfulTransaction()){

		BluePay paymentCancel(
			accountId,
			secretKey,
			mode
		);

		paymentCancel.voidTransaction(payment.getTransId());

		paymentCancel.process();

		// Reads the response from BluePay
		cout << string("Transaction ID: ") + paymentCancel.getTransId() + "\n";
		cout << string("Message: ") + paymentCancel.getMessage() + "\n";
		cout << string("Status: ") + paymentCancel.getResult() + "\n";
		cout << string("AVS Result: ") + paymentCancel.getAvs() + "\n";
		cout << string("CVV2 Result: ") + paymentCancel.getCvv2() + "\n";
		cout << string("Masked Payment Account: ") + paymentCancel.getMaskedPaymentAccount() + "\n";
		cout << string("Card Type: ") + paymentCancel.getCardType() + "\n";
		cout << string("Authorization Code: ") + paymentCancel.getAuthCode() + "\n";
	}
	else {
		cout << string("Error: ") + payment.getMessage();
	}
}