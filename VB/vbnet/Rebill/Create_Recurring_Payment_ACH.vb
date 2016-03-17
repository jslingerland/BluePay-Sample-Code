' *
' * Bluepay VB.NET Sample code.
' *
' * This code sample creates a recurring rebill charging $15.00 per month for one year.
' *

Imports System
Imports vbnet.BPVB

Namespace Rebill

    Public Class CreateRecurringPaymentACH

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
                address1:="1234 Test St.",
                address2:="Apt #500",
                city:="Testville",
                state:="IL",
                zipCode:="54321", 
                country:="USA",
                phone:="123-123-12345",
                email:="test@bluepay.com"
            )

            ' Routing Number: 123123123
            ' Account Number: 0523421
            ' Account Type: Checking
            ' ACH Document Type: WEB
            rebill.setACHInformation(
                routingNum:="123123123", 
                accNum:="0523421", 
                accType:="C", 
                docType:="WEB" 
            )

            ' Rebill Start Date: YYY-MM-DD
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
                Console.Write("Transaction ID: " + rebill.getTransID() + Environment.NewLine)
                Console.Write("Rebill ID: " + rebill.GetRebillID() + Environment.NewLine)
                Console.Write("Transaction Status: " + rebill.getStatus() + Environment.NewLine)
                Console.Write("Transaction Message: " + rebill.getMessage() + Environment.NewLine)
                Console.Write("Masked Payment Account: " + rebill.getMaskedPaymentAccount() + Environment.NewLine)
                Console.Write("Bank Name: " + rebill.GetBank() + Environment.NewLine)
            Else
                Console.Write("Transaction Error: " + rebill.getMessage() + Environment.NewLine)
            End If

        End Sub
    End Class
End Namespace