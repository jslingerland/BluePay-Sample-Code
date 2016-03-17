/*
* BluePay C#.NET Sample code.
*
* This code sample runs a $3.00 Credit Card Auth transaction
* against a customer using test payment information.
* If approved, a 2nd transaction is run to capture the Auth.
* If using TEST mode, odd dollar amounts will return
* an approval and even dollar amounts will return a decline.
*/

using System;
using BPCSharp;

namespace BP
{
    public class How_To_Use_Token
    {
        public How_To_Use_Token()
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

            // Auth Amount: $3.00
            payment.Auth("3.00");

            payment.Process();

            string result = payment.Process();

            // If transaction was approved..
            if (result == "APPROVED") {

                BluePayPayment paymentCapture = new BluePayPayment(
                        accountID,
                        secretKey,
                        mode);

                // Refunds
                paymentCapture.Capture(payment.GetTransID());
                paymentCapture.Process();

                // Outputs response from BluePay gateway
                Console.Write("Transaction ID: " + paymentCapture.GetTransID() + Environment.NewLine);
                Console.Write("Message: " + paymentCapture.GetMessage() + Environment.NewLine);
                Console.Write("Status: " + paymentCapture.GetStatus() + Environment.NewLine);
                Console.Write("AVS Result: " + paymentCapture.GetAVS() + Environment.NewLine);
                Console.Write("CVV2 Result: " + paymentCapture.GetCVV2() + Environment.NewLine);
                Console.Write("Masked Payment Account: " + paymentCapture.GetMaskedPaymentAccount() + Environment.NewLine);
                Console.Write("Card Type: " + paymentCapture.GetCardType() + Environment.NewLine);
                Console.Write("Authorization Code: " + paymentCapture.GetAuthCode() + Environment.NewLine);
            } else {
                Console.Write(payment.GetMessage());
            }
        }
    }
}