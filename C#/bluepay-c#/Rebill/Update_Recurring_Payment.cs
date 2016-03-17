/*
* BluePay C#.NET Sample code.
* 
* This code sample runs a $0.00 Credit Card Auth transaction
* against a customer using test payment information.
* Once the rebilling cycle is created, this sample shows how to
* update the rebilling cycle. See comments below
* on the details of the initial setup of the rebilling cycle as well as the
* updated rebilling cycle.
* 
*/
using System;
using BluePayLibrary;

namespace Rebill
{
    class UpdateRecurringPayment
    {
        public static void Main()
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
            
            rebill.SetCCInformation
            (
                ccNumber: "4111111111111111",
                ccExpiration: "1215",
                cvv2: "123"
            );

            // Set recurring payment
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


            // If transaction was approved..
            if (rebill.IsSuccessfulTransaction())
            {

                BluePay paymentInformationUpdate = new BluePay
                (
                    accountID,
                    secretKey,
                    mode
                );

                // Sets an updated credit card expiration date
                paymentInformationUpdate.SetCCInformation
                (
                	ccExpiration: "1219"
                );

                // Stores new card expiration date
                paymentInformationUpdate.Auth
                (
                	amount: "0.00",
                	masterID: rebill.GetRebillID()  // the id of the rebill to update
                	);

                // Makes the API Request to update the payment information
                paymentInformationUpdate.Process();

                // Creates a request to update the rebill
                BluePay rebillUpdate = new BluePay
                (
                    accountID,
                    secretKey,
                    mode
                );

                // Updates the Rebill
                rebillUpdate.UpdateRebillingInformation
                (
					rebillID: rebill.GetRebillID(), // The ID of the rebill to be updated.  
                	templateID: paymentInformationUpdate.GetTransID(), // Updates the payment information portion of the rebilling cycle with the new card expiration date entered above 
                	rebNextDate: "2015-03-01", // Rebill Start Date: March 1, 2015
                	rebExpr: "1 MONTH", // Rebill Frequency: 1 MONTH
                	rebCycles: "8", // Rebill // of Cycles: 8
                	rebAmount: "5.15", // Rebill Amount: $5.15
                	rebNextAmount: "1.50" // Rebill Next Amount: $1.50
                );


                // Makes the API Request to update the rebill
                rebillUpdate.Process();

                // Reads the responses from BluePay
                Console.WriteLine("Rebill Status: " + rebillUpdate.GetStatus());
                Console.WriteLine("Rebill ID: " + rebillUpdate.GetRebillID());
                Console.WriteLine("Rebill Creation Date: " + rebillUpdate.GetCreationDate());
                Console.WriteLine("Rebill Next Date: " + rebillUpdate.GetNextDate());
                Console.WriteLine("Rebill Schedule Expression: " + rebillUpdate.GetSchedExpr());
                Console.WriteLine("Rebill Cycles Remaining: " + rebillUpdate.GetCyclesRemain());
                Console.WriteLine("Rebill Amount: " + rebillUpdate.GetRebillAmount());
            } 
            else
            {
                Console.WriteLine(rebill.GetMessage());
            }
        }
    }
}
