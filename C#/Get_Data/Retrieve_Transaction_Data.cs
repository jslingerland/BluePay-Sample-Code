/*
* BluePay C#.NET Sample code.
*
* This code sample runs a report that grabs data from the
* BluePay gateway based on certain criteria. See comments below
* on the details of the report.
* If using TEST mode, only TEST transactions will be returned.
*/

using System;
using BPCSharp;

namespace BP
{
    class Retrieve_Transaction_Data
    {
        public Retrieve_Transaction_Data()
        {
        }

        public static void Main()
        {

            string accountID = "100221257489";
            string secretKey = "YCBJNEUEKNINP5PWEH1HRQDSQHYANPM/";
            string mode = "TEST";

            // Merchant's Account ID
            // Merchant's Secret Key
            // Transaction Mode: TEST (can also be LIVE)
            BluePayPayment report = new BluePayPayment(
                accountID,
                secretKey,
                mode);

            // Report Start Date: Jan. 1, 2013
            // Report End Date: Jan. 15, 2013
            // Also search subaccounts? Yes
            // Output response without commas? Yes
            // Do not include errored transactions? Yes
            report.GetTransactionReport(
                "2013-01-01",
                "2013-01-15",
                "1",
                "1",
                "1");

            report.Process();

            // Outputs response from BluePay gateway
            Console.Write(report.response);
        }
    }
}