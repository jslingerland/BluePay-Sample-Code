' *
' * Bluepay VB.NET Sample code.
' *
' * This code sample runs a $3.00 Credit Card Sale transaction
' * against a customer using test payment information.
' * If using TEST mode, odd dollar amounts will return
' * an approval and even dollar amounts will return a decline.
' *

Imports System
Imports vbnet.BPVB

Namespace Transactions

    Public Class CancelTransaction

        Public Shared Sub Run()
            
            Dim accountID As String = "Merchant's Account ID Here"
            Dim secretKey As String = "Merchant's Secret Key Here"
            Dim mode As String = "TEST"  ' Transaction Mode: TEST (can also be LIVE)

            Dim payment As BluePay = New BluePay(
                accountID,
                secretKey,
                mode
            )

            payment.setCustomerInformation(
                firstName:="Bob",
                lastName:="Tester",
                address1:="123456 Test St.",
                address2:="Apt #500",
                city:="Testville",
                state:="IL",
                zipCode:="54321", 
                country:="USA",
                phone:="123-123-12345",
                email:="test@bluepay.com"
            )

            payment.setCCInformation(
                ccNumber:="4111111111111111", 
                ccExpiration:="1215", 
                cvv2:="123" 
            )

            payment.sale(amount:="3.00")

            payment.process()

            If payment.isSuccessfulTransaction() Then

                Dim paymentCancel As BluePay = New BluePay(
                    accountID,
                    secretKey,
                    mode
                )

                ' Finds the previous payment by ID and attempts to void it
                paymentCancel.void(payment.getTransID())
                
                paymentCancel.process()
                
                Console.Write("Transaction Status: " + paymentCancel.getStatus() + Environment.NewLine)
                Console.Write("Transaction Message: " + paymentCancel.getMessage() + Environment.NewLine)
                Console.Write("Transaction ID: " + paymentCancel.getTransID() + Environment.NewLine)
                Console.Write("AVS Result: " + paymentCancel.getAVS() + Environment.NewLine)
                Console.Write("CVV2 Result: " + paymentCancel.getCVV2() + Environment.NewLine)
                Console.Write("Masked Payment Account: " + paymentCancel.getMaskedPaymentAccount() + Environment.NewLine)
                Console.Write("Card Type: " + paymentCancel.getCardType() + Environment.NewLine)
                Console.Write("Authorization Code: " + paymentCancel.getAuthCode() + Environment.NewLine)
            Else
                Console.Write("Transaction Error: " + payment.getMessage() + Environment.NewLine)
            End If
        End Sub
    End Class
End Namespace