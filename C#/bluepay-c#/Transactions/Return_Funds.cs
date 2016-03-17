/*
* BluePay C#.NET Sample code.
*
* This code sample runs a $3.00 Credit Card Sale transaction
* against a customer using test payment information. If
* approved, a 2nd transaction is run to refund the customer
* for $1.75.
* If using TEST mode, odd dollar amounts will return
* an approval and even dollar amounts will return a decline.
*/

using System;
using BluePayLibrary;

namespace Transactions
{
    class ReturnFunds
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
                ccExpiration: "1215",
                cvv2: "123"
            );

            // Sale Amount: $3.00
            payment.Sale(amount: "3.00");

            // Makes the API Request with BluePay
            payment.Process();

            // If transaction was approved..
            if (payment.IsSuccessfulTransaction())
            {
                BluePay paymentRefund = new BluePay
                (
                    accountID,
                    secretKey,
                    mode
                );

                // Creates a refund transaction against previous sale
                paymentRefund.Refund
                (
                    masterID: payment.GetTransID(), // id of previous transaction to refund
                    amount: "1.75" // partial refund of $1.75
                );

                // Makes the API Request with BluePay
                paymentRefund.Process();

                // Reads the responses from BluePay
                Console.WriteLine("Transaction Status: " + paymentRefund.GetStatus());
                Console.WriteLine("Transaction ID: " + paymentRefund.GetTransID());
                Console.WriteLine("Transaction Message: " + paymentRefund.GetMessage());
                Console.WriteLine("AVS Response: " + paymentRefund.GetAVS());
                Console.WriteLine("CVV2 Response: " + paymentRefund.GetCVV2());
                Console.WriteLine("Masked Payment Account: " + paymentRefund.GetMaskedPaymentAccount());
                Console.WriteLine("Card Type: " + paymentRefund.GetCardType());
                Console.WriteLine("Authorization Code: " + paymentRefund.GetAuthCode());
            }
            else
            {
                Console.WriteLine("Error: " + payment.GetMessage());
            }
        }
    }
}