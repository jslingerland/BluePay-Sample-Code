#include "URL_Generator.h"
#include "../bluepay-cpp/BluePay.h"
using namespace std;


void URLGenerator(){

string accountId = "Merchant's Account ID Here";
string secretKey = "Merchant's Secret Key Here";
string mode = "TEST";

BluePay testURL(
accountId,
secretKey,
mode
);


string generatedURL = testURL.generateURL(
"Test Merchant",	// merchantName
"www.google.com",	// returnURL
"SALE",	// transactionType
"Yes",	// acceptDiscover
"Yes",	// acceptAmex
"99.99", // amount
"Yes", // protectAmount
"mobileform01",	// paymentTemplate
"defaultres2", // receiptTemplate
"",	// receiptTempRemoteURL
"Yes",	// rebilling
"Yes", // rebProtect
"50",	// rebAmount
"12",	// rebCycles
"1 MONTH",	// rebStartDate
"1 MONTH",	// rebFrequency
"MyCustomID1.1234",	// customID1
"Yes",	// protectCustomID1
"MyCustomID2.12345678910",	// customID2
"Yes");	// protectCustomID2

cout << string("Generated URL: ") + generatedURL + "\n";

}