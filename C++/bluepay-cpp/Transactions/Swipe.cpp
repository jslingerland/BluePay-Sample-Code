//
// BluePay C++ Sample code.
//
// This code sample runs a $3.00 sales transaction using the payment information obtained from a credit card swipe.
// If using TEST mode, odd dollar amounts will return an approval and even dollar amounts will return a decline.//


#include "Swipe.h"
#include "BluePay.h"
using namespace std;

void swipe() {

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
	    "1234 Test St.", // Address1
	    "Apt #500", // Address2
	    "Testville", // City
	    "IL", // State
	    "54321", // Zip
	    "USA", // Country
	    "1231231234", // Phone Number
	    "test@bluepay.com" // Email Address
	  );

    // Set payment information for a swiped credit card transaction
    payment.swipe("%B4111111111111111^TEST/BLUEPAY^1911101100001100000000667000000?;4111111111111111=191110110000667?");
    
    payment.sale("3.00"); // Sale Amount: $3.00

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
