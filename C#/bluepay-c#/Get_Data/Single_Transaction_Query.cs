/*
* BluePay C#.NET Sample code.
*
* This code sample runs a report that grabs a single transaction
* from the BluePay gateway based on certain criteria.
* See comments below on the details of the report.
* If using TEST mode, only TEST transactions will be returned.
*/

using System;
using BluePayLibrary;

namespace GetData
{
    class SingleTransactionQuery
    {
        public static void Main()
        {
            string accountID = "Merchant's Account ID Here";
            string secretKey = "Merchant's Secret Key Here";
            string mode = "TEST";
            
            BluePay report = new BluePay
            (
                accountID,
                secretKey,
                mode
            );

            // Set Single Transaction Query parameters
            report.GetSingleTransQuery
            (
                transactionID: "Transaction ID Here",
                reportStartDate: "2015-01-01", // YYYY-MM-DD; required
                reportEndDate: "2015-05-30", // YYYY-MM-DD; required
                errors: "1" // Do not include errored transactions? Yes; optional
            );

            // Makes the API request with BluePay 
            report.Process();

            // Reads the response from BluePay
            Console.Write(report.response);
        }
    }
}