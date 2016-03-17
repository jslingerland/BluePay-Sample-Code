//
// BluePay C++ Sample code.
//
// This code sample runs a report that grabs a single transaction
// from the BluePay gateway based on certain criteria.
// See comments below on the details of the report.
// If using TEST mode, only TEST transactions will be returned.

#include "Single_Transaction_Query.h"
#include "BluePay.h"
using namespace std;

void singleTransactionQuery(){
    
    string accountId = "Merchant's Account ID Here";
    string secretKey = "Merchant's Secret Key Here";
    string mode = "TEST";
    string transactionID = "Transaction ID here";

	BluePay report(
		accountId,
		secretKey,
		mode
	);

	report.getSingleTransQuery(
		transactionID, // ID of previous transaction
		"2015-01-01", // Search Date Start: YYYY-MM-DD
		"2015-05-30", // Search Date End: YYYY-MM-DD
		"1" // Do not include errored transactions in search? Yes
	);

    // Makes the API Request with Blue
	report.process();

    // Reads the responses from BluePay if transaction was approved
	cout << report.getResponse();
}

