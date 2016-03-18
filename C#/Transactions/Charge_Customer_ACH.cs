/*
* BluePay C#.NET Sample code.
*
* This code sample runs a $3.00 ACH Sale transaction
* against a customer using test payment information.
*/

using System;
using BluePayLibrary;

namespace Transactions
{
    class ChargeCustomerACH
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

            payment.SetACHInformation
            (
                routingNum: "123123123",
                accountNum: "123456789",
                accountType: "C",
                docType: "WEB"
            );

            // Sale Amount: $3.00
            payment.Sale(amount: "3.00");
            
            // Makes the API Request with BluePay
            payment.Process();

            // If transaction was successful reads the responses from BluePay
            if (payment.IsSuccessfulTransaction())
            {
                Console.WriteLine("Transaction ID: " + payment.GetTransID());
                Console.WriteLine("Transaction Status: " + payment.GetStatus());
                Console.WriteLine("Transaction Message: " + payment.GetMessage());
                Console.WriteLine("Masked Payment Account: " + payment.GetMaskedPaymentAccount());
                Console.WriteLine("Customer Bank Name: " + payment.GetBank());
            }
            else
            {
                Console.WriteLine("Error: " + payment.GetMessage());
            }

        }
    }
}