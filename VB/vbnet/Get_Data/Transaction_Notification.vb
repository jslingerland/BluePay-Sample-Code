' *
' * Bluepay VB.NET Sample code.
' *
' * This code sample shows a very based approach
' * on handling data that is posted to a script running
' * a merchant's server after a transaction is processed
' * through their BluePay gateway account.
' *

 Imports System
 Imports vbnet.BPVB
 Imports System.IO
 Imports System.Collections.Specialized
 Imports System.Web
 Imports System.Net

 Namespace GetData

     Public Class TransactionNotification

        Public Sub New()
            MyBase.New()
        End Sub

        Public Shared Sub run()
            Dim accountID As String = "Merchant's Account ID Here"
            Dim secretKey As String = "Merchant's Secret Key Here"
            Dim mode As String = "TEST" 

            Dim tps As BluePay = New BluePay(
                accountID,
                secretKey,
                mode
            )

            Dim listener As HttpListener = New HttpListener()
            Dim prefix As String = "http://localhost:8080/" ' Change this to your listener URL (and optionally port)
            Dim responseString As String = ""

            Try
                'Listen for incoming data
                listener.Start()
            Catch ex As HttpListenerException
                Return
            End Try
            listener.Prefixes.Add(prefix)
            While (listener.IsListening)
                Dim context As HttpListenerContext = listener.GetContext()
                Dim response As HttpListenerResponse = context.Response
                Dim body As String = New StreamReader(context.Request.InputStream).ReadToEnd()

                ' Return HTTP Status of 200 to BluePay
                context.Response.StatusCode = 200
                context.Response.KeepAlive = False
                context.Response.ContentLength64 = body.Length

                ' Get Response
                Dim buffer() As Byte = System.Text.Encoding.UTF8.GetBytes(body)
                response.ContentLength64 = buffer.Length
                Dim output As System.IO.Stream = response.OutputStream
                output.Write(buffer, 0, buffer.Length)
                responseString = System.Text.Encoding.ASCII.GetString(buffer)
                context.Response.Close()

                ' Parse data into a NVP collection
                Dim vals As NameValueCollection = HttpUtility.ParseQueryString(responseString)
                Dim bpStampDef As String = vals("BP_STAMP_DEF")
                Dim tpsHashType As String = vals("TPS_HASH_TYPE")
                Dim bpStampVals As String
                Dim strArr() As String
                Dim i As Integer
                strArr = bpStampDef.Split(" ")
                For i = 0 To strArr.Length - 1
                    bpStampVals += vals(strArr(i))
                Next

                ' calculate the expected BP_STAMP
                Dim bpStamp As String = tps.generateTPS(bpStampVals, tpsHashType)

                ' Output data if the expected bp_stamp matches the actual BP_STAMP
                If bpStamp = vals("BP_STAMP") Then
                    Console.Write("Transaction ID: " + vals("trans_id"))
                    Console.Write("Transaction Status: " + vals("trans_stats"))
                    Console.Write("Transaction Type: " + vals("trans_type"))
                    Console.Write("Transaction Amount: " + vals("amount"))
                    Console.Write("Rebill ID: " + vals("rebill_id"))
                Else
                    Console.Write("ERROR IN RECEIVING DATA FROM BLUEPAY")
                End If

            End While
            listener.Close()
        End Sub
    End Class
 End Namespace
''