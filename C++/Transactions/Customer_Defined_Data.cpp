//
// BluePay C++ Sample code.
//
// This code sample runs a $25.00 Credit Card Sale transaction
// against a customer using test payment information.
// Optional transaction data is also sent.
// If using TEST mode, odd dollar amounts will return
// an approval and even dollar amounts will return a decline.


#include "Customer_Defined_Data.h"
#include "../bluepay-cpp/BluePay.h"
#include <iostream>
using namespace std;

void customerDefinedData(){
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
    "1215", // Card Expire
    "123" // Card CVV2
  );

	// Optional fields users can set
	payment.setPhone("1231231234"); // Phone #: 123-123-1234
	payment.setEmail("test@bluepay.com"); // Email Address: test@bluepay.com
	payment.setCustomId1("12345"); // Custom ID1: 12345
	payment.setCustomId2("09866"); // Custom ID2: 09866
	payment.setInvoiceId("500000"); // Invoice ID: 50000
	payment.setOrderId("10023145"); // Order ID: 10023145
	payment.setAmountTip("2.00"); // Tip Amount: $2.00
	payment.setAmountTax("3.50"); // Tax Amount: $3.50
	payment.setAmountFood("3.11"); // Food Amount: $3.11
	payment.setAmountMisc("5.00"); // Miscellaneous Amount: $5.00

	// Sale Amount: $25.00
	payment.sale("25.00"); 
	
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