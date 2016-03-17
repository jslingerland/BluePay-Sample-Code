/*
* BluePay C++ Sample code.
*
* This code sample runs a $3.00 Credit Card Sale transaction
* against a customer using test payment information. If
* approved, a 2nd transaction is run to void the transaction.
* If using TEST mode, odd dollar amounts will return
* an approval and even dollar amounts will return a decline.
*/

#include "BluePayPayment.h"

using namespace std;

int main()
{
string accountId = "MERCHANT'S ACCOUNT ID HERE";
string secretKey = "MERCHANT'S SECRET KEY HERE";
string mode = "TEST";
string result;

// Merchant's Account ID
// Merchant's Secret Key
// Transaction Mode: TEST (can also be LIVE)
BluePayPayment payment(
accountId,
secretKey,
mode);

// Card Number: 4111111111111111
// Card Expire: 12/15
// Card CVV2: 123
payment.setCCInformation(
"4111111111111111",
"1215",
"123");

// First Name: Bob
// Last Name: Tester
// Address1: 123 Test St.
// Address2: Apt #500
// City: Testville
// State: IL
// Zip: 54321
// Country: USA
payment.setCustomerInformation(
"Bob",
"Tester",
"123 Test St.",
"Apt #500",
"Testville",
"IL",
"54321",
"USA");

// Rebill Amount: $3.50
// Rebill Start Date: Jan. 5, 2015
// Rebill Frequency: 1 MONTH
// Rebill # of Cycles: 5
payment.setRebillingInformation(
"3.50",
"2015-01-05",
"1 MONTH",
"5");

// Phone #: 123-123-1234
payment.setPhone("1231231234");

// Email Address: test@bluepay.com
payment.setEmail("test@bluepay.com");

// Sale Amount: $3.00
payment.sale("3.00");

payment.process();
result = payment.getResult();
// If transaction was approved..
if (result == "APPROVED") {
BluePayPayment paymentCancel(
accountId,
secretKey,
mode);

paymentCancel.voidTransaction(payment.getTransId());

paymentCancel.process();

// Outputs response from BluePay gateway
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
cout << payment.getResult();
}

return 0;
}