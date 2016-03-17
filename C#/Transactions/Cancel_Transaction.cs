/*
* BluePay C#.NET Sample code.
*
* This code sample runs a $3.00 Credit Card Sale transaction
* against a customer using test payment information.
* If using TEST mode, odd dollar amounts will return
* an approval and even dollar amounts will return a decline.
*/

using System;
using BPCSharp;

namespace BP
{
    public class Cancel_Transaction
    {
        public Cancel_Transaction()
        {
        }

        public static void Main()
        {

            string accountID = "100221257489";
            string secretKey = "YCBJNEUEKNINP5PWEH1HRQDSQHYANPM/";
            string mode = "TEST";

            // Merchant's Account ID
            // Merchant's Secret Key
            // Transaction Mode: TEST (can also be LIVE)
            BluePayPayment payment = new BluePayPayment(
                accountID,
                secretKey,
                mode);

            // Card Number: 4111111111111111
            // Card Expire: 12/15
            // Card CVV2: 123
            payment.SetCCInformation(
                    "4111111111111111",
                    "1215",
                    "123");

            // First Name: Bob
            // Last Name: Tester
            // Address1: 123 Test St.
            // Address2: Apt #500
            // City: Testville
            // State: IL
            // Zip: 54321
            // Country: USA
            payment.SetCustomerInformation(
                    "Bob",
                    "Tester",
                    "123 Test St.",
                    "Apt #500",
                    "Testville",
                    "IL",
                    "54321",
                    "USA");

            // Phone #: 123-123-1234
            payment.SetPhone("1231231234");

            // Email Address: test@bluepay.com
            payment.SetEmail("test@bluepay.com");

            // Sale Amount: $3.00
            payment.Sale("3.00");

            payment.Process();

            string result = payment.Process();
            // If transaction was approved..
            if (result == "APPROVED") {
                BluePayPayment paymentCancel = new BluePayPayment(
                        accountID,
                        secretKey,
                        mode);

                // Voids above transaction
                paymentCancel.VoidTransaction(payment.GetTransID());
                paymentCancel.Process();

                // Outputs response from BluePay gateway
                Console.Write("Transaction ID: " + paymentCancel.GetTransID() + Environment.NewLine);
                Console.Write("Message: " + paymentCancel.GetMessage() + Environment.NewLine);
                Console.Write("Status: " + paymentCancel.GetStatus() + Environment.NewLine);
                Console.Write("AVS Result: " + paymentCancel.GetAVS() + Environment.NewLine);
                Console.Write("CVV2 Result: " + paymentCancel.GetCVV2() + Environment.NewLine);
                Console.Write("Masked Payment Account: " + paymentCancel.GetMaskedPaymentAccount() + Environment.NewLine);
                Console.Write("Card Type: " + paymentCancel.GetCardType() + Environment.NewLine);
                Console.Write("Authorization Code: " + paymentCancel.GetAuthCode() + Environment.NewLine);
            } else {
                Console.Write(payment.GetMessage());
            }
        }
    }
}