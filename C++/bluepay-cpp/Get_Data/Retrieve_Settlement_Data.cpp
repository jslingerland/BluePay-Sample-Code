//
// BluePay C++ Sample code.
//
// This code sample runs a report that grabs data from the
// BluePay gateway based on certain criteria. This will ONLY return
// transactions that have already settled. See comments below
// on the details of the report.
// If using TEST mode, only TEST transactions will be returned.

#include "Retrieve_Settlement_Data.h"
#include "BluePay.h"
using namespace std;

void retrieveSettlementData(){

    string accountId = "Merchant's Account ID Here";
    string secretKey = "Merchant's Secret Key Here";
    string mode = "TEST";

	BluePay report(
		accountId,
		secretKey,
		mode
	);

	report.getTransactionSettledReport(
		"2013-01-01", // Search Date Start: YYYY-MM-DD
		"2013-01-15", // Search Date End: YYYY-MM-DD
		"1", // Also search subaccounts? Yes
		"1", // Output response without commas? Yes
		"1" // Do not include errored transactions? Yes
	);

    // Makes the API Request with Blue
	report.process();

    // Reads the responses from BluePay if transaction was approved
	cout << report.getResponse();
}