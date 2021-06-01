/*
* BluePay C#.NET Sample code.
*
* Charges a customer $3.00 using the payment and customer information from a previous transaction.
* If using TEST mode, odd dollar amounts will return
* an approval and even dollar amounts will return a decline.
*/

using System;
using BluePayLibrary;

namespace Transactions
{
    class HowToUseToken
    {
        public static void Main()
        {
            string accountID = "Merchant's Account ID Here";
            string secretKey = "Merchant's Secret Key Here";
            string mode = "TEST";
            string token = "Transaction ID Here"; 
        
            BluePay payment = new BluePay
            (
                accountID,
                secretKey,
                mode
            );

            //payment.SetCustomerInformation
            //(
            //    storedIndicator: "F",
            //    storedType:"C",
            //    storedId:"TESTID765456"
            //);

            // Set Sale Amount: $3.00.
            payment.Sale
            (
                amount: "3.00",
                masterID: token // Transaction ID of a previous sale
            );

            // Makes the API Request with BluePay
            payment.Process();

            // If transaction was successful reads the responses from BluePay
            if (payment.IsSuccessfulTransaction())
            {
                Console.WriteLine("Transaction Status: " + payment.GetStatus());
                Console.WriteLine("Transaction ID: " + payment.GetTransID());
                Console.WriteLine("Transaction Message: " + payment.GetMessage());
                Console.WriteLine("AVS Response: " + payment.GetAVS());
                Console.WriteLine("CVV2 Response: " + payment.GetCVV2());
                Console.WriteLine("Masked Payment Account: " + payment.GetMaskedPaymentAccount());
                Console.WriteLine("Card Type: " + payment.GetCardType());
                Console.WriteLine("Authorization Code: " + payment.GetAuthCode());
                //Console.WriteLine("Stored ID: " + payment.GetStoredId());
            }
            else
            {
                Console.WriteLine("Error: " + payment.GetMessage());
            }
        }
    }
}
