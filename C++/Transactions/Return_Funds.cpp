//
// BluePay C++ Sample code.
//
// This code sample runs a $3.00 Credit Card Sale transaction
// against a customer using test payment information. If
// approved, a 2nd transaction is run to partially refund the 
// customer for $1.75 of the $3.00.
// If using TEST mode, odd dollar amounts will return
// an approval and even dollar amounts will return a decline.


#include "Return_Funds.h"
#include "../bluepay-cpp/BluePay.h"
#include <iostream>
using namespace std;

void returnFunds(){
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

	// Sale Amount: $3.00
	payment.sale("3.00");

	payment.process();


	// Reads the responses from BluePay if transaction was approved
	if (payment.isSuccessfulTransaction()){
		
		BluePay paymentRefund(
			accountId,
			secretKey,
			mode
		);

		paymentRefund.refund(payment.getTransId(), "1.75");

		paymentRefund.process();

		// Reads the responses from BluePay 
		cout << string("Transaction Status: ") + paymentRefund.getResult() + "\n";
		cout << string("Transaction Message: ") + paymentRefund.getMessage() + "\n";
		cout << string("Transaction ID: ") + paymentRefund.getTransId() + "\n";
		cout << string("AVS Result: ") + paymentRefund.getAvs() + "\n";
		cout << string("CVV2 Result: ") + paymentRefund.getCvv2() + "\n";
		cout << string("Masked Payment Account: ") + paymentRefund.getMaskedPaymentAccount() + "\n";
		cout << string("Card Type: ") + paymentRefund.getCardType() + "\n";
		cout << string("Authorization Code: ") + paymentRefund.getAuthCode() + "\n";
	}
	else {
        cout << string("Error: ") + payment.getMessage();
	}
}
