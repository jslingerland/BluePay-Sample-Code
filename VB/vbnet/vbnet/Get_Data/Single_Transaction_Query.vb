' *
' * Bluepay VB.NET Sample code.
' *
' * This code sample runs a report that grabs a single transaction
' * from the BluePay gateway based on certain criteria.
' * See comments below on the details of the report.
' * If using TEST mode, only TEST transactions will be returned.
' *

Imports System
Imports vbnet.BPVB

Namespace GetData

    Public Class SingleTransactionQuery

        Public Shared Sub run()

            Dim accountID As String = "Merchant's Account ID Here"
            Dim secretKey As String = "Merchant's Secret Key Here"
            Dim mode As String = "TEST" 

            Dim query As BluePay = New BluePay(
                accountID,
                secretKey,
                mode
            )

            ' Transaction ID: "100233712957"
            ' Report Stard Date: YYYY-MM-DD
            ' Report End Date: YYYY-MM-DD
            ' Do not include errored transactions? Yes
            query.getSingleTransactionQuery(
              transactionID:= "Transaction ID here", 
              reportStartDate:= "2015-01-01", 
              reportEndDate:= "2015-05-30", 
              excludeErrors:= "1" 
            )

            query.process()

            Console.Write(query.response)
    
        End Sub
    End Class
End Namespace