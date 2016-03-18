/*
* BluePay C#.NET Sample code.
* 
* This code sample runs a $0.00 Credit Card Auth transaction
* against a customer using test payment information, sets up
* a rebilling cycle, and also shows how to cancel that rebilling cycle.
* See comments below on the details of the initial setup of the
* rebilling cycle.
* 
*/
using System;
using BluePayLibrary;

namespace Rebill
{
    class CancelRecurringPayment
    {
        public static void Main()
        {
            string accountID = "Merchant's Account ID Here";
            string secretKey = "Merchant's Secret Key Here";
            string mode = "TEST";

            BluePay rebill = new BluePay
            (
                accountID,
                secretKey,
                mode
            );

            rebill.SetCustomerInformation
            (
                firstName: "Bob",
                lastName: "Tester",
                address1: "1234 Test St.",
                address2: "Apt #500",
                city: "Testville",
                state: "IL",
                zip: "54321",
                country: "USA",
                phone: "123-123-12345",
                email: "test@bluepay.com"
            );
            
            rebill.SetCCInformation
            (
                ccNumber: "4111111111111111",
                ccExpiration: "1215",
                cvv2: "123"
            );

            // Sets recurring payment
            rebill.SetRebillingInformation
            (
                rebFirstDate: "2015-01-01", // Rebill Start Date: Jan. 1, 2015
                rebExpr: "1 MONTH", // Rebill Frequency: 1 MONTH
                rebCycles: "12", // Rebill # of Cycles: 12
                rebAmount: "15.00" // Rebill Amount: $15.00
            );

            // Sets a Card Authorization at $0.00
            rebill.Auth(amount: "0.00");

            // Makes the API Request
            rebill.Process();


            // If transaction was approved..
            if (rebill.IsSuccessfulTransaction())
            {

                BluePay rebillCancel = new BluePay
                (
                    accountID,
                    secretKey,
                    mode
                );

                // Find the rebill by ID and cancel rebilling cycle
                rebillCancel.CancelRebilling(rebill.GetRebillID());

                // Makes the API Request
                rebillCancel.Process();

                // Reads the responses from BluePay
                Console.WriteLine("Rebill Status: " + rebillCancel.GetStatus());
                Console.WriteLine("Rebill ID: " + rebillCancel.GetRebillID());
                Console.WriteLine("Rebill Creation Date: " + rebillCancel.GetCreationDate());
                Console.WriteLine("Rebill Next Date: " + rebillCancel.GetNextDate());
                Console.WriteLine("Rebill Schedule Expression: " + rebillCancel.GetSchedExpr());
                Console.WriteLine("Rebill Cycles Remaining: " + rebillCancel.GetCyclesRemain());
                Console.WriteLine("Rebill Amount: " + rebillCancel.GetRebillAmount());
            } 
            else
            {
                Console.WriteLine(rebill.GetMessage());
            }
        }
    }
}