' *
' * Bluepay VB.NET Sample code.
' *
' * This code sample runs a $0.00 Credit Card Auth transaction
' * against a customer using test payment information.
' * Once the rebilling cycle is created, this sample shows how to
' * update the rebilling cycle. See comments below
' * on the details of the initial Setup of the rebilling cycle as well as the
' * updated rebilling cycle.
' *

Imports System
Imports vbnet.BPVB

Namespace Rebill

    Public Class UpdateRecurringPayment

        Public Shared Sub run()

            Dim accountID As String = "Merchant's Account ID Here"
            Dim secretKey As String = "Merchant's Secret Key Here"
            Dim mode As String = "TEST" 

            Dim rebill As BluePay = New BluePay(
                accountID,
                secretKey,
                mode
            )

            rebill.setCustomerInformation(
                firstName:="Bob",
                lastName:="Tester",
                address1:="12345 Test St.",
                address2:="Apt #500",
                city:="Testville",
                state:="IL",
                zipCode:="54321", 
                country:="USA",
                phone:="123-123-12345",
                email:="test@bluepay.com"
            )

            rebill.setCCInformation(
                ccNumber:="4111111111111111", 
                ccExpiration:="1225", 
                cvv2:="123" 
            )

            ' Rebill Start Date:=Jan. 1, 2015
            ' Rebill Frequency: 1 MONTH
            ' Rebill number of Cycles: 12
            ' Rebill Amount: $15.00
            rebill.setRebillingInformation(
                rebFirstDate:="2015-01-01", 
                rebExpr:="1 MONTH", 
                rebCycles:="12", 
                rebAmount:="15.00" 
            )

            rebill.auth(amount:="0.00")

            rebill.process()

            If rebill.isSuccessfulTransaction() Then

                Dim updatePaymentInformation As BluePay = New BluePay(
                    accountID,
                    secretKey,
                    mode
                )

                'Sets an updated credit card expiration date               
                updatePaymentInformation.setCCInformation(ccExpiration:="1229")

                ' Stores new card expiration date
                updatePaymentInformation.auth(
                    amount:="0.00", 
                    transactionID:=rebill.getTransID() 
                )

                updatePaymentInformation.process()

                Dim rebillUpdate As BluePay = New BluePay(
                    accountID,
                    secretKey,
                    mode
                )

                ' Rebill ID: The ID of the rebill to be updated.  
                ' Template ID: Updates the payment information portion of the rebill with the new card expiration date entered above 
                ' Rebill Start Date: March 1, 2015
                ' Rebill Frequency: 1 MONTH
                ' Rebill number of Cycles: 8
                ' Rebill Amount: $5.15
                ' Rebill Next Amount: $1.50
                rebillUpdate.updateRebillingInformation(
                  rebillID:= rebill.getRebillId(), 
                  templateID:= updatePaymentInformation.getTransId(), 
                  rebNextDate:= "2015-03-01", 
                  rebExpr:= "1 MONTH", 
                  rebCycles:= "8", 
                  rebAmount:= "5.15", 
                  rebNextAmount:= "1.50" 
                )

                rebillUpdate.process()

                Console.Write("Rebill Status: " + rebillUpdate.GetStatus() + Environment.NewLine)
                Console.Write("Rebill ID: " + rebillUpdate.GetRebillID() + Environment.NewLine)
                Console.Write("Rebill Creation Date: " + rebillUpdate.GetCreationDate() + Environment.NewLine)
                Console.Write("Rebill Next Date: " + rebillUpdate.GetNextDate() + Environment.NewLine)
                Console.Write("Rebill Last Date: " + rebillUpdate.GetLastDate() + Environment.NewLine)
                Console.Write("Rebill Schedule Expression: " + rebillUpdate.GetSchedExpr() + Environment.NewLine)
                Console.Write("Rebill Cycles Remaining: " + rebillUpdate.GetCyclesRemain() + Environment.NewLine)
                Console.Write("Rebill Amount: " + rebillUpdate.GetRebillAmount() + Environment.NewLine)
                Console.Write("Rebill Next Amount: " + rebillUpdate.GetNextAmount() + Environment.NewLine)
            Else
                Console.Write("Transaction Error: " + rebill.getMessage() + Environment.NewLine)
            End If
        End Sub
    End Class
End Namespace