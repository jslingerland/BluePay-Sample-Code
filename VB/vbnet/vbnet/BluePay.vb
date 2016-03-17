' *
' * Bluepay VB.NET Sample code.
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
    Public Class BluePay

        ' Required for every transaction
        Private accountID As String = ""
        Private URL As String = ""
        Private secretKey As String = ""
        Private mode As String = ""

        ' Required for auth or sale
        Private paymentAccount As String = ""
        Private cardExpire As String = ""
        Private cvv2 As String = ""
        Private track1And2 As Regex = New Regex("(%B)\d{0,19}\^([\w\s]*)\/([\w\s]*)([\s]*)\^\d{7}\w*\?;\d{0,19}=\d{7}\w*\?")
        Private track2Only As Regex = New Regex(";\d{0,19}=\d{7}\w*\?")
        Private swipeData As String = ""
        Private name1 As String = ""
        Private name2 As String = ""
        Private addr1 As String = ""
        Private addr2 As String = ""
        Private city As String = ""
        Private state As String = ""
        Private zip As String = ""
        Private routingNum As String = ""
        Private accountNum As String = ""
        Private accountType As String = ""
        Private docType As String = ""

        ' Optional for auth or sale
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
        Private id As String = ""
        Private reportStartDate As String = ""
        Private reportEndDate As String = ""
        Private doNotEscape As String = ""
        Private queryBySettlement As String = ""
        Private queryByHierarchy As String = ""
        Private excludeErrors As String = ""

        Private TPS As String = ""
        Private api As String = ""
        Public response As String = ""

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
        ''' <param name="addr2"></param>
        ''' <param name="city"></param>
        ''' <param name="state"></param>
        ''' <param name="zip"></param>
        ''' <param name="country"></param>
        ''' <param name="phone"></param>
        ''' <param name="email"></param>
        ''' 
        Public Sub setCustomerInformation(Optional ByVal firstName As String = "", Optional ByVal lastName As String = "", Optional ByVal address1 As String = "", Optional ByVal address2 As String = "", Optional ByVal city As String = "", Optional ByVal state As String = "", Optional ByVal zipCode As String = "", Optional ByVal country As String = "", Optional ByVal phone As String = "", Optional ByVal email As String = "")
            Me.name1 = firstName
            Me.name2 = lastName
            Me.addr1 = address1
            Me.addr2 = address2
            Me.city = city
            Me.state = state
            Me.zip = zipCode
            Me.country = country
            Me.phone = phone
            Me.email = email
        End Sub

        ''' <summary>
        ''' Sets Credit Card Information
        ''' </summary>
        ''' <param name="cardNum"></param>
        ''' <param name="cardExpire"></param>
        ''' <param name="cvv2"></param>
        ''' 
        Public Sub setCCInformation(Optional ByVal ccNumber As String = "", Optional ByVal ccExpiration As String = "", Optional ByVal cvv2 As String = "")
            Me.paymentType = "CREDIT"
            Me.paymentAccount = ccNumber
            Me.cardExpire = ccExpiration
            Me.cvv2 = cvv2
        End Sub

        ''' <summary>
        ''' Sets Swipe Information Using Either Both Track 1 2, Or Just Track 2
        ''' </summary>
        ''' <param name="swipe"></param>
        ''' 
        Public Sub swipe(ByVal swipe As String)
            Me.paymentType = "CREDIT"
            Me.swipeData = swipe
        End Sub

        ''' <summary>
        ''' Sets ACH Information
        ''' </summary>
        ''' <param name="routingNum"></param>
        ''' <param name="accNum"></param>
        ''' <param name="accType"></param>
        ''' <param name="docType"></param>
        ''' 
        Public Sub setACHInformation(ByVal routingNum As String, ByVal accNum As String, ByVal accType As String, Optional ByVal docType As String = "")
            Me.paymentType = "ACH"
            Me.routingNum = routingNum
            Me.accountNum = accNum
            Me.accountType = accType
            Me.docType = docType 'optional'
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
        Public Sub setRebillingInformation(ByVal rebAmount As String, ByVal rebFirstDate As String, ByVal rebExpr As String, ByVal rebCycles As String)
            Me.doRebill = "1"
            Me.rebillFirstDate = rebFirstDate
            Me.rebillExpr = rebExpr
            Me.rebillCycles = rebCycles
            Me.rebillAmount = rebAmount
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
        ''' <param name="templateID"></param>
        ''' 
        Public Sub updateRebillingInformation(
            ByVal rebillID As String, 
            Optional ByVal rebNextDate As String ="", 
            Optional ByVal rebExpr As String ="", 
            Optional ByVal rebCycles As String ="", 
            Optional ByVal rebAmount As String="", 
            Optional ByVal rebNextAmount As String ="", 
            Optional ByVal templateID As String=""
        )
            Me.transType = "SET"
            Me.api = "bp20rebadmin"
            Me.rebillID = rebillID
            Me.rebillNextDate = rebNextDate
            Me.rebillExpr = rebExpr
            Me.rebillCycles = rebCycles
            Me.rebillAmount = rebAmount
            Me.rebillNextAmount = rebNextAmount
            Me.templateID = templateID
        End Sub

        ''' <summary>
        ''' Cancels Rebilling Cycle
        ''' </summary>
        ''' <param name="rebillID"></param>
        '''
        Public Sub cancelRebilling(ByVal rebillID As String)
            Me.transType = "SET"
            Me.rebillStatus = "stopped"
            Me.rebillID = rebillID
            Me.api = "bp20rebadmin"
        End Sub

        ''' <summary>
        ''' Gets Rebilling Cycle's Status
        ''' </summary>
        ''' <param name="rebillID"></param>
        ''' <remarks></remarks>
        Public Sub getRebillStatus(ByVal rebillID As String)
            Me.transType = "GET"
            Me.rebillID = rebillID
            Me.api = "bp20rebadmin"
        End Sub

        ''' <summary>
        ''' Runs a Sale Transaction
        ''' </summary>
        ''' <param name="amount"></param>
        ''' 
        Public Sub sale(ByVal amount As String)
            Me.transType = "SALE"
            Me.amount = amount
            Me.api = "bp10emu"
        End Sub

        ''' <summary>
        ''' Runs a Sale Transaction
        ''' </summary>
        ''' <param name="amount"></param>
        ''' <param name="masterID"></param>
        ''' 
        Public Sub sale(ByVal amount As String, ByVal transactionID As String)
            Me.transType = "SALE"
            Me.amount = amount
            Me.masterID = transactionID
            Me.api = "bp10emu"
        End Sub

        ''' <summary>
        ''' Runs an Auth Transaction
        ''' </summary>
        ''' <param name="amount"></param>
        ''' 
        Public Sub auth(ByVal amount As String)
            Me.transType = "AUTH"
            Me.amount = amount
            Me.api = "bp10emu"
        End Sub

        ''' <summary>
        ''' Runs an Auth Transaction
        ''' </summary>
        ''' <param name="amount"></param>
        ''' <param name="masterID"></param>
        ''' 
        Public Sub auth(ByVal amount As String, ByVal transactionID As String)
            Me.transType = "AUTH"
            Me.amount = amount
            Me.masterID = transactionID
            Me.api = "bp10emu"
        End Sub

        ''' <summary>
        ''' Runs a Refund Transaction
        ''' </summary>
        ''' <param name="masterID"></param>
        ''' 
        Public Sub refund(ByVal masterID As String)
            Me.transType = "REFUND"
            Me.masterID = masterID
            Me.api = "bp10emu"
        End Sub

        ''' <summary>
        ''' Runs a Refund Transaction
        ''' </summary>
        ''' <param name="masterID"></param>
        ''' <param name="amount"></param>
        ''' <remarks></remarks>
        Public Sub refund(ByVal transactionID As String, ByVal amount As String)
            Me.transType = "REFUND"
            Me.masterID = transactionID
            Me.amount = amount
            Me.api = "bp10emu"
        End Sub

        ''' <summary>
        ''' Runs a Refund Transaction
        ''' </summary>
        ''' <param name="masterID"></param>
        ''' 
        Public Sub capture(ByVal masterID As String)
            Me.transType = "CAPTURE"
            Me.masterID = masterID
            Me.api = "bp10emu"
        End Sub

        ''' <summary>
        ''' Runs a Capture Transaction
        ''' </summary>
        ''' <param name="masterID"></param>
        ''' <param name="amount"></param>
        ''' 
        Public Sub capture(ByVal masterID As String, ByVal amount As String)
            Me.transType = "CAPTURE"
            Me.masterID = masterID
            Me.amount = amount
            Me.api = "bp10emu"
        End Sub

        ''' <summary>
        ''' Runs a Void Transaction
        ''' </summary>
        ''' <param name="masterID"></param>
        ''' 
        Public Sub void(ByVal masterID As String)
            Me.transType = "VOID"
            Me.masterID = masterID
            Me.api = "bp10emu"
        End Sub

        ''' <summary>
        ''' Sets Custom ID Field
        ''' </summary>
        ''' <param name="customID1"></param>
        ''' 
        Public Sub setCustomID1(ByVal customID1 As String)
            Me.customID1 = customID1
        End Sub

        ''' <summary>
        ''' Sets Custom ID2 Field
        ''' </summary>
        ''' <param name="customID2"></param>
        '''
        Public Sub setCustomID2(ByVal customID2 As String)
            Me.customID2 = customID2
        End Sub

        ''' <summary>
        ''' Sets Invoice ID Field
        ''' </summary>
        ''' <param name="invoiceID"></param>
        ''' 
        Public Sub setInvoiceID(ByVal invoiceID As String)
            Me.invoiceID = invoiceID
        End Sub

        ''' <summary>
        ''' Sets Order ID Field
        ''' </summary>
        ''' <param name="orderID"></param>
        ''' 
        Public Sub setOrderID(ByVal orderID As String)
            Me.orderID = orderID
        End Sub

        ''' <summary>
        ''' Sets Amount Tip Field
        ''' </summary>
        ''' <param name="amountTip"></param>
        ''' 
        Public Sub setAmountTip(ByVal amountTip As String)
            Me.amountTip = amountTip
        End Sub

        ''' <summary>
        ''' Sets Amount Tax Field
        ''' </summary>
        ''' <param name="amountTax"></param>
        ''' <remarks></remarks>
        Public Sub setAmountTax(ByVal amountTax As String)
            Me.amountTax = amountTax
        End Sub

        ''' <summary>
        ''' Sets Amount Food Field
        ''' </summary>
        ''' <param name="amountFood"></param>
        ''' 
        Public Sub setAmountFood(ByVal amountFood As String)
            Me.amountFood = amountFood
        End Sub

        ''' <summary>
        ''' Sets Amount Misc Field
        ''' </summary>
        ''' <param name="amountMisc"></param>
        ''' 
        Public Sub setAmountMisc(ByVal amountMisc As String)
            Me.amountMisc = amountMisc
        End Sub

        ''' <summary>
        ''' Sets Memo Field
        ''' </summary>
        ''' <param name="memo"></param>
        ''' 
        Public Sub setMemo(ByVal memo As String)
            Me.memo = memo
        End Sub

        ''' <summary>
        ''' Sets Phone Field
        ''' </summary>
        ''' <param name="phone"></param>
        ''' 
        Public Sub setPhone(ByVal phone As String)
            Me.phone = phone
        End Sub

        ''' <summary>
        ''' Sets Email Field
        ''' </summary>
        ''' <param name="email"></param>
        ''' 
        Public Sub setEmail(ByVal email As String)
            Me.email = email
        End Sub

        Public Sub set_Param(ByVal Name As String, ByVal Value As String)
            Name = Value
        End Sub


        ''' <summary>
        ''' Retrieve Single Transaction Data
        ''' </summary>
        ''' <param name="reportStartDate"></param>
        ''' <param name="reportEndDate"></param>
        ''' <param name="transactionID"></param>
        ''' <param name="excludeErrors"></param>
        ''' 
        Public Sub getSingleTransactionQuery(
            ByVal reportStartDate As String,
            ByVal reportEndDate As String, 
            ByVal transactionID As String, 
            Optional ByVal excludeErrors As String = "" 
            )
            Me.reportStartDate = reportStartDate
            Me.reportEndDate = reportEndDate
            Me.id = transactionID
            Me.excludeErrors = excludeErrors
            Me.api = "stq"
        End Sub

        ''' <summary>
        ''' Retrieve Transaction Data
        ''' </summary>
        ''' <param name="reportStartDate"></param>
        ''' <param name="reportEndDate"></param>
        ''' <param name="queryByHierarchy"></param>
        ''' <param name="doNotEscape"></param>
        ''' <param name="excludeErrors"></param>
        ''' 
        Public Sub getTransactionReport(
            ByVal reportStartDate As String,
            ByVal reportEndDate As String, 
            Optional ByVal queryByHierarchy As String="", 
            Optional ByVal doNotEscape As String="", 
            Optional ByVal excludeErrors As String = "" 
            )
            Me.queryBySettlement = "0"
            Me.api = "bpdailyreport2"
            Me.reportStartDate = reportStartDate
            Me.reportEndDate = reportEndDate
            Me.queryByHierarchy = queryByHierarchy
            Me.doNotEscape = doNotEscape
            Me.excludeErrors = excludeErrors
        End Sub

                ''' <summary>
        ''' Retrieves Settled Transactions 
        ''' </summary>
        ''' <param name="reportStartDate"></param>
        ''' <param name="reportEndDate"></param>
        ''' <param name="queryByHierarchy"></param>
        ''' <param name="doNotEscape"></param>
        ''' <param name="excludeErrors"></param>
        ''' 
        Public Sub getSettledTransactionReport(
            ByVal reportStartDate As String,
            ByVal reportEndDate As String, 
            Optional ByVal queryByHierarchy As String="", 
            Optional ByVal doNotEscape As String="", 
            Optional ByVal excludeErrors As String = "" 
            )
            Me.queryBySettlement = "1"
            Me.api = "bpdailyreport2"
            Me.reportStartDate = reportStartDate
            Me.reportEndDate = reportEndDate
            Me.queryByHierarchy = queryByHierarchy
            Me.doNotEscape = doNotEscape
            Me.excludeErrors = excludeErrors
        End Sub



        ''' <summary>
        ''' Calculates TAMPER_PROOF_SEAL for bp10emu API
        ''' </summary>
        '''
        Public Sub calcTPS()
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
        ''' Calculates TAMPER_PROOF_SEAL for stq and bpdailyreport2 API
        ''' </summary>
        '''
        Public Sub calcReportTPS()
            Dim tps As String = Me.secretKey _
                        + Me.accountID _
                        + Me.reportStartDate _
                        + Me.reportEndDate
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
        Public Sub calcRebillTPS()
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

        Public Function process() As String
            Dim postData As String = "MODE=" + HttpUtility.UrlEncode(Me.mode)
            'If (Me.transType <> "SET" And Me.transType <> "GET") Then
            If (Me.API = "bp10emu") Then
                calcTPS()
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
                "&AMOUNT_MISC=" + HttpUtility.UrlEncode(Me.amountMisc) + _
                "&CUSTOM_ID=" + HttpUtility.UrlEncode(Me.customID1) + _
                "&CUSTOM_ID2=" + HttpUtility.UrlEncode(Me.customID2)
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
                ElseIf (Me.paymentType = "ACH")
                    postData = postData + "&ACH_ROUTING=" + HttpUtility.UrlEncode(Me.routingNum) + _
                    "&ACH_ACCOUNT=" + HttpUtility.UrlEncode(Me.accountNum) + _
                    "&ACH_ACCOUNT_TYPE=" + HttpUtility.UrlEncode(Me.accountType) + _
                    "&DOC_TYPE=" + HttpUtility.UrlEncode(Me.docType)
                End If
            ElseIf (Me.api="bp20rebadmin")
                ' Calculate the Tamperproof Seal
                calcRebillTPS()
                Me.URL = "https://secure.bluepay.com/interfaces/bp20rebadmin"
                postData = postData + "&ACCOUNT_ID=" + HttpUtility.UrlEncode(Me.accountID) + _
                "&TRANS_TYPE=" + HttpUtility.UrlEncode(Me.transType) + _
                "&TAMPER_PROOF_SEAL=" + HttpUtility.UrlEncode(Me.TPS) + _
                "&REBILL_ID=" + HttpUtility.UrlEncode(Me.rebillID) + _
                "&TEMPLATE_ID=" + HttpUtility.UrlEncode(Me.templateID) + _
                "&REB_EXPR=" + HttpUtility.UrlEncode(Me.rebillExpr) + _
                "&REB_CYCLES=" + HttpUtility.UrlEncode(Me.rebillCycles) + _
                "&REB_AMOUNT=" + HttpUtility.UrlEncode(Me.rebillAmount) + _
                "&NEXT_AMOUNT=" + HttpUtility.UrlEncode(Me.rebillNextAmount) + _
                "&STATUS=" + HttpUtility.UrlEncode(Me.rebillStatus)
            ElseIf (Me.api="stq")
                calcReportTPS()
                Me.URL = "https://secure.bluepay.com/interfaces/stq"
                postData = postData + "&ACCOUNT_ID=" + HttpUtility.UrlEncode(Me.accountID) + _
                "&TAMPER_PROOF_SEAL=" + HttpUtility.UrlEncode(Me.TPS) + _
                "&REPORT_START_DATE=" + HttpUtility.UrlEncode(Me.reportStartDate) + _
                "&REPORT_END_DATE=" + HttpUtility.UrlEncode(Me.reportEndDate) + _
                "&id=" + HttpUtility.UrlEncode(Me.id) + _
                "&EXCLUDE_ERRORS=" + HttpUtility.UrlEncode(Me.excludeErrors) + _
                "&QUERY_BY_HIERARCHY=" + HttpUtility.UrlEncode(Me.queryByHierarchy) + _
                "&DO_NOT_ESCAPE=" + HttpUtility.UrlEncode(Me.doNotEscape)
            ElseIf (Me.api="bpdailyreport2")
                calcReportTPS()
                Me.URL = "https://secure.bluepay.com/interfaces/bpdailyreport2"
                postData = postData + "&ACCOUNT_ID=" + HttpUtility.UrlEncode(Me.accountID) + _
                "&TAMPER_PROOF_SEAL=" + HttpUtility.UrlEncode(Me.TPS) + _
                "&QUERY_BY_SETTLEMENT=" + HttpUtility.UrlEncode(Me.queryBySettlement) + _
                "&REPORT_START_DATE=" + HttpUtility.UrlEncode(Me.reportStartDate) + _
                "&REPORT_END_DATE=" + HttpUtility.UrlEncode(Me.reportEndDate) + _
                "&QUERY_BY_HIERARCHY=" + HttpUtility.UrlEncode(Me.queryByHierarchy) + _
                "&DO_NOT_ESCAPE=" + HttpUtility.UrlEncode(Me.doNotEscape) + _
                "&EXCLUDE_ERRORS=" + HttpUtility.UrlEncode(Me.excludeErrors)
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
                    responseParams(response)
                Catch e As WebException
                    Dim response As WebResponse = e.Response()
                    GetResponse(e)
                    response.Close()
                End Try
                dataStream.Close()
                                
                Return getStatus()
        End Function

        Public Sub getResponse(ByVal request As WebRequest)
            Dim httpResponse As WebResponse = request.GetResponse()
            responseParams(httpResponse)
        End Sub

        Public Sub getResponse(ByVal request As WebException)
            Dim httpResponse As WebResponse = request.Response()
            responseParams(httpResponse)
        End Sub


        Public Function responseParams(ByVal httpResponse As WebResponse) As String
            Dim dataStream As Stream = httpResponse.GetResponseStream()
            Dim reader As New StreamReader(dataStream)
            Dim responseFromServer As String = reader.ReadToEnd()
            Me.response = HttpUtility.UrlDecode(responseFromServer)
            reader.Close()
            Return responseFromServer
        End Function
        
        ''' <summary>
        ''' Returns True if transaction was successful 
        ''' </summary>
        ''' 
        Public Function isSuccessfulTransaction() As Boolean
            If (Not Me.getMessage = "DUPLICATE") And (Me.getStatus = "APPROVED") Then
                Return True
            Else
                Return False
            End If
        End Function 

        ''' <summary>
        ''' Returns STATUS from response
        ''' </summary>
        ''' 
        Public Function getStatus() As String
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
        Public Function getTransID() As String
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
        Public Function getMessage() As String
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
        Public Function getCVV2() As String
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
        Public Function getAVS() As String
            Dim r As Regex = New Regex("AVS=([^&$]+)")
            Dim m As Match = r.Match(Me.response)
            If m.Success Then
                Return m.Value.Substring(4)
            Else
                Return ""
            End If
        End Function

        Public Function getMaskedPaymentAccount() As String
            Dim r As Regex = New Regex("PAYMENT_ACCOUNT=([^&$]+)")
            Dim m As Match = r.Match(Me.response)
            If m.Success Then
                Return m.Value.Substring(16)
            Else
                Return ""
            End If
        End Function

        Public Function getCardType() As String
            Dim r As Regex = New Regex("CARD_TYPE=([^&$]+)")
            Dim m As Match = r.Match(Me.response)
            If m.Success Then
                Return m.Value.Substring(10)
            Else
                Return ""
            End If
        End Function

        Public Function getBank() As String
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
        Public Function getAuthCode() As String
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
        Public Function getRebillID() As String
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
        Public Function getCreationDate() As String
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
        Public Function getNextDate() As String
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
        Public Function getLastDate() As String
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
        Public Function getSchedExpr() As String
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
        Public Function getCyclesRemain() As String
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
        Public Function getRebillAmount() As String
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
        Public Function getNextAmount() As String
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


