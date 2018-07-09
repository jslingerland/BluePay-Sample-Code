/*
* BluePay C#.NET Sample code.
*
* This code sample runs a $0.00 AUTH transaction
* and creates a customer token using test payment information,
* which is then used to run a separate $3.99 sale.
*/

using System;
using BluePayLibrary;

namespace Transactions
{
    class CreateCustomerToken
    {
        public static void Main()
        {
            string accountID = "Merchant's Account ID Here";
            string secretKey = "Merchant's Secret Key Here";
            string mode = "TEST";

            BluePay token = new BluePay
            (
                accountID,
                secretKey,
                mode
            );
            
            token.SetCustomerInformation
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

            token.SetCCInformation
            (
                ccNumber: "4111111111111111",
                ccExpiration: "1225",
                cvv2: "123"
            );

            // Card Authorization Amount: $0.00
            token.Auth(
                amount: "0.00", 
                newCustomerToken: "true" // "True" generates random string. Other values will be used literally.
            );

            // Makes the API Request with BluePay
            token.Process();

            // If transaction was successful reads the responses from BluePay
            if (token.IsSuccessfulTransaction())
            {
                BluePay payment = new BluePay
                (
                    accountID,
                    secretKey,
                    mode
                );

                payment.Sale(
                    amount: "3.99",
                    customerToken: token.GetCustomerToken()
                );

                payment.Process();

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
                    Console.WriteLine("Customer Token: " + payment.GetCustomerToken());
                }
                else
                {
                    Console.WriteLine("Error: " + payment.GetMessage());
                }
            }
            else
            {
                Console.WriteLine("Error: " + token.GetMessage());
            }
        }
    }
}