/*
* BluePay C++ Sample code.
*
* This code sample runs a $3.00 Credit Card Sale transaction
* against a customer using a previously generated token.
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
string token = "TRANSACTION ID HERE";

// Merchant's Account ID
// Merchant's Secret Key
// Transaction Mode: TEST (can also be LIVE)
BluePayPayment payment(
accountId,
secretKey,
mode);

// Sale Amount: $3.00
payment.sale("3.00", token);

payment.process();

// Outputs response from BluePay gateway
cout << string("Transaction ID: ") + payment.getTransId() + "\n";
cout << string("Message: ") + payment.getMessage() + "\n";
cout << string("Status: ") + payment.getResult() + "\n";
cout << string("AVS Result: ") + payment.getAvs() + "\n";
cout << string("CVV2 Result: ") + payment.getCvv2() + "\n";
cout << string("Masked Payment Account: ") + payment.getMaskedPaymentAccount() + "\n";
cout << string("Card Type: ") + payment.getCardType() + "\n";
cout << string("Authorization Code: ") + payment.getAuthCode() + "\n";
return 0;
}