/*
* BluePay C#.NET Sample code.
*
* This code sample runs a $0.00 Auth transaction
* against a customer using test payment information.
* This stores the customer's payment information securely in
* BluePay to be used for further transactions.
* Note: THIS DOES NOT ENSURE THAT THE CREDIT CARD OR ACH
* ACCOUNT IS VALID.
*/

using System;
using BluePayLibrary;

namespace Transactions
{
    class StorePaymentInformation
    {
        public static void Main()
        {
            string accountID = "100221257489";
            string secretKey = "YCBJNEUEKNINP5PWEH1HRQDSQHYANPM/";
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
            
            payment.SetCCInformation
            (
                ccNumber: "4111111111111111",
                ccExpiration: "1215",
                cvv2: "123"
            );

            // Card Authorization Amount: $0.00
            payment.Auth(amount: "0.00");
            
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