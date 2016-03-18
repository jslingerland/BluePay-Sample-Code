' *
' * Bluepay VB.NET Sample code.
' *
' * This code sample runs a report that grabs data from the
' * BluePay gateway based on certain criteria. See comments below
' * on the details of the report.
' * If using TEST mode, only TEST transactions will be returned.
' *

Imports System
Imports vbnet.BPVB

Namespace GetData

    Public Class RetrieveTransactionData

        Public Shared Sub run()
            
            Dim accountID As String = "Merchant's Account ID Here"
            Dim secretKey As String = "Merchant's Secret Key Here"
            Dim mode As String = "TEST" 
            
            Dim query As BluePay = New BluePay(
                accountID,
                secretKey,
                mode
            )

          ' Report Stard Date: YYYY-MM-DD
          ' Report End Date: YYYY-MM-DD
          ' Also search subaccounts? Yes
          ' Output response without commas? Yes
          ' Do not include errored transactions? Yes
            query.getTransactionReport(
              reportStartDate:="2015-01-01", 
              reportEndDate:="2015-04-30", 
              queryByHierarchy:="1", 
              doNotEscape:="1", 
              excludeErrors:="1" 
            )
            
            query.process()

            Console.Write(query.response)

        End Sub
    End Class
End Namespace