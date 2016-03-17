' *
' * Bluepay VB.NET Sample code.
' *
' * This code sample runs a report that grabs data from the
' * BluePay gateway based on certain criteria. See comments below
' * on the details of the report.
' * If using TEST mode, only TEST transactions will be returned.
' *

Imports System
Imports BluePay.BPVB

Namespace BP10Emu

    Public Class Retrieve_Transaction_Data

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
            Dim report As BluePayPayment_BP10Emu = New BluePayPayment_BP10Emu(
                    accountID,
                    secretKey,
                    mode)

            ' Report Start Date: Jan. 1, 2013
            ' Report End Date: Jan. 15, 2013
            ' Also search subaccounts? Yes
            ' Output response without commas? Yes
            ' Do not include errored transactions? Yes
            report.GetTransactionReport(
                "2013-01-01",
                "2013-01-15",
                "1",
                "1",
                "1")

            report.Process()

            ' Outputs response from BluePay gateway
            Console.Write(report.response)
        End Sub
    End Class
End Namespace