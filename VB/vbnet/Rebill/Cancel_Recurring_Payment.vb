' *
' * Bluepay VB.NET Sample code.
' *
' * This code sample runs a $0.00 Credit Card Auth transaction
' * against a customer using test payment information.
' * See comments below on the details of the initial Setup of the
' * rebilling cycle.
' *

Imports System
Imports vbnet.BPVB

Namespace Rebill

    Public Class CancelRecurringPayment

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
                address1:="12345678 Test St.",
                address2:="Apt #500",
                city:="Testville",
                state:="IL",
                zipCode:="54321", 
                country:="USA",
                phone:="123-123-12345",
                email:="test@bluepay.com"
            )

            rebill.setCCInformation(
                ccNumber:="4111111111111111", 
                ccExpiration:="1215", 
                cvv2:="123" 
            )

            ' Rebill Start Date: YYYY-MM-DD
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

                Dim rebillCancel As BluePay = New BluePay(
                    accountID,
                    secretKey,
                    mode
                )

                ' Find rebill by id and cancel rebilling cycle
                rebillCancel.cancelRebilling(rebill.getRebillID())

                rebillCancel.process()

                Console.Write("Transaction ID: " + rebillCancel.getTransID() + Environment.NewLine)
                Console.Write("Rebill ID: " + rebillCancel.getRebillID() + Environment.NewLine)
                Console.Write("Transaction Status: " + rebillCancel.getStatus() + Environment.NewLine)
                Console.Write("Transaction Message: " + rebillCancel.getMessage() + Environment.NewLine)
                Console.Write("AVS Result: " + rebillCancel.getAVS() + Environment.NewLine)
                Console.Write("CVV2 Result: " + rebillCancel.getCVV2() + Environment.NewLine)
                Console.Write("Masked Payment Account: " + rebillCancel.getMaskedPaymentAccount() + Environment.NewLine)
                Console.Write("Card Type: " + rebillCancel.getCardType() + Environment.NewLine)
                Console.Write("Authorization Code: " + rebillCancel.getAuthCode() + Environment.NewLine)
            Else
                Console.Write("Transaction Error: " + rebill.getMessage() + Environment.NewLine)
            End If
        End Sub
    End Class
End Namespace