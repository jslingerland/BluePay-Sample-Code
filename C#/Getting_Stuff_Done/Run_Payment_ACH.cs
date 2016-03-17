/*
* BluePay C#.NET Sample code.
*
* This code sample runs a $3.00 ACH Sale transaction
* against a customer using test payment information.
*/

using System;
using BPCSharp;

namespace BP
{
    public class Run_ACH_Payment
    {
        public Run_ACH_Payment()
        {
        }

        public static void Main()
        {

            string accountID = "MERCHANT'S ACCOUNT ID HERE";
            string secretKey = "MERCHANT'S SECRET KEY HERE";
            string mode = "TEST";

            // Merchant's Account ID
            // Merchant's Secret Key
            // Transaction Mode: TEST (can also be LIVE)
            BluePayPayment payment = new BluePayPayment(
                accountID,
                secretKey,
                mode);

            // Routing Number: 123123123
            // Account Number: 0523421
            // Account Type: Checking
            // ACH Document Type: WEB
            payment.setACHInformation(
                    "123123123",
                    "0523421",
                    "C",
                    "WEB");

            // First Name: Bob
            // Last Name: Tester
            // Address1: 123 Test St.
            // Address2: Apt #500
            // City: Testville
            // State: IL
            // Zip: 54321
            // Country: USA
            payment.setCustomerInformation(
                    "Bob",
                    "Tester",
                    "123 Test St.",
                    "Apt #500",
                    "Testville",
                    "IL",
                    "54321",
                    "USA");

            // Phone #: 123-123-1234
            payment.setPhone("1231231234");

            // Email Address: test@bluepay.com
            payment.setEmail("test@bluepay.com");

            // Sale Amount: $3.00
            payment.sale("3.00");

            payment.Process();

            // Outputs response from BluePay gateway
            Console.Write("Transaction ID: " + payment.getTransID() + Environment.NewLine);
            Console.Write("Message: " + payment.getMessage() + Environment.NewLine);
            Console.Write("Status: " + payment.getStatus() + Environment.NewLine);
            Console.Write("AVS Result: " + payment.getAVS() + Environment.NewLine);
            Console.Write("CVV2 Result: " + payment.getCVV2() + Environment.NewLine);
            Console.Write("Masked Payment Account: " + payment.getMaskedPaymentAccount() + Environment.NewLine);
            Console.Write("Bank Name: " + payment.getBank() + Environment.NewLine);
            Console.Write("Authorization Code: " + payment.getAuthCode() + Environment.NewLine);
        }
    }
}