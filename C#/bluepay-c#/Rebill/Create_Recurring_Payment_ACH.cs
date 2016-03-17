/*
* BluePay C#.NET Sample code.
*
* This code sample creates a recurring payment charging $15.00 per month for one year.
*/

using System;
using BluePayLibrary;

namespace Rebill
{
	class CreateRecurringPaymentACH
	{
		public static void Main ()
		{
			string accountID = "Merchant's Account ID Here";
			string secretKey = "Merchant's Secret Key Here";
			string mode = "TEST";

			BluePay rebill = new BluePay
			(
			    accountID,
			    secretKey,
			    mode
			);

			rebill.SetCustomerInformation
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

			rebill.SetACHInformation
			(
			    routingNum: "123123123",
			    accountNum: "123456789",
			    accountType: "C",
			    docType: "WEB"
			);

			// Sets recurring payment
			rebill.SetRebillingInformation
			(
			    rebFirstDate: "2015-01-01", // Rebill Start Date: Jan. 1, 2015
			    rebExpr: "1 MONTH", // Rebill Frequency: 1 MONTH
			    rebCycles: "12", // Rebill # of Cycles: 12
			    rebAmount: "15.00" // Rebill Amount: $15.00
			);

			// Sets a Card Authorization at $0.00
			rebill.Auth(amount: "0.00");

			// Makes the API Request
			rebill.Process();

			// If transaction was successful reads the responses from BluePay
			if (rebill.IsSuccessfulTransaction())
			{
			    Console.WriteLine("Transaction ID: " + rebill.GetTransID());
			    Console.WriteLine("Rebill ID: " + rebill.GetRebillID());
			    Console.WriteLine("Transaction Status: " + rebill.GetStatus());
			    Console.WriteLine("Transaction Message: " + rebill.GetMessage());
			    Console.WriteLine("Masked Payment Account: " + rebill.GetMaskedPaymentAccount());
			    Console.WriteLine("Customer Bank Name: " + rebill.GetBank());
			}
			else
			{
			    Console.WriteLine("Error: " + rebill.GetMessage());
			}
		}
	}
}

