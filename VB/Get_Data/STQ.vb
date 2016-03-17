' *
' * Bluepay VB.NET Sample code.
' *
' * This code sample runs a report that grabs a single transaction
' * from the BluePay gateway based on certain criteria.
' * See comments below on the details of the report.
' * If using TEST mode, only TEST transactions will be returned.
' *

Imports System
Imports BluePay.BPVB

Namespace BP10Emu

    Public Class Single_Trans_Query

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
            Dim stq As BluePayPayment_BP10Emu = New BluePayPayment_BP10Emu(
                    accountID,
                    secretKey,
                    mode)

            ' Search Date Start: Jan. 1, 2013
            ' Search Date End: Jan 15, 2013
            ' Do not include errored transactions in search? Yes
            stq.GetSingleTransQuery(
                "2013-01-01",
                "2013-01-15",
                "1")
            stq.QueryByTransactionID("ENTER A TRANSACTION ID HERE")
            stq.Process()

            ' Outputs response from BluePay gateway
            Console.Write(stq.response)
        End Sub
    End Class
End Namespace