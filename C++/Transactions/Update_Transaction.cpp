//
// BluePay C++ Sample code.
//
// This code sample runs a $3.00 Credit Card Sale transaction
// against a customer using test payment information. If
// approved, a 2nd transaction is run to update the first transaction
// to $5.75, $2.75 more than the original $3.00.
// If using TEST mode, odd dollar amounts will return
// an approval and even dollar amounts will return a decline.


#include "Update_Transaction.h"
#include "../bluepay-cpp/BluePay.h"
#include <iostream>
using namespace std;

void updateTransaction(){
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
		
		BluePay paymentUpdate(
			accountId,
			secretKey,
			mode
		);

		paymentUpdate.update(payment.getTransId(), "5.75"); // add $2.75 to previous amount

		paymentUpdate.process();

		// Reads the responses from BluePay 
		cout << string("Transaction Status: ") + paymentUpdate.getResult() + "\n";
		cout << string("Transaction Message: ") + paymentUpdate.getMessage() + "\n";
		cout << string("Transaction ID: ") + paymentUpdate.getTransId() + "\n";
		cout << string("AVS Result: ") + paymentUpdate.getAvs() + "\n";
		cout << string("CVV2 Result: ") + paymentUpdate.getCvv2() + "\n";
		cout << string("Masked Payment Account: ") + paymentUpdate.getMaskedPaymentAccount() + "\n";
		cout << string("Card Type: ") + paymentUpdate.getCardType() + "\n";
		cout << string("Authorization Code: ") + paymentUpdate.getAuthCode() + "\n";
	}
	else {
        cout << string("Error: ") + payment.getMessage();
	}
}
