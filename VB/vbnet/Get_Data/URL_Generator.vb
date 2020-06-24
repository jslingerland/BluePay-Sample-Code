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

Namespace Sample

    Public Class URLGeneratorMethodSample

        Public Shared Sub run()

            Dim accountID As String = "Merchant's Account ID Here"
            Dim secretKey As String = "Merchant's Secret Key Here"
            Dim mode As String = "TEST" 

            Dim testURL As BluePay = New BluePay(
                accountID,
                secretKey,
                mode
            )
            
            Dim generatedURL As String = testURL.generateURL(
              merchantName:= "Test Merchant",
              returnURL:= "www.google.com",
              transactionType:= "SALE",
              acceptDiscover:= "Yes",
              acceptAmex:= "Yes",
              amount:= "99.99", 
              protectAmount:= "Yes",
              rebilling:= "Yes",
              rebProtect:= "Yes", 
              rebAmount:= "50", 
              rebCycles:= "12", 
              rebStartDate:= "1 MONTH", 
              rebFrequency:= "1 MONTH", 
              customID1:= "MyCustomID1.1234", 
              protectCustomID1:= "Yes", 
              customID2:= "MyCustomID2.12345678910", 
              protectCustomID2:= "Yes", 
              paymentTemplate:= "mobileform01", 
              receiptTemplate:= "defaultres2", 
              receiptTempRemoteURL:= ""
            )

            Console.WriteLine("Hosted Payment Form URL: " + generatedURL)
    
        End Sub
    End Class
End Namespace