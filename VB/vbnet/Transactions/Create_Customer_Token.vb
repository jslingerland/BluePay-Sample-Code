' *
' * Bluepay VB.NET Sample code.
' *
' * This code sample runs a $0.00 AUTH transaction
' * and creates a customer token using test payment information,
' * which is then used to run a separate $3.99 sale.
' *


Imports System
Imports vbnet.BPVB

Namespace Transactions

    Public Class CreateCustomerToken

        Public Shared Sub run()

            Dim accountID As String = "Merchant's Account ID Here"
            Dim secretKey As String = "Merchant's Secret Key Here"
            Dim mode As String = "TEST"

            Dim token As BluePay = New BluePay(
                accountID,
                secretKey,
                mode
            )

            token.setCustomerInformation(
                firstName:="Bob",
                lastName:="Tester",
                address1:="123 Test St.",
                address2:="Apt #500",
                city:="Testville",
                state:="IL",
                zipCode:="54321", 
                country:="USA",
                phone:="123-123-12345",
                email:="test@bluepay.com"
            )

            token.setCCInformation(
                ccNumber:="4111111111111111", 
                ccExpiration:="1225", 
                cvv2:="123" 
            )

            token.auth(
                amount:="0.00",
                newCustomerToken:="true" ' "true" generates random string. Other values will be used literally
            )
            
            token.process()

            ' Try again if we accidentally create a non-unique token
            If token.getMessage().Contains("Customer%20Tokens%20must%20be%20unique") Then
                token.auth(
                    amount:="0.00",
                    newCustomerToken:="true"
                )
                token.process()
            End If

            If token.isSuccessfulTransaction() Then
                Dim payment As BluePay = New BluePay(
                    accountID,
                    secretKey,
                    mode
                )

                payment.sale(
                    amount:="3.99",
                    customerToken:= token.getCustomerToken()
                )

                payment.process()

                If payment.isSuccessfulTransaction() Then
                    Console.Write("Transaction Status: " + payment.getStatus() + Environment.NewLine)
                    Console.Write("Transaction Message: " + payment.getMessage() + Environment.NewLine)
                    Console.Write("Transaction ID: " + payment.getTransID() + Environment.NewLine)
                    Console.Write("AVS Result: " + payment.getAVS() + Environment.NewLine)
                    Console.Write("CVV2 Result: " + payment.getCVV2() + Environment.NewLine)
                    Console.Write("Masked Payment Account: " + payment.getMaskedPaymentAccount() + Environment.NewLine)
                    Console.Write("Card Type: " + payment.getCardType() + Environment.NewLine)
                    Console.Write("Authorization Code: " + payment.getAuthCode() + Environment.NewLine)
                Else
                    Console.Write("Transaction Error: " + payment.getMessage() + Environment.NewLine)
                End If
            Else
                Console.Write("Transaction Error: " + token.getMessage() + Environment.NewLine)
            End If
        End Sub
    End Class
End Namespace