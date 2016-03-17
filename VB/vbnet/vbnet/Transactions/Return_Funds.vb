' *
' * Bluepay VB.NET Sample code.
' *
' * This code sample runs a $3.00 Credit Card Sale transaction
' * against a customer using test payment information. If
' * approved, a 2nd transaction is run to refund the customer
' * for $1.75.
' * If using TEST mode, odd dollar amounts will return
' * an approval and even dollar amounts will return a decline.
' *

Imports System
Imports vbnet.BPVB

Namespace Transactions

    Public Class ReturnFunds

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
                address1:="12345 Test St.",
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
                
                Dim paymentReturn As BluePay = New BluePay(
                    accountID,
                    secretKey,
                    mode
                )

                ' Creates a refund transaction against previous sale
                paymentReturn.refund(transactionID:= payment.GetTransID(), amount:="1.75")

                paymentReturn.process()

                Console.Write("Transaction Status: " + paymentReturn.getStatus() + Environment.NewLine)
                Console.Write("Transaction Message: " + paymentReturn.getMessage() + Environment.NewLine)
                Console.Write("Transaction ID: " + paymentReturn.getTransID() + Environment.NewLine)
                Console.Write("AVS Result: " + paymentReturn.getAVS() + Environment.NewLine)
                Console.Write("CVV2 Result: " + paymentReturn.getCVV2() + Environment.NewLine)
                Console.Write("Masked Payment Account: " + paymentReturn.getMaskedPaymentAccount() + Environment.NewLine)
                Console.Write("Card Type: " + paymentReturn.getCardType() + Environment.NewLine)
                Console.Write("Authorization Code: " + paymentReturn.getAuthCode() + Environment.NewLine)
            Else
                Console.Write("Transaction Error: " + payment.getMessage() + Environment.NewLine)
            End If
        End Sub
    End Class
End Namespace