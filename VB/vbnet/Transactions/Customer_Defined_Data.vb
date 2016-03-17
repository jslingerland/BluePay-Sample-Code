' *
' * Bluepay VB.NET Sample code.
' *
' * This code sample runs a $25.00 Credit Card Sale transaction
' * against a customer using test payment information.
' * Optional transaction data is also sent.
' * If using TEST mode, odd dollar amounts will return
' * an approval and even dollar amounts will return a decline.
' *

Imports System
Imports vbnet.BPVB

Namespace Transactions
    Public Class CustomerDefinedData

        Public Shared Sub run()

            Dim accountID As String = "Merchant's Account ID Here"
            Dim secretKey As String = "Merchant's Secret Key Here"
            Dim mode As String = "TEST"

            Dim payment As BluePay = New BluePay(
                accountID,
                secretKey,
                mode
            )

            ' Sets customer information.
            payment.setCustomerInformation(
                firstName:="Bob",
                lastName:="Tester",
                address1:="1233 Test St.",
                address2:="Apt #500",
                city:="Testville",
                state:="IL",
                zipCode:="54321", 
                country:="USA",
                phone:="123-123-12345",
                email:="test@bluepay.com"
            )

            ' Sets credit card information
            payment.setCCInformation(
                ccNumber:="4111111111111111", 
                ccExpiration:="1215", 
                cvv2:="123" 
            )

            ' Optional fields users can set
            payment.setCustomID1("12345") ' Custom ID1: 12345
            payment.setCustomID2("09866") ' Custom ID2: 09866
            payment.setInvoiceID("500000") ' Invoice ID: 50000
            payment.setOrderID("10023145") ' Order ID: 10023145
            payment.setAmountFood("15.00") ' Food Amount: $15.00
            payment.setAmountTax("2.50") ' Tax Amount: $2.50
            payment.setAmountTip("2.50") ' Tip Amount: $2.50
            payment.setAmountMisc("5.00") ' Miscellaneous Amount: $5.00
            payment.setMemo("Enter any comments about the transaction here.") ' Comments about order

            ' Sale Amount: $25.00
            payment.sale(amount:="25.00")

            ' Makes the API request with BluePay
            payment.process()

            ' If transaction was successful reads the responses from BluePay
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
        End Sub
    End Class
End Namespace