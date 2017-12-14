/*
* BluePay C#.NET Sample code.
*
* This code sample runs a $3.00 Credit Card Sale transaction
* against a customer using test payment information.
* If using TEST mode, odd dollar amounts will return
* an approval and even dollar amounts will return a decline.
*/

using System;
using BluePayLibrary;

namespace Transactions
{
    class CancelTransaction
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
            
            payment.SetCCInformation
            (
                ccNumber: "4111111111111111",
                ccExpiration: "1225",
                cvv2: "123"
            );

            // Sale Amount: $3.00
            payment.Sale(amount: "3.00");

            // Makes the API Request with BluePay
            payment.Process();


            if (payment.IsSuccessfulTransaction())
            {
                // Creates a payment cancelation
                BluePay paymentCancel = new BluePay
                (
                    accountID,
                    secretKey,
                    mode
                );

                // Finds the previous payment by ID and attempts to void it
                paymentCancel.Void(payment.GetTransID());

                // Makes the API Request with BluePay to cancel transaction
                paymentCancel.Process();

                // Reads the responses from BluePAy
                Console.WriteLine("Transaction Status: " + paymentCancel.GetStatus());
                Console.WriteLine("Transaction ID: " + paymentCancel.GetTransID());
                Console.WriteLine("Transaction Message: " + paymentCancel.GetMessage());
                Console.WriteLine("AVS Response: " + paymentCancel.GetAVS());
                Console.WriteLine("CVV2 Response: " + paymentCancel.GetCVV2());
                Console.WriteLine("Masked Payment Account: " + paymentCancel.GetMaskedPaymentAccount());
                Console.WriteLine("Card Type: " + paymentCancel.GetCardType());
                Console.WriteLine("Authorization Code: " + paymentCancel.GetAuthCode());
            }
            else
            {
                Console.WriteLine("Error: " + payment.GetMessage());
            }
        }
    }
}