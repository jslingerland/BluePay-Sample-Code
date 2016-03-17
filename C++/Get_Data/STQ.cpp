/*
* BluePay C++ Sample code.
*
* This code sample runs a report that grabs a single transaction
* from the BluePay gateway based on certain criteria.
* See comments below on the details of the report.
* If using TEST mode, only TEST transactions will be returned.
*/

#include "BluePayPayment.h"
using namespace std;

int main()
{
string accountId = "MERCHANT'S ACCOUNT ID HERE";
string secretKey = "MERCHANT'S SECRET KEY HERE";
string mode = "TEST";
        string transId = "TRANSACTION ID HERE";

// Merchant's Account ID
// Merchant's Secret Key
// Transaction Mode: TEST (can also be LIVE)
BluePayPayment stq(
accountId,
secretKey,
mode);

// Search Date Start: Jan. 1, 2013
// Search Date End: Jan 15, 2013
// Do not include errored transactions in search? Yes
stq.getSingleTransQuery(
"2013-01-01",
"2013-01-15",
"1");
stq.queryByTransactionId(transId);

stq.process();

// Outputs response from BluePay gateway
cout << stq.getResponse();
return 0;
}