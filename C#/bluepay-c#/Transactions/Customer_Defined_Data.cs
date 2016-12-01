/*
* BluePay C#.NET Sample code.
*
* This code sample runs a $15.00 Credit Card Sale transaction
* against a customer using test payment information.
* Optional transaction data is also sent.
* If using TEST mode, odd dollar amounts will return
* an approval and even dollar amounts will return a decline.
*/

using System;
using System.Linq;
using BluePayLibrary;
using BluePayLibrary.Interfaces;
using BluePayLibrary.Interfaces.BluePay20Post;

namespace Transactions
{
    class CustomerDefinedData
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
                .FromCreditCard(
                    ccNumber: "4111111111111111",
                    expiration: DateTime.Today.AddYears(1),
                    cvv2: "123"
                )
                // Optional fields users can set
                .WithFields(builder => builder
                    .CustomId("12345")
                    .CustomId2("09866")
                    .InvoiceId("500000")
                    .OrderId("10023145")
                    .AmountTip(6.00m)
                    .AmountTax(3.50m)
                    .AmountFood(3.11m)
                    .AmountMisc(5.00m)
                    .Memo("Enter any comments about the transaction here.")
                    )
                
                // Sale Amount: $15.00
                .Sale(amount: 15.00m);

            var client = new BluePay20PostClient();

            // Makes the API Request with BluePay
            var result = client.Process(payment.ToMessage(secretKey));

            if (!result.IsError && result.IsApproved)
            {
                // Reads the responses from BluePAy
                Console.WriteLine("Transaction Status: " + result.Status);
                Console.WriteLine("Transaction ID: " + result.TransId);
                Console.WriteLine("Transaction Message: " + result.Message);
                Console.WriteLine("AVS Response: " + result.Avs);
                Console.WriteLine("CVV2 Response: " + result.Cvv2);
                Console.WriteLine("Masked Payment Account: " + result.PaymentAccountMask);
                Console.WriteLine("Card Type: " + result.CardType);
                Console.WriteLine("Authorization Code: " + result.AuthCode);
            }
            else
            {
                Console.WriteLine("Error: " + result.Message);
            }
        }
    }
}