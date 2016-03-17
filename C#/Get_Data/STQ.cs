/*
* BluePay C#.NET Sample code.
*
* This code sample runs a report that grabs a single transaction
* from the BluePay gateway based on certain criteria.
* See comments below on the details of the report.
* If using TEST mode, only TEST transactions will be returned.
*/

using System;
using BPCSharp;

namespace BP
{
    class Single_Trans_Query
    {
        public Single_Trans_Query()
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
            BluePayPayment stq = new BluePayPayment(
                accountID,
                secretKey,
                mode);

            // Search Date Start: Jan. 1, 2013
            // Search Date End: Jan 15, 2013
            // Do not include errored transactions in search? Yes
            stq.GetSingleTransQuery(
                "2013-01-01",
                "2015-04-15",
                "1");
            stq.QueryByTransactionID("100221432443");
            stq.Process();

            // Outputs response from BluePay gateway
            Console.Write(stq.response);
        }
    }
}