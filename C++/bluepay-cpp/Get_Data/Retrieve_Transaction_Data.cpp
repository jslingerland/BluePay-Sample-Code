//
// BluePay C++ Sample code.
//
// This code sample runs a report that grabs data from the
// BluePay gateway based on certain criteria.
// If using TEST mode, only TEST transactions will be returned.

#include "Retrieve_Transaction_Data.h"
#include "BluePay.h"
using namespace std;

void retrieveTransactionData(){
    
    string accountId = "Merchant's Account ID Here";
    string secretKey = "Merchant's Secret Key Here";
    string mode = "TEST";

	BluePay report(
		accountId,
		secretKey,
		mode
	);

	report.getTransactionReport(
		"2015-01-01", // Report Start Date: YYYY-MM-DD
		"2015-05-31", // Report End Date: YYYY-MM-DD
		"1", // Also search subaccounts? Yes
		"1", // Output response without commas? Yes
		"1" // Do not include errored transactions? Yes
	);
    
    // Makes the API Request with Blue
	report.process();

    // Reads the responses from BluePay if transaction was approved
	cout << report.getResponse();
}

