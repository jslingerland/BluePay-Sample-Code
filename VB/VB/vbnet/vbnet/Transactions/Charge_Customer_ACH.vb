' *
' * Bluepay VB.NET Sample code.
' *
' * This code sample runs a $3.00 ACH Sale transaction
' * against a customer using test payment information.
' *

Imports System
Imports vbnet.BPVB

Namespace Transactions

    Public Class ChargeCustomerACH
    
        Public Shared Sub run()

            Dim accountID As String = "Merchant's Account ID Here"
            Dim secretKey As String = "Merchant's Secret Key Here"
            Dim mode As String = "TEST" 

            Dim payment As BluePay = New BluePay(
                accountID,
                secretKey,
                mode
            )

            payment.setCustomerInformation(
                firstName:="Bob",
                lastName:="Tester",
                address1:="12344 Test St.",
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
            payment.setACHInformation(
                routingNum:="123123123", 
                accNum:="0523421", 
                accType:="C", 
                docType:="WEB" 
            )

            payment.sale(amount:="3.00")
            
            payment.process()

            If payment.isSuccessfulTransaction() Then
                Console.Write("Transaction ID: " + payment.getTransID() + Environment.NewLine)
                Console.Write("Transaction Status: " + payment.getStatus() + Environment.NewLine)
                Console.Write("Transaction Message: " + payment.getMessage() + Environment.NewLine)
                Console.Write("Masked Payment Account: " + payment.getMaskedPaymentAccount() + Environment.NewLine)
                Console.Write("Bank Name: " + payment.GetBank() + Environment.NewLine)
            Else
                Console.Write("Transaction Error: " + payment.getMessage() + Environment.NewLine)
            End If
        End Sub
    End Class
End Namespace