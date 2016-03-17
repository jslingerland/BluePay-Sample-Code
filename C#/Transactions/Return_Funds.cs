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
using BPCSharp;

namespace BP
{
    public class Return_Funds
    {
        public Return_Funds()
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
            if (result == "APPROVED")
            {

                BluePayPayment_BP10Emu paymentRefund = new BluePayPayment_BP10Emu(
                        accountID,
                        secretKey,
                        mode);
                // Refunds
                paymentRefund.Refund(payment.GetTransID(), "1.75");
                paymentRefund.Process();

                // Outputs response from BluePay gateway
                Console.Write("Transaction ID: " + paymentRefund.GetTransID() + Environment.NewLine);
                Console.Write("Message: " + paymentRefund.GetMessage() + Environment.NewLine);
                Console.Write("Status: " + paymentRefund.GetStatus() + Environment.NewLine);
                Console.Write("AVS Result: " + paymentRefund.GetAVS() + Environment.NewLine);
                Console.Write("CVV2 Result: " + paymentRefund.GetCVV2() + Environment.NewLine);
                Console.Write("Masked Payment Account: " + paymentRefund.GetMaskedPaymentAccount() + Environment.NewLine);
                Console.Write("Card Type: " + paymentRefund.GetCardType() + Environment.NewLine);
                Console.Write("Authorization Code: " + paymentRefund.GetAuthCode() + Environment.NewLine);
            } else {
                Console.Write(payment.GetMessage());
            }
        }
    }
}