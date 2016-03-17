//
// BluePay C++ Sample code.
//
// Charges a customer $3.00 using the payment and customer information from a previous transaction. 
// If using TEST mode, odd dollar amounts will return
// an approval and even dollar amounts will return a decline.

#include "How_To_Use_Token.h"
#include "BluePay.h"
using namespace std;

void howToUseToken(){
	
	string accountId = "Merchant's Account ID Here";
	string secretKey = "Merchant's Secret Key Here";
	string mode = "TEST";
	string token = "Transaction ID here"; // id of a previous transaction

	BluePay payment(
		accountId,
		secretKey,
		mode
	);

	// Sale Amount: $3.00
	payment.sale("3.00", token);

   	// Makes the API Request with Blue
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
	}
	else {
	    cout << string("Error: ") + payment.getMessage();
	}
}
