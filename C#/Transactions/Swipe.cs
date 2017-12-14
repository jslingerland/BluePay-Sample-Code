/*
* BluePay C#.NET Sample code.
*
* This code sample runs a $3.00 sales transaction using the payment information obtained from a credit card swipe.
* If using TEST mode, odd dollar amounts will return an approval and even dollar amounts will return a decline.
*/

using System;
using BluePayLibrary;

namespace Transactions
{
    class Swipe
    {
        public static void Main()
        {
            string accountID = "Merchant's Account ID Here";
            string secretKey = "Merchant's Secret Key Here";
            string mode = "TEST";
            
            BluePay payment = new BluePay
            (
                accountID,
                secretKey,
                mode
            );

            
            payment.SetCustomerInformation
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
            
            // Set payment information for a swiped credit card transaction
            payment.Swipe("%B4111111111111111^TEST/BLUEPAY^2511101100001100000000667000000?;4111111111111111=251110110000667?");

            // Sale Amount: $3.00
            payment.Sale(amount: "3.00");

            // Makes the API Request with BluePay
            payment.Process();

            // If transaction was successful reads the responses from BluePay
            if (payment.IsSuccessfulTransaction())
            {
                Console.WriteLine("Transaction Status: " + payment.GetStatus());
                Console.WriteLine("Transaction ID: " + payment.GetTransID());
                Console.WriteLine("Transaction Message: " + payment.GetMessage());
                Console.WriteLine("AVS Response: " + payment.GetAVS());
                Console.WriteLine("CVV2 Response: " + payment.GetCVV2());
                Console.WriteLine("Masked Payment Account: " + payment.GetMaskedPaymentAccount());
                Console.WriteLine("Card Type: " + payment.GetCardType());
                Console.WriteLine("Authorization Code: " + payment.GetAuthCode());
            }
            else
            {
                Console.WriteLine("Error: " + payment.GetMessage());
            }

        }
    }
}