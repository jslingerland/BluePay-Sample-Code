' *
' * Bluepay VB.NET Sample code.
' *
' * This code sample runs a $3.00 ACH Sale transaction
' * against a customer using test payment information. See comments below
' * on the details of the initial Setup of the rebilling cycle.
' *

Imports System
Imports BluePay.BPVB

Namespace BP10Emu

    Public Class Run_ACH_Payment_Recurring

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

            ' Routing Number: 123123123
            ' Account Number: 0523421
            ' Account Type: Checking
            ' ACH Document Type: WEB
            payment.SetACHInformation(
                    "123123123",
                    "0523421",
                    "C",
                    "WEB")

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

            ' Sale Amount: $3.00
            payment.Sale("3.00")

            payment.Process()

            ' Outputs response from BluePay gateway
            Console.Write("Transaction ID: " + payment.GetTransID() + Environment.NewLine)
            Console.Write("Rebill ID: " + payment.GetRebillID() + Environment.NewLine)
            Console.Write("Message: " + payment.GetMessage() + Environment.NewLine)
            Console.Write("Status: " + payment.GetStatus() + Environment.NewLine)
            Console.Write("AVS Result: " + payment.GetAVS() + Environment.NewLine)
            Console.Write("CVV2 Result: " + payment.GetCVV2() + Environment.NewLine)
            Console.Write("Masked Payment Account: " + payment.GetMaskedPaymentAccount() + Environment.NewLine)
            Console.Write("Bank Name: " + payment.GetBank() + Environment.NewLine)
            Console.Write("Authorization Code: " + payment.GetAuthCode() + Environment.NewLine)
        End Sub
    End Class
End Namespace