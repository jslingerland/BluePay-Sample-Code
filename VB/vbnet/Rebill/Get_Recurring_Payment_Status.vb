' *
' * Bluepay VB.NET Sample code.
' *
' * This code sample runs a $0.00 Credit Card Auth transaction
' * against a customer using test rebill information.
' * Once the rebilling cycle is created, this sample shows how to
' * Get information back on this rebilling cycle.
' * See comments below on the details of the initial Setup of the
' * rebilling cycle.
' *

Imports System
Imports vbnet.BPVB

Namespace Rebill

    Public Class GetRecurringPaymentStatus

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
                address1:="123 Test St.",
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

                Dim rebillStatus As BluePay = New BluePay(
                    accountID,
                    secretKey,
                    mode
                )

                ' Find the rebill by ID and get rebill status 
                rebillStatus.getRebillStatus(rebill.getRebillID())

                ' Makes the API Request to get the rebill status
                rebillStatus.process()

                Console.Write("Rebill Status: " + rebillStatus.getStatus() + Environment.NewLine)
                Console.Write("Rebill ID: " + rebillStatus.getRebillID() + Environment.NewLine)
                Console.Write("Rebill Creation Date: " + rebillStatus.getCreationDate() + Environment.NewLine)
                Console.Write("Rebill Next Date: " + rebillStatus.getNextDate() + Environment.NewLine)
                Console.Write("Rebill Last Date: " + rebillStatus.getLastDate() + Environment.NewLine)
                Console.Write("Rebill Schedule Expression: " + rebillStatus.getSchedExpr() + Environment.NewLine)
                Console.Write("Rebill Cycles Remaining: " + rebillStatus.getCyclesRemain() + Environment.NewLine)
                Console.Write("Rebill Amount: " + rebillStatus.getRebillAmount() + Environment.NewLine)
                Console.Write("Rebill Next Amount: " + rebillStatus.getNextAmount() + Environment.NewLine)
            Else
                Console.Write(rebill.getMessage())
            End If
        End Sub
    End Class
End Namespace