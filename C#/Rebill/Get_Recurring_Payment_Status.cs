/*
* BluePay C#.NET Sample code.
* 
* This code sample retrieves rebill status by  Rebill ID.
* 
*/
using System;
using BluePayLibrary;

namespace Rebill
{
    class GetRecurringPaymentStatus
    {
        public static void Main()
        {
            string accountID = "Merchant's Account ID Here";
            string secretKey = "Merchant's Secret Key Here";
            string mode = "TEST";
            string rebillID = "Rebill ID Here";

            BluePay rebillData = new BluePay
            (
                accountID,
                secretKey,
                mode
            );

            // Find the rebill by ID and cancel rebilling cycle
            rebillData.GetRebillStatus(rebillID);

            // Makes the API Request
            rebillData.Process();

            // Reads the responses from BluePay
            Console.WriteLine("Rebill Status: " + rebillData.GetStatus());
            Console.WriteLine("Rebill ID: " + rebillData.GetRebillID());
            Console.WriteLine("Rebill Creation Date: " + rebillData.GetCreationDate());
            Console.WriteLine("Rebill Next Date: " + rebillData.GetNextDate());
            Console.WriteLine("Rebill Schedule Expression: " + rebillData.GetSchedExpr());
            Console.WriteLine("Rebill Cycles Remaining: " + rebillData.GetCyclesRemain());
            Console.WriteLine("Rebill Amount: " + rebillData.GetRebillAmount());
        }
    }
}