/*
* BluePay C++ Sample code.
*
* This code sample runs a $0.00 Credit Card Auth transaction
* against a customer using test payment information to initially
* set up the rebilling cycle. See comments below
* on the details of the initial setup of the rebilling cycle as well as the
* updated rebilling cycle.
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

// Auth Amount: $0.00
payment.auth("0.00");

payment.process();
result = payment.getResult();
// If transaction was approved..
if (result == "APPROVED") {
BluePayPayment rebillUpdate(
accountId,
secretKey,
mode);

// Cancels rebill using Rebill ID token returned
// Rebill Start Date: March 1, 2015
// Rebill Frequency: 1 MONTH
// Rebill # of Cycles: 8
// Rebill Amount: $5.15
// Rebill Next Amount: $1.50
rebillUpdate.updateRebillingInformation(
payment.getRebillId(),
"2015-03-01",
"1 MONTH",
"8",
"5.15",
"1.50");

rebillUpdate.process();

// Outputs response from BluePay gateway
cout << string("Rebill ID: ") + rebillUpdate.getRebillId() + "\n";
cout << string("Creation Date: ") + rebillUpdate.getCreationDate() + "\n";
cout << string("Rebill Next Date: ") + rebillUpdate.getNextDate() + "\n";
cout << string("Rebill Last Date: ") + rebillUpdate.getLastDate() + "\n";
cout << string("Rebill Frequency: ") + rebillUpdate.getSchedExpr() + "\n";
cout << string("Rebill Cycles Remaining: ") + rebillUpdate.getCyclesRemain() + "\n";
cout << string("Rebill Amount: ") + rebillUpdate.getRebillAmount() + "\n";

} else {
cout << payment.getResult();
}

return 0;
}