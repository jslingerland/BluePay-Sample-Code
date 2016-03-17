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
using BPCSharp;

namespace BP
{
    public class Customer_Defined_Data
    {
        public Customer_Defined_Data()
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

            // Custom ID1: 12345
            payment.SetCustomID1("12345");

            // Custom ID2: 09866
            payment.SetCustomID2("09866");

            // Invoice ID: 50000
            payment.SetInvoiceID("500000");

            // Order ID: 10023145
            payment.SetOrderID("10023145");

            // Tip Amount: $6.00
            payment.SetAmountTip("6.00");

            // Tax Amount: $3.50
            payment.SetAmountTax("3.50");

            // Food Amount: $3.11
            payment.SetAmountFood("3.11");

            // Miscellaneous Amount: $5.00
            payment.SetAmountMisc("5.00");

            // Sale Amount: $15.00
            payment.Sale("15.00");

            payment.Process();

            // Outputs response from BluePay gateway
            Console.Write("Transaction ID: " + payment.GetTransID() + Environment.NewLine);
            Console.Write("Message: " + payment.GetMessage() + Environment.NewLine);
            Console.Write("Status: " + payment.GetStatus() + Environment.NewLine);
            Console.Write("AVS Result: " + payment.GetAVS() + Environment.NewLine);
            Console.Write("CVV2 Result: " + payment.GetCVV2() + Environment.NewLine);
            Console.Write("Masked Payment Account: " + payment.GetMaskedPaymentAccount() + Environment.NewLine);
            Console.Write("Card Type: " + payment.GetCardType() + Environment.NewLine);
            Console.Write("Authorization Code: " + payment.GetAuthCode() + Environment.NewLine);
        }
    }
}