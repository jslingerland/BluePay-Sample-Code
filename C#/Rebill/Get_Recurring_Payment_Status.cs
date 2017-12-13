/*
* BluePay C#.NET Sample code.
*
* This code sample runs a $0.00 Credit Card Auth transaction
* against a customer using test payment information.
* Once the rebilling cycle is created, this sample shows how to
* get information back on this rebilling cycle.
* See comments below on the details of the initial setup of the
* rebilling cycle.
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
                ccExpiration: "1225",
                cvv2: "123"
            );

            // Set recurring payment
            rebill.SetRebillingInformation
            (
                rebFirstDate: "2015-01-01", // Rebill Start Date: Jan. 1, 2015
                rebExpr: "1 MONTH", // Rebill Frequency: 1 MONTH
                rebCycles: "12", // Rebill # of Cycles: 12
                rebAmount: "15.00" // Rebill Amount: $15.00
            );

            // Set a Card Authorization at $0.00
            rebill.Auth(amount: "0.00");

            // Makes the API Request
            rebill.Process();


            // If transaction was approved..
            if (rebill.IsSuccessfulTransaction())
            {

                BluePay rebillStatus = new BluePay
                (
                    accountID,
                    secretKey,
                    mode
                );

                // Find the rebill by ID and get rebill status 
                rebillStatus.GetRebillStatus(rebill.GetRebillID());

                // Makes the API Request
                rebillStatus.Process();

                // Reads the responses from BluePay
                Console.WriteLine("Rebill Status: " + rebillStatus.GetStatus());
                Console.WriteLine("Rebill ID: " + rebillStatus.GetRebillID());
                Console.WriteLine("Rebill Creation Date: " + rebillStatus.GetCreationDate());
                Console.WriteLine("Rebill Next Date: " + rebillStatus.GetNextDate());
                Console.WriteLine("Rebill Last Date: " + rebillStatus.GetLastDate());
                Console.WriteLine("Rebill Schedule Expression: " + rebillStatus.GetSchedExpr());
                Console.WriteLine("Rebill Cycles Remaining: " + rebillStatus.GetCyclesRemain());
                Console.WriteLine("Rebill Amount: " + rebillStatus.GetRebillAmount());
                Console.WriteLine("Rebill Next Amount: " + rebillStatus.GetNextAmount());
            } 
            else
            {
                Console.WriteLine(rebill.GetMessage());
            }
        }
    }
}