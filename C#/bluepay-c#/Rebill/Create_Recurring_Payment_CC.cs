/*
* BluePay C#.NET Sample code.
*
* This code sample creates a recurring payment charging $15.00 per month for one year.
*/

using System;
using BluePayLibrary;

namespace Rebill
{
    class CreateRecurringPaymentCC
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

            // If transaction was successful reads the responses from BluePay
            if (rebill.IsSuccessfulTransaction())
            {
                Console.WriteLine("Transaction ID: " + rebill.GetTransID());
                Console.WriteLine("Rebill ID: " + rebill.GetRebillID());
                Console.WriteLine("Transaction Status: " + rebill.GetStatus());
                Console.WriteLine("Transaction Message: " + rebill.GetMessage());
                Console.WriteLine("AVS Response: " + rebill.GetAVS());
                Console.WriteLine("CVV2 Response: " + rebill.GetCVV2());
                Console.WriteLine("Masked Payment Account: " + rebill.GetMaskedPaymentAccount());
                Console.WriteLine("Card Type: " + rebill.GetCardType());
                Console.WriteLine("Authorization Code: " + rebill.GetAuthCode());
            }
            else
            {
                Console.WriteLine("Error: " + rebill.GetMessage());
            }

        }
    }
}