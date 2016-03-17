' *
' * Bluepay VB.NET Sample code.
' *
' * This code sample runs a $1.00 ACH Sale transaction
' * against a customer using test payment information.
' * See comments below on the details of the initial Setup of the
' * rebilling cycle.
' * If using TEST mode, odd dollar amounts will return
' * an approval and even dollar amounts will return a decline.
' *

Imports System
Imports BluePay.BPVB

Namespace BP10Emu

    Public Class Create_Recurring_Payment

        Public Sub New()
            MyBase.New()
        End Sub

        Public Shared Sub Main()

            Dim accountID As String = "100221257489"
            Dim secretKey As String = "YCBJNEUEKNINP5PWEH1HRQDSQHYANPM/"
            Dim mode As String = "TEST"

            ' Merchant's Account ID
            ' Merchant's Secret Key
            ' Transaction Mode: TEST (can also be LIVE)
            Dim payment As BluePayPayment_BP10Emu = New BluePayPayment_BP10Emu(
                    accountID,
                    secretKey,
                    mode)

            ' Card Number: 4111111111111111
            ' Card Expire: 12/15
            ' Card CVV2: 123
            payment.SetCCInformation(
                    "4111111111111111",
                    "1215",
                    "123")

            ' First Name: Bob
            ' Last Name: Tester
            ' Address1: 123 Test St.
            ' Address2: Apt #500
            ' City: Testville
            ' State: IL
            ' Zip: 54321
            ' Country: USA
            payment.SetCustomerInformation(
                    "Bob",
                    "Tester",
                    "123 Test St.",
                    "Apt #500",
                    "Testville",
                    "IL",
                    "54321",
                    "USA")

            ' Rebill Amount: $3.50
            ' Rebill Start Date: Jan. 5, 2015
            ' Rebill Frequency: 1 MONTH
            ' Rebill # of Cycles: 5
            payment.SetRebillingInformation(
                    "3.50",
                    "2015-01-05",
                    "1 MONTH",
                    "5")

            ' Phone #: 123-123-1234
            payment.SetPhone("1231231234")

            ' Email Address: test@bluepay.com
            payment.SetEmail("test@bluepay.com")

            ' Auth Amount: $0.00
            payment.Auth("0.00")

            payment.Process()

            ' Outputs response from BluePay gateway
            Console.Write("Transaction ID: " + payment.GetTransID() + Environment.NewLine)
            Console.Write("Rebill ID: " + payment.GetRebillID() + Environment.NewLine)
            Console.Write("AVS Result: " + payment.GetAVS() + Environment.NewLine)
            Console.Write("CVV2 Result: " + payment.GetCVV2() + Environment.NewLine)
            Console.Write("Authorization Code: " + payment.GetAuthCode() + Environment.NewLine)
            Console.Write("Message: " + payment.GetMessage() + Environment.NewLine)
            Console.Write("Status: " + payment.GetStatus() + Environment.NewLine)
        End Sub
    End Class
End Namespace