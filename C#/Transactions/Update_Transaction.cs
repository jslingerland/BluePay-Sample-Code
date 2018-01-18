/*
* BluePay C#.NET Sample code.
*
* This code sample runs a $3.00 Credit Card Sale transaction
* against a customer using test payment information. If
* approved, a 2nd transaction is run to update the first transaction 
* to $5.75, $2.75 more than the original $3.00.
* If using TEST mode, odd dollar amounts will return
* an approval and even dollar amounts will return a decline.
*/

using System;
using BluePayLibrary;

namespace Transactions
{
    class UpdateTransaction
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
                address1: "123 Test St.",
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

            // If transaction was approved..
            if (payment.IsSuccessfulTransaction())
            {
                BluePay paymentUpdate = new BluePay
                (
                    accountID,
                    secretKey,
                    mode
                );

                // Updates previous sale transaction
                paymentUpdate.Update
                (
                    masterID: payment.GetTransID(), // id of previous transaction to update
                    amount: "5.75" // add $2.75 to original amount
                );

                // Makes the API Request with BluePay
                paymentUpdate.Process();

                // Reads the responses from BluePay
                Console.WriteLine("Transaction Status: " + paymentUpdate.GetStatus());
                Console.WriteLine("Transaction ID: " + paymentUpdate.GetTransID());
                Console.WriteLine("Transaction Message: " + paymentUpdate.GetMessage());
                Console.WriteLine("AVS Response: " + paymentUpdate.GetAVS());
                Console.WriteLine("CVV2 Response: " + paymentUpdate.GetCVV2());
                Console.WriteLine("Masked Payment Account: " + paymentUpdate.GetMaskedPaymentAccount());
                Console.WriteLine("Card Type: " + paymentUpdate.GetCardType());
                Console.WriteLine("Authorization Code: " + paymentUpdate.GetAuthCode());
            }
            else
            {
                Console.WriteLine("Error: " + payment.GetMessage());
            }
        }
    }
}