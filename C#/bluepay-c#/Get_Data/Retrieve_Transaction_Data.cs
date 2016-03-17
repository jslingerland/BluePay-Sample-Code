/*
* BluePay C#.NET Sample code.
*
* This code sample runs a report that grabs data from the
* BluePay gateway based on certain criteria. See comments below
* on the details of the report.
* If using TEST mode, only TEST transactions will be returned.
*/

using System;
using BluePayLibrary;

namespace GetData
{
    class RetrieveTransactionData
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

            report.GetTransactionReport
            (
                reportStartDate: "2015-01-01", // YYYY-MM-DD
                reportEndDate: "2015-05-30", // YYYY-MM-DD
                subaccountsSearched: "1", // Also search subaccounts? Yes
                doNotEscape: "1", // Output response without commas? Yes
                excludeErrors: "1" // Do not include errored transactions? Yes
            );

            // Makes the API request with BluePay
            report.Process();

            // Reads the reportesponse from BluePay
            Console.Write(report.response);
        }
    }
}