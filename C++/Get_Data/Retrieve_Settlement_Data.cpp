/*
* BluePay C++ Sample code.
*
* This code sample runs a report that grabs data from the
* BluePay gateway based on certain criteria. This will ONLY return
* transactions that have already settled. See comments below
* on the details of the report.
* If using TEST mode, only TEST transactions will be returned.
*/

#include "BluePayPayment.h"
using namespace std;

int main()
{
std::string accountId = "MERCHANT'S ACCOUNT ID HERE";
std::string secretKey = "MERCHANT'S SECRET KEY HERE";
std::string mode = "TEST";

// Merchant's Account ID
// Merchant's Secret Key
// Transaction Mode: TEST (can also be LIVE)
BluePayPayment report(
accountId,
secretKey,
mode);

// Report Start Date: Jan. 1, 2013
// Report End Date: Jan. 15, 2013
// Also search subaccounts? Yes
// Output response without commas? Yes
// Do not include errored transactions? Yes
report.getTransactionSettledReport(
"2013-01-01",
"2013-01-15",
"1",
"1",
"1");

report.process();

// Outputs response from BluePay gateway
cout << report.getResponse();
return 0;
}