' *
' * Bluepay VB.NET Sample code.
' *
' * Developed by Joel Tosi, Chris Jansen, and Justin Slingerland of Bluepay.
' *
' * This code is Free.  You may use it, modify it and redistribute it.
' * If you do make modifications that are useful, Bluepay would love it if you donated
' * them back to us!
' *

Imports System
Imports System.Web
Imports System.Text
Imports System.Security.Cryptography
Imports System.Net
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Security.Cryptography.X509Certificates
Imports System.Collections

Namespace BPVB

    ''' <summary>
    ''' This is the BluePayPayment object.
    ''' </summary>
    Public Class BluePayPayment_BP10Emu

        ' Required for every transaction
        Private accountID As String = ""
        Private URL As String = ""
        Private secretKey As String = ""
        Private mode As String = ""

        ' Required for auth or sale
        Private paymentAccount As String = ""
        Private cvv2 As String = ""
        Private cardExpire As String = ""
        Private paymentAccount As String = ""
        Private cvv2 As String = ""
        Private cardExpire As String = ""
        Private track1And2 As Regex = New Regex("(%B)\d{0,19}\^([\w\s]*)\/([\w\s]*)([\s]*)\^\d{7}\w*\?;\d{0,19}=\d{7}\w*\?")
        Private track2Only As Regex = New Regex(";\d{0,19}=\d{7}\w*\?")
        Private swipeData As String = ""
        Private name1 As String = ""
        Private name2 As String = ""
        Private addr1 As String = ""
        Private city As String = ""
        Private state As String = ""
        Private zip As String = ""
        Private routingNum As String = ""
        Private accountNum As String = ""
        Private accountType As String = ""
        Private docType As String = ""
        Private name1 As String = ""
        Private name2 As String = ""
        Private addr1 As String = ""
        Private city As String = ""
        Private state As String = ""
        Private zip As String = ""

        ' Optional for auth or sale
        Private addr2 As String = ""
        Private phone As String = ""
        Private email As String = ""
        Private country As String = ""

        ' Transaction variables
        Private amount As String = ""
        Private transType As String = ""
        Private paymentType As String = ""
        Private masterID As String = ""
        Private rebillID As String = ""

        ' Rebill variables
        Private doRebill As String = ""
        Private rebillAmount As String = ""
        Private rebillFirstDate As String = ""
        Private rebillExpr As String = ""
        Private rebillCycles As String = ""
        Private rebillNextAmount As String = ""
        Private rebillNextDate As String = ""
        Private rebillStatus As String = ""
        Private templateID As String = ""

        ' Level2 variables
        Private customID1 As String = ""
        Private customID2 As String = ""
        Private invoiceID As String = ""
        Private orderID As String = ""
        Private amountTip As String = ""
        Private amountTax As String = ""
        Private amountFood As String = ""
        Private amountMisc As String = ""
        Private memo As String = ""

        ' Report variables
        Private reportStartDate As String = ""
        Private reportEndDate As String = ""
        Private doNotEscape As String = ""
        Private queryBySettlement As String = ""
        Private queryByHierarchy As String = ""
        Private excludeErrors As String = ""

        Private TPS As String = ""

        Public response = ""

        Public Sub New(ByVal accID As String, ByVal secretKey As String, ByVal mode As String)
            Me.accountID = accID
            Me.secretKey = secretKey
            Me.mode = mode
        End Sub

        ''' <summary>
        ''' Sets Customer Information
        ''' </summary>
        ''' <param name="firstName"></param>
        ''' <param name="lastName"></param>
        ''' <param name="addr1"></param>
        ''' <param name="city"></param>
        ''' <param name="state"></param>
        ''' <param name="zip"></param>
        ''' 
        Public Sub SetCustomerInformation(ByVal firstName As String, ByVal lastName As String, ByVal addr1 As String, ByVal city As String,
                                          ByVal state As String, ByVal zip As String)
            Me.name1 = firstName
            Me.name2 = lastName
            Me.addr1 = addr1
            Me.city = city
            Me.state = state
            Me.zip = zip
        End Sub

        ''' <summary>
        ''' Sets Customer Information
        ''' </summary>
        ''' <param name="firstName"></param>
        ''' <param name="lastName"></param>
        ''' <param name="addr1"></param>
        ''' <param name="addr2"></param>
        ''' <param name="city"></param>
        ''' <param name="state"></param>
        ''' <param name="zip"></param>
        '''
        Public Sub SetCustomerInformation(ByVal firstName As String, ByVal lastName As String, ByVal addr1 As String, ByVal addr2 As String,
                                          ByVal city As String, ByVal state As String, ByVal zip As String)
            Me.name1 = firstName
            Me.name2 = lastName
            Me.addr1 = addr1
            Me.addr2 = addr2
            Me.city = city
            Me.state = state
            Me.zip = zip
        End Sub

        ''' <summary>
        ''' Sets Customer Information
        ''' </summary>
        ''' <param name="firstName"></param>
        ''' <param name="lastName"></param>
        ''' <param name="addr1"></param>
        ''' <param name="addr2"></param>
        ''' <param name="city"></param>
        ''' <param name="state"></param>
        ''' <param name="zip"></param>
        ''' <param name="country"></param>
        ''' 
        Public Sub SetCustomerInformation(ByVal firstName As String, ByVal lastName As String, ByVal addr1 As String, ByVal addr2 As String,
                                          ByVal city As String, ByVal state As String, ByVal zip As String, ByVal country As String)
            Me.name1 = firstName
            Me.name2 = lastName
            Me.addr1 = addr1
            Me.addr2 = addr2
            Me.city = city
            Me.state = state
            Me.zip = zip
            Me.country = country
        End Sub

        ''' <summary>
        ''' Sets Credit Card Information
        ''' </summary>
        ''' <param name="cardNum"></param>
        ''' <param name="cardExpire"></param>
        ''' 
        Public Sub SetCCInformation(ByVal cardNum As String, ByVal cardExpire As String)
            Me.paymentType = "CREDIT"
            Me.paymentAccount = cardNum
            Me.cardExpire = cardExpire
        End Sub

        ''' <summary>
        ''' Sets Credit Card Information
        ''' </summary>
        ''' <param name="cardNum"></param>
        ''' <param name="cardExpire"></param>
        ''' <param name="cvv2"></param>
        ''' 
        Public Sub SetCCInformation(ByVal cardNum As String, ByVal cardExpire As String, ByVal cvv2 As String)
            Me.paymentType = "CREDIT"
            Me.paymentAccount = cardNum
            Me.cardExpire = cardExpire
            Me.cvv2 = cvv2
        End Sub

        ''' <summary>
        ''' Sets Swipe Information Using Either Both Track 1 2, Or Just Track 2
        ''' </summary>
        ''' <param name="swipe"></param>
        ''' 
        Public Sub Swipe(ByVal swipe As String)
            Me.paymentType = "CREDIT"
            Me.swipeData = swipe
        End Sub

        ''' <summary>
        ''' Sets ACH Information
        ''' </summary>
        ''' <param name="routingNum"></param>
        ''' <param name="accNum"></param>
        ''' <param name="accType"></param>
        ''' <remarkscomment></remarks>
        '''
        Public Sub SetACHInformation(ByVal routingNum As String, ByVal accNum As String, ByVal accType As String)
            Me.paymentType = "ACH"
            Me.routingNum = routingNum
            Me.accountNum = accNum
            Me.accountType = accType
        End Sub

        ''' <summary>
        ''' Sets ACH Information
        ''' </summary>
        ''' <param name="routingNum"></param>
        ''' <param name="accNum"></param>
        ''' <param name="accType"></param>
        ''' <param name="docType"></param>
        ''' 
        Public Sub SetACHInformation(ByVal routingNum As String, ByVal accNum As String, ByVal accType As String, ByVal docType As String)
            Me.paymentType = "ACH"
            Me.routingNum = routingNum
            Me.accountNum = accNum
            Me.accountType = accType
            Me.docType = docType
        End Sub

        ''' <summary>
        ''' Sets Rebilling Cycle Information
        ''' </summary>
        ''' <param name="rebAmount"></param>
        ''' <param name="rebFirstDate"></param>
        ''' <param name="rebExpr"></param>
        ''' <param name="rebCycles"></param>
        ''' <remarks>
        ''' To be used with other functions for Setting up a transaction
        ''' </remarks>
        Public Sub SetRebillingInformation(ByVal rebAmount As String, ByVal rebFirstDate As String, ByVal rebExpr As String, ByVal rebCycles As String)
            Me.doRebill = "1"
            Me.rebillAmount = rebAmount
            Me.rebillFirstDate = rebFirstDate
            Me.rebillExpr = rebExpr
            Me.rebillCycles = rebCycles
        End Sub

        ''' <summary>
        ''' Updates Rebilling Cycle Information
        ''' </summary>
        ''' <param name="rebillID"></param>
        ''' <param name="rebNextDate"></param>
        ''' <param name="rebExpr"></param>
        ''' <param name="rebCycles"></param>
        ''' <param name="rebAmount"></param>
        ''' <param name="rebNextAmount"></param>
        ''' 
        Public Sub UpdateRebillingInformation(ByVal rebillID As String, ByVal rebNextDate As String, ByVal rebExpr As String, ByVal rebCycles As String,
                                              ByVal rebAmount As String, ByVal rebNextAmount As String)
            Me.transType = "SET"
            Me.rebillID = rebillID
            Me.rebillNextDate = rebNextDate
            Me.rebillExpr = rebExpr
            Me.rebillCycles = rebCycles
            Me.rebillAmount = rebAmount
            Me.rebillNextAmount = rebNextAmount
        End Sub

        ''' <summary>
        ''' Updates a rebilling cycle's payment information
        ''' </summary>
        ''' <param name="templateID"></param>
        ''' <remarks></remarks>
        Public Sub UpdateRebillPaymentInformation(ByVal templateID As String)
            Me.templateID = templateID
        End Sub

        ''' <summary>
        ''' Cancels Rebilling Cycle
        ''' </summary>
        ''' <param name="rebillID"></param>
        '''
        Public Sub CancelRebilling(ByVal rebillID As String)
            Me.transType = "SET"
            Me.rebillID = rebillID
            Me.rebillStatus = "stopped"
        End Sub

        ''' <summary>
        ''' Gets Rebilling Cycle's Status
        ''' </summary>
        ''' <param name="rebillID"></param>
        ''' <remarks></remarks>
        Public Sub GetRebillStatus(ByVal rebillID As String)
            Me.transType = "GET"
            Me.rebillID = rebillID
        End Sub

        ''' <summary>
        ''' Runs a Sale Transaction
        ''' </summary>
        ''' <param name="amount"></param>
        ''' 
        Public Sub Sale(ByVal amount As String)
            Me.transType = "SALE"
            Me.amount = amount
        End Sub

        ''' <summary>
        ''' Runs a Sale Transaction
        ''' </summary>
        ''' <param name="amount"></param>
        ''' <param name="masterID"></param>
        ''' 
        Public Sub Sale(ByVal amount As String, ByVal masterID As String)
            Me.transType = "SALE"
            Me.amount = amount
            Me.masterID = masterID
        End Sub

        ''' <summary>
        ''' Runs an Auth Transaction
        ''' </summary>
        ''' <param name="amount"></param>
        ''' 
        Public Sub Auth(ByVal amount As String)
            Me.transType = "AUTH"
            Me.amount = amount
        End Sub

        ''' <summary>
        ''' Runs an Auth Transaction
        ''' </summary>
        ''' <param name="amount"></param>
        ''' <param name="masterID"></param>
        ''' 
        Public Sub Auth(ByVal amount As String, ByVal masterID As String)
            Me.transType = "AUTH"
            Me.amount = amount
            Me.masterID = masterID
        End Sub

        ''' <summary>
        ''' Runs a Refund Transaction
        ''' </summary>
        ''' <param name="masterID"></param>
        ''' 
        Public Sub Refund(ByVal masterID As String)
            Me.transType = "REFUND"
            Me.masterID = masterID
        End Sub

        ''' <summary>
        ''' Runs a Refund Transaction
        ''' </summary>
        ''' <param name="masterID"></param>
        ''' <param name="amount"></param>
        ''' <remarks></remarks>
        Public Sub Refund(ByVal masterID As String, ByVal amount As String)
            Me.transType = "REFUND"
            Me.masterID = masterID
            Me.amount = amount
        End Sub

        ''' <summary>
        ''' Runs a Refund Transaction
        ''' </summary>
        ''' <param name="masterID"></param>
        ''' 
        Public Sub Capture(ByVal masterID As String)
            Me.transType = "CAPTURE"
            Me.masterID = masterID
        End Sub

        ''' <summary>
        ''' Runs a Capture Transaction
        ''' </summary>
        ''' <param name="masterID"></param>
        ''' <param name="amount"></param>
        ''' 
        Public Sub Capture(ByVal masterID As String, ByVal amount As String)
            Me.transType = "CAPTURE"
            Me.masterID = masterID
            Me.amount = amount
        End Sub

        ''' <summary>
        ''' Runs a Void Transaction
        ''' </summary>
        ''' <param name="masterID"></param>
        ''' 
        Public Sub Void(ByVal masterID As String)
            Me.transType = "VOID"
            Me.masterID = masterID
        End Sub

        ''' <summary>
        ''' Sets Custom ID Field
        ''' </summary>
        ''' <param name="customID1"></param>
        ''' 
        Public Sub SetCustomID1(ByVal customID1 As String)
            Me.customID1 = customID1
        End Sub

        ''' <summary>
        ''' Sets Custom ID2 Field
        ''' </summary>
        ''' <param name="customID2"></param>
        '''
        Public Sub SetCustomID2(ByVal customID2 As String)
            Me.customID2 = customID2
        End Sub

        ''' <summary>
        ''' Sets Invoice ID Field
        ''' </summary>
        ''' <param name="invoiceID"></param>
        ''' 
        Public Sub SetInvoiceID(ByVal invoiceID As String)
            Me.invoiceID = invoiceID
        End Sub

        ''' <summary>
        ''' Sets Order ID Field
        ''' </summary>
        ''' <param name="orderID"></param>
        ''' 
        Public Sub SetOrderID(ByVal orderID As String)
            Me.orderID = orderID
        End Sub

        ''' <summary>
        ''' Sets Amount Tip Field
        ''' </summary>
        ''' <param name="amountTip"></param>
        ''' 
        Public Sub SetAmountTip(ByVal amountTip As String)
            Me.amountTip = amountTip
        End Sub

        ''' <summary>
        ''' Sets Amount Tax Field
        ''' </summary>
        ''' <param name="amountTax"></param>
        ''' <remarks></remarks>
        Public Sub SetAmountTax(ByVal amountTax As String)
            Me.amountTax = amountTax
        End Sub

        ''' <summary>
        ''' Sets Amount Food Field
        ''' </summary>
        ''' <param name="amountFood"></param>
        ''' 
        Public Sub SetAmountFood(ByVal amountFood As String)
            Me.amountFood = amountFood
        End Sub

        ''' <summary>
        ''' Sets Amount Misc Field
        ''' </summary>
        ''' <param name="amountMisc"></param>
        ''' 
        Public Sub SetAmountMisc(ByVal amountMisc As String)
            Me.amountMisc = amountMisc
        End Sub

        ''' <summary>
        ''' Sets Memo Field
        ''' </summary>
        ''' <param name="memo"></param>
        ''' 
        Public Sub SetMemo(ByVal memo As String)
            Me.memo = memo
        End Sub

        ''' <summary>
        ''' Sets Phone Field
        ''' </summary>
        ''' <param name="phone"></param>
        ''' 
        Public Sub SetPhone(ByVal phone As String)
            Me.phone = phone
        End Sub

        ''' <summary>
        ''' Sets Email Field
        ''' </summary>
        ''' <param name="email"></param>
        ''' 
        Public Sub SetEmail(ByVal email As String)
            Me.email = email
        End Sub

        Public Sub Set_Param(ByVal Name As String, ByVal Value As String)
            Name = Value
        End Sub

        ''' <summary>
        ''' Calculates TAMPER_PROOF_SEAL for bp10emu API
        ''' </summary>
        '''
        Public Sub CalcTPS()
            Dim tps As String = Me.secretKey _
                        + Me.accountID _
                        + Me.transType _
                        + Me.amount _
                        + Me.doRebill _
                        + Me.rebillFirstDate _
                        + Me.rebillExpr _
                        + Me.rebillCycles _
                        + Me.rebillAmount _
                        + Me.masterID _
                        + Me.mode
            Dim md5 As MD5 = New MD5CryptoServiceProvider
            Dim hash() As Byte
            Dim encode As ASCIIEncoding = New ASCIIEncoding
            Dim buffer() As Byte = encode.GetBytes(tps)
            hash = md5.ComputeHash(buffer)
            Me.TPS = ByteArrayToString(hash)
        End Sub

        ''' <summary>
        ''' Calculates TAMPER_PROOF_SEAL for bp20rebadmin API
        ''' </summary>
        '''
        Public Sub CalcRebillTPS()
            Dim tps As String = Me.secretKey _
                        + Me.accountID _
                        + Me.transType _
                        + Me.rebillID
            Dim md5 As MD5 = New MD5CryptoServiceProvider
            Dim hash() As Byte
            Dim encode As ASCIIEncoding = New ASCIIEncoding
            Dim buffer() As Byte = encode.GetBytes(tps)
            hash = md5.ComputeHash(buffer)
            Me.TPS = ByteArrayToString(hash)
        End Sub

        'This is used to convert a byte array to a hex string
        Private Shared Function ByteArrayToString(ByVal arrInput() As Byte) As String
            Dim i As Integer
            Dim sOutput As StringBuilder = New StringBuilder(arrInput.Length)
            i = 0
            Do While (i < arrInput.Length)
                sOutput.Append(arrInput(i).ToString("X2"))
                i = (i + 1)
            Loop
            Return sOutput.ToString
        End Function

        Public Function Process() As String
            Dim postData As String = "MODE=" + HttpUtility.UrlEncode(Me.mode)
            If (Me.transType <> "SET" And Me.transType <> "GET") Then
                CalcTPS()
                Me.URL = "https://secure.bluepay.com/interfaces/bp10emu"
                postData = postData + "&MERCHANT=" + HttpUtility.UrlEncode(Me.accountID) + _
                "&TRANSACTION_TYPE=" + HttpUtility.UrlEncode(Me.transType) + _
                "&TAMPER_PROOF_SEAL=" + HttpUtility.UrlEncode(Me.TPS) + _
                "&NAME1=" + HttpUtility.UrlEncode(Me.name1) + _
                "&NAME2=" + HttpUtility.UrlEncode(Me.name2) + _
                "&AMOUNT=" + HttpUtility.UrlEncode(Me.amount) + _
                "&ADDR1=" + HttpUtility.UrlEncode(Me.addr1) + _
                "&ADDR2=" + HttpUtility.UrlEncode(Me.addr2) + _
                "&CITY=" + HttpUtility.UrlEncode(Me.city) + _
                "&STATE=" + HttpUtility.UrlEncode(Me.state) + _
                "&ZIPCODE=" + HttpUtility.UrlEncode(Me.zip) + _
                "&COMMENT=" + HttpUtility.UrlEncode(Me.memo) + _
                "&PHONE=" + HttpUtility.UrlEncode(Me.phone) + _
                "&EMAIL=" + HttpUtility.UrlEncode(Me.email) + _
                "&REBILLING=" + HttpUtility.UrlEncode(Me.doRebill) + _
                "&REB_FIRST_DATE=" + HttpUtility.UrlEncode(Me.rebillFirstDate) + _
                "&REB_EXPR=" + HttpUtility.UrlEncode(Me.rebillExpr) + _
                "&REB_CYCLES=" + HttpUtility.UrlEncode(Me.rebillCycles) + _
                "&REB_AMOUNT=" + HttpUtility.UrlEncode(Me.rebillAmount) + _
                "&RRNO=" + HttpUtility.UrlEncode(Me.masterID) + _
                "&PAYMENT_TYPE=" + HttpUtility.UrlEncode(Me.paymentType) + _
                "&INVOICE_ID=" + HttpUtility.UrlEncode(Me.invoiceID) + _
                "&ORDER_ID=" + HttpUtility.UrlEncode(Me.orderID) + _
                "&AMOUNT_TIP=" + HttpUtility.UrlEncode(Me.amountTip) + _
                "&AMOUNT_TAX=" + HttpUtility.UrlEncode(Me.amountTax) + _
                "&AMOUNT_FOOD=" + HttpUtility.UrlEncode(Me.amountFood) + _
                "&AMOUNT_MISC=" + HttpUtility.UrlEncode(Me.amountMisc)
                If (Me.swipeData <> "") Then
                    Dim matchTrack1And2 As Match = track1And2.Match(Me.swipeData)
                    Dim matchTrack2 As Match = track2Only.Match(Me.swipeData)
                    If matchTrack1And2.Success Then
                        postData = postData + "&SWIPE=" + HttpUtility.UrlEncode(Me.swipeData)
                    ElseIf matchTrack2.Success Then
                        postData = postData + "&TRACK2=" + HttpUtility.UrlEncode(Me.swipeData)
                    End If
                ElseIf (Me.paymentType = "CREDIT") Then
                    postData = postData + "&CC_NUM=" + HttpUtility.UrlEncode(Me.paymentAccount) + _
                    "&CC_EXPIRES=" + HttpUtility.UrlEncode(Me.cardExpire) + _
                    "&CVCVV2=" + HttpUtility.UrlEncode(Me.cvv2)
                Else
                    postData = postData + "&ACH_ROUTING=" + HttpUtility.UrlEncode(Me.routingNum) + _
                    "&ACH_ACCOUNT=" + HttpUtility.UrlEncode(Me.accountNum) + _
                    "&ACH_ACCOUNT_TYPE=" + HttpUtility.UrlEncode(Me.accountType) + _
                    "&DOC_TYPE=" + HttpUtility.UrlEncode(Me.docType)
                End If
            Else
                ' Calculate the Tamperproof Seal
                CalcRebillTPS()
                Me.URL = "https://secure.bluepay.com/interfaces/bp20rebadmin"
                postData = postData + "&ACCOUNT_ID=" + HttpUtility.UrlEncode(Me.accountID) + _
                "&TRANS_TYPE=" + HttpUtility.UrlEncode(Me.mode) + _
                "&TAMPER_PROOF_SEAL=" + HttpUtility.UrlEncode(Me.TPS) + _
                "&REBILL_ID=" + HttpUtility.UrlEncode(Me.rebillID) + _
                "&TEMPLATE_ID=" + HttpUtility.UrlEncode(Me.templateID) + _
                "&REB_EXPR=" + HttpUtility.UrlEncode(Me.rebillExpr) + _
                "&REB_CYCLES=" + HttpUtility.UrlEncode(Me.rebillCycles) + _
                "&REB_AMOUNT=" + HttpUtility.UrlEncode(Me.rebillAmount) + _
                "&NEXT_AMOUNT=" + HttpUtility.UrlEncode(Me.rebillNextAmount) + _
                "&STATUS=" + HttpUtility.UrlEncode(Me.rebillStatus)
            End If
                ' Create HTTPS POST object and send to BluePay
                Dim httpRequest As HttpWebRequest = HttpWebRequest.Create(Me.URL)
                httpRequest.Method = "POST"
                httpRequest.AllowAutoRedirect = False
                Dim byteArray As Byte() = Text.Encoding.UTF8.GetBytes(postData)
                httpRequest.ContentType = "application/x-www-form-urlencoded"
                httpRequest.ContentLength = byteArray.Length
                Dim dataStream As Stream = httpRequest.GetRequestStream()
                dataStream.Write(byteArray, 0, byteArray.Length)
                dataStream.Close()
                Try
                    Dim response As WebResponse = httpRequest.GetResponse()
                    ResponseParams(response)
                Catch e As WebException
                    Dim response As WebResponse = e.Response()
                    GetResponse(e)
                    response.Close()
                End Try
                dataStream.Close()
                Return GetStatus()
        End Function

        Public Sub GetResponse(ByVal request As WebRequest)
            Dim httpResponse As WebResponse = request.GetResponse()
            ResponseParams(httpResponse)
        End Sub

        Public Sub GetResponse(ByVal request As WebException)
            Dim httpResponse As WebResponse = request.Response()
            ResponseParams(httpResponse)
        End Sub

        Public Function ResponseParams(ByVal httpResponse As WebResponse) As String
            Dim dataStream As Stream = httpResponse.GetResponseStream()
            Dim reader As New StreamReader(dataStream)
            Dim responseFromServer As String = reader.ReadToEnd()
            Me.response = HttpUtility.UrlDecode(responseFromServer)
            reader.Close()
            Return responseFromServer
        End Function

        ''' <summary>
        ''' Returns STATUS from response
        ''' </summary>
        ''' 
        Public Function GetStatus() As String
            Dim r As Regex = New Regex("Result=([^&$]*)")
            Dim m As Match = r.Match(Me.response)
            If m.Success Then
                Return m.Value.Substring(7)
            End If
            r = New Regex("status=([^&$]+)")
            m = r.Match(response)
            If (m.Success) Then
                Return (m.Value.Substring(7))
            Else
                Return ""
            End If
        End Function

        ''' <summary>
        ''' Returns TRANS_ID from response
        ''' </summary>
        '''
        Public Function GetTransID() As String
            Dim r As Regex = New Regex("RRNO=([^&$]*)")
            Dim m As Match = r.Match(Me.response)
            If m.Success Then
                Return m.Value.Substring(5)
            Else
                Return ""
            End If
        End Function

        ''' <summary>
        ''' Returns MESSAGE from response
        ''' </summary>
        '''
        Public Function GetMessage() As String
            Dim r As Regex = New Regex("MESSAGE=([^&$]+)")
            Dim m As Match = r.Match(Me.response)
            If m.Success Then
                Return m.Value.Substring(8).Split("""")(0)
            Else
                Return ""
            End If
        End Function

        ''' <summary>
        ''' Returns CVV2 from response
        ''' </summary>
        '''
        Public Function GetCVV2() As String
            Dim r As Regex = New Regex("CVV2=([^&$]*)")
            Dim m As Match = r.Match(Me.response)
            If m.Success Then
                Return m.Value.Substring(5)
            Else
                Return ""
            End If
        End Function

        ''' <summary>
        ''' Returns AVS from response
        ''' </summary>
        '''
        Public Function GetAVS() As String
            Dim r As Regex = New Regex("AVS=([^&$]+)")
            Dim m As Match = r.Match(Me.response)
            If m.Success Then
                Return m.Value.Substring(4)
            Else
                Return ""
            End If
        End Function

        Public Function GetMaskedPaymentAccount() As String
            Dim r As Regex = New Regex("PAYMENT_ACCOUNT=([^&$]+)")
            Dim m As Match = r.Match(Me.response)
            If m.Success Then
                Return m.Value.Substring(16)
            Else
                Return ""
            End If
        End Function

        Public Function GetCardType() As String
            Dim r As Regex = New Regex("CARD_TYPE=([^&$]+)")
            Dim m As Match = r.Match(Me.response)
            If m.Success Then
                Return m.Value.Substring(10)
            Else
                Return ""
            End If
        End Function

        Public Function GetBank() As String
            Dim r As Regex = New Regex("BANK_NAME=([^&$]+)")
            Dim m As Match = r.Match(Me.response)
            If m.Success Then
                Return m.Value.Substring(10)
            Else
                Return ""
            End If
        End Function

        ''' <summary>
        ''' Returns AUTH_CODE from response
        ''' </summary>
        '''
        Public Function GetAuthCode() As String
            Dim r As Regex = New Regex("AUTH_CODE=([^&$]+)")
            Dim m As Match = r.Match(Me.response)
            If m.Success Then
                Return m.Value.Substring(10)
            Else
                Return ""
            End If
        End Function

        ''' <summary>
        ''' Returns REBID or rebill_id from response
        ''' </summary>
        '''
        Public Function GetRebillID() As String
            Dim r As Regex = New Regex("REBID=([^&$]+)")
            Dim m As Match = r.Match(Me.response)
            If m.Success Then
                Return m.Value.Substring(6)
            End If
            r = New Regex("rebill_id=([^&$]+)")
            m = r.Match(response)
            If (m.Success) Then
                Return (m.Value.Substring(10))
            Else
                Return ""
            End If
        End Function

        ''' <summary>
        ''' Returns creation_date from response
        ''' </summary>
        '''
        Public Function GetCreationDate() As String
            Dim r As Regex = New Regex("creation_date=([^&$]+)")
            Dim m As Match = r.Match(Me.response)
            If m.Success Then
                Return m.Value.Substring(14)
            Else
                Return ""
            End If
        End Function

        ''' <summary>
        ''' Returns next_date from response
        ''' </summary>
        '''
        Public Function GetNextDate() As String
            Dim r As Regex = New Regex("next_date=([^&$]+)")
            Dim m As Match = r.Match(Me.response)
            If m.Success Then
                Return m.Value.Substring(10)
            Else
                Return ""
            End If
        End Function

        ''' <summary>
        ''' Returns last_date from response
        ''' </summary>
        '''
        Public Function GetLastDate() As String
            Dim r As Regex = New Regex("last_date=([^&$]+)")
            Dim m As Match = r.Match(Me.response)
            If m.Success Then
                Return m.Value.Substring(9)
            Else
                Return ""
            End If
        End Function

        ''' <summary>
        ''' Returns sched_expr from response
        ''' </summary>
        '''
        Public Function GetSchedExpr() As String
            Dim r As Regex = New Regex("sched_expr=([^&$]+)")
            Dim m As Match = r.Match(Me.response)
            If m.Success Then
                Return m.Value.Substring(11)
            Else
                Return ""
            End If
        End Function

        ''' <summary>
        ''' Returns cycles_remain from response
        ''' </summary>
        '''
        Public Function GetCyclesRemain() As String
            Dim r As Regex = New Regex("cycles_remain=([^&$]+)")
            Dim m As Match = r.Match(Me.response)
            If m.Success Then
                Return m.Value.Substring(14)
            Else
                Return ""
            End If
        End Function

        ''' <summary>
        ''' Returns reb_amount from response
        ''' </summary>
        '''
        Public Function GetRebillAmount() As String
            Dim r As Regex = New Regex("reb_amount=([^&$]+)")
            Dim m As Match = r.Match(Me.response)
            If m.Success Then
                Return m.Value.Substring(11)
            Else
                Return ""
            End If
        End Function

        ''' <summary>
        ''' Returns next_amount from response
        ''' </summary>
        '''
        Public Function GetNextAmount() As String
            Dim r As Regex = New Regex("next_amount=([^&$]+)")
            Dim m As Match = r.Match(Me.response)
            If m.Success Then
                Return m.Value.Substring(12)
            Else
                Return ""
            End If
        End Function

    End Class
End Namespace


