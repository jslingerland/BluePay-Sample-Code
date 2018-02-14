' *
' * Bluepay VB.NET Sample code.
' *
' * This code sample reads the values from a BP10emu redirect
' * and authenticates the message using the the BP_STAMP
' * provided in the response. Point the REDIRECT_URL of your 
' * BP10emu request to the URI prefix specified below.
' *

 Imports System
 Imports vbnet.BPVB
 Imports System.IO
 Imports System.Collections.Specialized
 Imports System.Web
 Imports System.Net

 Namespace GetData

     Public Class ValidateBPStamp

        Public Sub New()
            MyBase.New()
        End Sub

        Public Shared Sub run()
            Dim accountID As String = "Merchant's Account ID Here"
            Dim secretKey As String = "Merchant's Secret Key Here"
            Dim mode As String = "TEST" 
            Dim prefix As String = "Merchant's Target URI Prefix Here" ' Set the listener URI (and port if necessary)

            Dim listener As HttpListener = New HttpListener()
            listener.Prefixes.Add(prefix)

            Try
                'Listen for incoming data
                listener.Start()
            Catch ex As HttpListenerException
                Return
            End Try
            listener.Prefixes.Add(prefix)
            While (listener.IsListening)
                Dim context As HttpListenerContext = listener.GetContext()
                Dim responsePairs As NameValueCollection = context.Request.QueryString
                Dim msg As String
                If responsePairs.Item("BP_STAMP") <> Nothing Then ' Check whether BP_STAMP is provided
                    Dim bp As BluePay = New BluePay(
                    accountID,
                    secretKey,
                    mode
                    )

                    Dim bpStampString As String = ""
                    Dim separator() As String = {" "}
                    Dim bpStampDef As String = responsePairs.Item("BP_STAMP_DEF")
                    Dim bpStampFields() As String = bpStampDef.Split(separator, StringSplitOptions.RemoveEmptyEntries) ' Split BP_STAMP_DEF on whitespace
                    For Each field As String in bpStampFields
                        bpStampString = bpStampString + responsePairs.Item(field) ' Concatenate values used to calculate expected BP_STAMP
                    Next
                    Dim tpsHashType As String = responsePairs.Item("TPS_HASH_TYPE")
                    Dim expectedStamp As String = bp.generateTPS(bpStampString, tpsHashType) ' Calculate expected BP_STAMP using hash function specified in response
                    If expectedStamp = responsePairs.Item("BP_STAMP") Then ' Compare expected BP_STAMP with received BP_STAMP
                        ' Validate BP_STAMP and reads the response results
                        msg = "VALID BP_STAMP: TRUE" & vbNewLine
                        For Each key As String in responsePairs
                            msg = msg + (key + ": " + responsePairs.Item(key) & vbNewLine)
                        Next
                    Else
                        msg = "ERROR: BP_STAMP VALUES DO NOT MATCH" & vbNewLine
                    End If
                Else
                    msg = "ERROR: BP_STAMP NOT FOUND. CHECK MESSAGE & RESPONSEVERSION"
                End If

                ' Return HTTP Status of 200 to BluePay
                context.Response.ContentLength64 = msg.Length
                context.Response.StatusCode = 200

                Using stream As Stream = context.Response.OutputStream
                    Using writer As StreamWriter = new StreamWriter(stream)
                        writer.Write(msg)
                    End Using
                End Using
            End While
            listener.Close()
        End Sub
    End Class
 End Namespace