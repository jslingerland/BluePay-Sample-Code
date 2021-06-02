' *
' * Bluepay VB.NET Sample code.
' *
' * Charges a customer $3.00 using the payment and customer information from a previous transaction.
' * If using TEST mode, odd dollar amounts will return
' * an approval and even dollar amounts will return a decline.
' *

Imports System
Imports vbnet.BPVB

Namespace Transactions

    Public Class HowToUseToken

        Public Shared Sub run()

            Dim accountID As String = "Merchant's Account ID Here"
            Dim secretKey As String = "Merchant's Secret Key Here"
            Dim mode As String = "TEST"
            Dim token As String = "Transaction ID here" 

            Dim payment As BluePay = New BluePay(
                accountID,
                secretKey,
                mode
            )

            'payment.setCustomerInformation(
            '    storedIndicator:="F",
            '    storedType=:"C",
            '    storedId=:"TESTID987456"
            ')


            ' Creates a sale using the transaction id of a previous sale
            payment.sale(amount:="3.00", transactionID:= token)

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
                'Console.Write("Stored Id: " + payment.getStoredId() + Environment.NewLine)
            Else
                Console.Write("Transaction Error: " + payment.getMessage() + Environment.NewLine)
            End If
        End Sub
    End Class
End Namespace
