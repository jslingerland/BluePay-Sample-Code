/*
* BluePay C#.NET Sample code.
*
* This code sample runs a $3.00 ACH Sale transaction
* against a customer using test payment information.
*/

using System;
using BluePayLibrary;
using BluePayLibrary.Interfaces;
using BluePayLibrary.Interfaces.BluePay20Post;

namespace Transactions
{
    class ChargeCustomerACH
    {
        public static void Main()
        {
            var accountID = "Merchant's Account ID Here";
            var secretKey = "Merchant's Secret Key Here";
            var mode = Mode.TEST;

            var payment = BluePayMessage.Build(accountID, mode)
                //Override duplicate just for tests
                .WithFields(builder => builder.DuplicateOverride(true))
                .ForCustomer(
                    name1: "Bob",
                    name2: "Tester",
                    addr1: "1234 Test St.",
                    addr2: "Apt #500",
                    city: "Testville",
                    state: "IL",
                    zip: "54321",
                    country: "USA",
                    phone: "123-123-12345",
                    email: "test@bluepay.com"
                )
                .FromAch(
                    routingNumber: "123123123",
                    accountNumber: "123456789",
                    accountType: AccountType.Checking, 
                    docType: DocType.WEB
                )
                // Sale Amount: $3.00
                .Sale(amount: 3.00m);

            var client = new BluePay20PostClient();

            // Makes the API Request with BluePay
            var result = client.Process(payment.ToMessage(secretKey));

            if (!result.IsError && result.IsApproved)
            {
                // Reads the responses from BluePAy
                Console.WriteLine("Transaction Status: " + result.Status);
                Console.WriteLine("Transaction ID: " + result.TransId);
                Console.WriteLine("Transaction Message: " + result.Message);
                Console.WriteLine("Masked Payment Account: " + result.PaymentAccountMask);
                Console.WriteLine("Customer Bank Name: " + result.BankName);
            }
            else
            {
                Console.WriteLine("Error: " + result.Message);
            }
        }
    }
}