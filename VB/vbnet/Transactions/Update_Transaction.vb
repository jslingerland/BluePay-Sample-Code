' *
' * Bluepay VB.NET Sample code.
' *
' * This code sample runs a $3.00 Credit Card Sale transaction
' * against a customer using test payment information. If
' * approved, a 2nd transaction is run to update the first transaction 
' * to $5.75, $2.75 more than the original $3.00.
' * If using TEST mode, odd dollar amounts will return
' * an approval and even dollar amounts will return a decline.
' *

Imports System
Imports vbnet.BPVB

Namespace Transactions

    Public Class UpdateTransaction

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
                ccExpiration:="1225", 
                cvv2:="123" 
            )

            payment.sale(amount:="3.00")

            payment.process()
            
            If payment.isSuccessfulTransaction() Then
                
                Dim paymentUpdate As BluePay = New BluePay(
                    accountID,
                    secretKey,
                    mode
                )

                ' Updates a transaction from previous sale
                paymentUpdate.update(transactionID:= payment.GetTransID(), amount:="5.75")

                paymentUpdate.process()

                Console.Write("Transaction Status: " + paymentUpdate.getStatus() + Environment.NewLine)
                Console.Write("Transaction Message: " + paymentUpdate.getMessage() + Environment.NewLine)
                Console.Write("Transaction ID: " + paymentUpdate.getTransID() + Environment.NewLine)
                Console.Write("AVS Result: " + paymentUpdate.getAVS() + Environment.NewLine)
                Console.Write("CVV2 Result: " + paymentUpdate.getCVV2() + Environment.NewLine)
                Console.Write("Masked Payment Account: " + paymentUpdate.getMaskedPaymentAccount() + Environment.NewLine)
                Console.Write("Card Type: " + paymentUpdate.getCardType() + Environment.NewLine)
                Console.Write("Authorization Code: " + paymentUpdate.getAuthCode() + Environment.NewLine)
            Else
                Console.Write("Transaction Error: " + payment.getMessage() + Environment.NewLine)
            End If
        End Sub
    End Class
End Namespace