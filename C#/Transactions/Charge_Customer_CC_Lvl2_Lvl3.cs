/*
* BluePay C#.NET Sample code.
*
* This code sample runs a $3.00 Credit Card Sale transaction
* against a customer using test payment information.
* If using TEST mode, odd dollar amounts will return
* an approval and even dollar amounts will return a decline.
*/

using System;
using System.Collections.Generic;
using BluePayLibrary;

namespace Transactions
{
    class ChargeCustomerCCLv2Lv3
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
                //storedIndicator: "F",
                //storedType:"C",
                //storedId:"TESTID765456",
                email: "test@bluepay.com"
            );

            payment.SetCCInformation
            (
                ccNumber: "4111111111111111",
                ccExpiration: "1225",
                cvv2: "123"
            );

            // Set Level 2 Information
            payment.SetInvoiceID("123456789");
            payment.SetAmountTax("0.91");

            // Set Level 3 line item information. Repeat for each item up to 99.
            payment.AddLineItem
            (
                 quantity: "1", // The number of units of item. Max: 5 digits
                 unitCost: "3.00", // The cost per unit of item. Max: 9 digits decimal
                 descriptor: "test1", //Description of the item purchased. Max: 26 character
                 commodityCode: "123412341234", // Commodity Codes can be found at http://www.census.gov/svsd/www/cfsdat/2002data/cfs021200.pdf. Max: 12 characters
                 productCode: "432143214321", // Merchant-defined code for the product or service being purchased. Max: 12 characters 
                 measureUnits: "EA", // The unit of measure of the item purchase. Normally EA. Max: 3 characters
                 taxRate: "7%", // Tax rate for the item. Max: 4 digits
                 taxAmount: "0.21", // Tax amount for the item. unit_cost * quantity * tax_rate = tax_amount. Max: 9 digits.
                 itemDiscount: "0.00", // The amount of any discounts on the item. Max: 12 digits.
                 lineItemTotal: "3.21" // The total amount for the item including taxes and discounts.           
            );

            payment.AddLineItem
            (
                quantity: "2", 
                unitCost: "5.00", 
                descriptor: "test2", 
                commodityCode: "123412341234", 
                productCode: "098709870987", 
                measureUnits: "EA", 
                taxRate: "7%",
                taxAmount: "0.70", 
                itemDiscount: "0.00", 
                lineItemTotal: "10.70"
            );


            // Sale Amount: $13.91
            payment.Sale(amount: "13.91");

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
