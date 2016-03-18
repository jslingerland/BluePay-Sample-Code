' *
' * Bluepay VB.NET Sample code.
' *
' * This code sample shows a very based approach
' * on handling data that is posted to a script running
' * a merchant's server after a transaction is processed
' * through their BluePay gateway account.
' *

' Imports System
' Imports vbnet.BPVB
' Imports System.IO
' Imports System.Collections.Specialized
' Imports System.Web
' Imports System.Net

' Namespace GetData

'     Public Class TransactionNotification

'         Public Sub New()
'             MyBase.New()
'         End Sub

'         Public Shared Sub run()
'             Dim listener As HttpListener = New HttpListener()
'             Dim secretKey As String = ""
'             Dim response As String = ""

'             Try
'                 'Listen for incoming data
'                 listener.Start()
'             Catch ex As HttpListenerException
'                 Return
'             End Try
'             While (listener.IsListening)
'                 Dim context = listener.GetContext()
'                 Dim body = New StreamReader(context.Request.InputStream).ReadToEnd()

'                 Dim b() As Byte = System.Text.Encoding.UTF8.GetBytes("ACK")

'                 ' Return HTTP Status of 200 to BluePay
'                 context.Response.StatusCode = 200
'                 context.Response.KeepAlive = False
'                 context.Response.ContentLength64 = body.Length

'                 Dim output = context.Response.OutputStream
'                 output.Write(b, 0, b.Length)

'                 ' Get Response
'                 Using reader As New System.IO.StreamReader(output)
'                     response = reader.ReadToEnd()
'                 End Using
'                 context.Response.Close()
'             End While
'             listener.Close()

'             ' Parse data into a NVP collection
'             Dim vals As NameValueCollection = HttpUtility.ParseQueryString(response)
'             Dim transID As String = vals("trans_id")
'             Dim transStatus As String = vals("trans_stats")
'             Dim transType As String = vals("trans_type")
'             Dim amount As String = vals("amount")
'             Dim batchID As String = vals("batch_id")
'             Dim batchStatus As String = vals("batch_status")
'             Dim totalCount As String = vals("total_count")
'             Dim totalAmount As String = vals("total_amount")
'             Dim batchUploadID As String = vals("batch_upload_id")
'             Dim rebillID As String = vals("rebill_id")
'             Dim rebillAmount As String = vals("rebill_amount")
'             Dim rebillStatus As String = vals("rebill_status")

'             ' calculate the expected BP_STAMP
'             Dim bpStamp = calcTransNotifyTPS(secretKey,
'                 vals("trans_id"),
'                 vals("trans_stats"),
'                 vals("trans_type"),
'                 vals("amount"),
'                 vals("batch_id"),
'                 vals("batch_status"),
'                 vals("total_count"),
'                 vals("total_amount"),
'                 vals("batch_upload_id"),
'                 vals("rebill_id"),
'                 vals("rebill_amount"),
'                 vals("rebill_status"))

'             ' Output data if the expected bp_stamp matches the actual BP_STAMP
'             If bpStamp = vals("bp_stamp") Then
'                 Console.Write("Transaction ID: " + transID)
'                 Console.Write("Transaction Status: " + transStatus)
'                 Console.Write("Transaction Type: " + transType)
'                 Console.Write("Transaction Amount: " + amount)
'                 Console.Write("Rebill ID: " + rebillID)
'                 Console.Write("Rebill Amount: " + rebillAmount)
'                 Console.Write("Rebill Status: " + rebillStatus)
'             Else
'                 Console.Write("ERROR IN RECEIVING DATA FROM BLUEPAY")
'             End If

'         End Sub
'     End Class
' End Namespace