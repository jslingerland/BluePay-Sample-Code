' *
' * BluePay Visual Basic.NET Sample code.
' *
' * Updated: 2021-06-01
' *
' * This code is Free.  You may use it, modify it and redistribute it.
' * If you do make modifications that are useful, Bluepay would love it if you donated
' * them back to us!
' *
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
Imports System.Collections.Generic

Namespace BPVB

    ''' <summary>
    ''' This is the BluePayPayment object.
    ''' </summary>
    Public Class BluePay
        Public Const RELEASE_VERSION = "3.0.6"

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
        Private companyName As String = ""
        Private country As String = ""
        Private newCustToken As String = ""
        Private custToken As String = ""
        Private storedIndicator As String = ""
        Private storedType As String = ""
        Private storedId As String = ""
        
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
        Private level2Info As Dictionary(Of String, String) = New Dictionary(Of String, String)

        ' Level3 Field
        Private lineItems As ArrayList = New ArrayList()

        ' Generating Simple Hosted Payment Form URL fields
        Private dba As String = ""
        Private returnURL As String = ""
        Private discoverImage As String = ""
        Private amexImage As String = ""
        Private protectAmount As String = ""
        Private rebillProtect As String = ""
        Private protectCustomID1 As String = ""
        Private protectCustomID2 As String = ""
        Private shpfFormID As String = ""
        Private receiptFormID As String = ""
        Private remoteURL As String = ""
        Private shpfTpsHashType As String = ""
        Private receiptTpsHashType As String = ""
        Private cardTypes As String = ""
        Private receiptTpsDef As String = ""
        Private receiptTpsString As String = ""
        Private receiptTamperProofSeal As String = ""
        Private receiptURL As String = ""
        Private bp10emuTpsDef As String = ""
        Private bp10emuTpsString As String = ""
        Private bp10emuTamperProofSeal As String = ""
        Private shpfTpsDef As String = ""
        Private shpfTpsString As String = ""
        Private shpfTamperProofSeal As String = ""

        ' Report variables
        Private id As String = ""
        Private reportStartDate As String = ""
        Private reportEndDate As String = ""
        Private backendID As String = ""
        Private origin As String = ""
        Private doNotEscape As String = ""
        Private queryBySettlement As String = ""
        Private queryByHierarchy As String = ""
        Private excludeErrors As String = ""

        Private TPS As String = ""
        Private tpsHashType As String = "HMAC_SHA512"
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
        ''' <param name="storedIndicator"></param>
        ''' <param name="storedType"></param>
        ''' <param name="storedId"></param>
        ''' 
        Public Sub setCustomerInformation(Optional ByVal firstName As String = "", Optional ByVal lastName As String = "", Optional ByVal address1 As String = "", Optional ByVal address2 As String = "", Optional ByVal city As String = "", Optional ByVal state As String = "", Optional ByVal zipCode As String = "", Optional ByVal country As String = "", Optional ByVal phone As String = "", Optional ByVal email As String = "", Optional ByVal companyName As String = "", Optional ByVal storedIndicator As String = "", Optional ByVal storedType As String = "", Optional ByVal storedId As String = "")
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
            Me.companyName = companyName
	    Me.storedIndicator = storedIndicator
	    Me.storedType = storedType
	    Me.storedId = storedId
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
        ''' Adds information required for level 2 processing.
        ''' </summary>
        Public sub addLevel2Information(Optional ByVal taxRate As String = "", Optional ByVal goodsTaxRate As String = "", Optional ByVal goodsTaxAmount As String = "", Optional ByVal shippingAmount As String = "", Optional ByVal discountAmount As String = "", Optional ByVal custPO As String = "", Optional ByVal goodsTaxID As String = "", Optional ByVal taxID As String = "", Optional ByVal customerTaxID As String = "", Optional ByVal dutyAmount As String = "", Optional ByVal supplementalData As String = "", Optional ByVal cityTaxRate As String = "", Optional ByVal cityTaxAmount As String = "", Optional ByVal countyTaxRate As String = "", Optional ByVal countyTaxAmount As String = "", Optional ByVal stateTaxRate As String = "", Optional ByVal stateTaxAmount As String = "", Optional ByVal buyerName As String = "", Optional ByVal customerReference As String = "", Optional ByVal customerNumber As String = "", Optional ByVal shipName As String = "", Optional ByVal shipAddr1 As String = "", Optional ByVal shipAddr2 As String = "", Optional ByVal shipCity As String = "", Optional ByVal shipState As String = "", Optional ByVal shipZip As String = "", Optional ByVal shipCountry As String = "")
            me.level2Info.Add( "LV2_ITEM_TAX_RATE", taxRate )
            me.level2Info.Add( "LV2_ITEM_GOODS_TAX_RATE", goodsTaxRate )
            me.level2Info.Add( "LV2_ITEM_GOODS_TAX_AMOUNT", goodsTaxAmount )
            me.level2Info.Add( "LV2_ITEM_SHIPPING_AMOUNT", shippingAmount )
            me.level2Info.Add( "LV2_ITEM_DISCOUNT_AMOUNT", discountAmount )
            me.level2Info.Add( "LV2_ITEM_CUST_PO", custPO )
            me.level2Info.Add( "LV2_ITEM_GOODS_TAX_ID", goodsTaxID )
            me.level2Info.Add( "LV2_ITEM_TAX_ID", taxID )
            me.level2Info.Add( "LV2_ITEM_CUSTOMER_TAX_ID", customerTaxID )
            me.level2Info.Add( "LV2_ITEM_DUTY_AMOUNT", dutyAmount )
            me.level2Info.Add( "LV2_ITEM_SUPPLEMENTAL_DATA", supplementalData )
            me.level2Info.Add( "LV2_ITEM_CITY_TAX_RATE", cityTaxRate )
            me.level2Info.Add( "LV2_ITEM_CITY_TAX_AMOUNT", cityTaxAmount )
            me.level2Info.Add( "LV2_ITEM_COUNTY_TAX_RATE", countyTaxRate )
            me.level2Info.Add( "LV2_ITEM_COUNTY_TAX_AMOUNT", countyTaxAmount )
            me.level2Info.Add( "LV2_ITEM_STATE_TAX_RATE", stateTaxRate )
            me.level2Info.Add( "LV2_ITEM_STATE_TAX_AMOUNT", stateTaxAmount )
            me.level2Info.Add( "LV2_ITEM_BUYER_NAME", buyerName )
            me.level2Info.Add( "LV2_ITEM_CUSTOMER_REFERENCE", customerReference )
            me.level2Info.Add( "LV2_ITEM_CUSTOMER_NUMBER", customerNumber )
            me.level2Info.Add( "LV2_ITEM_SHIP_NAME", shipName )
            me.level2Info.Add( "LV2_ITEM_SHIP_ADDR1", shipAddr1 )
            me.level2Info.Add( "LV2_ITEM_SHIP_ADDR2", shipAddr2 )
            me.level2Info.Add( "LV2_ITEM_SHIP_CITY", shipCity )
            me.level2Info.Add( "LV2_ITEM_SHIP_STATE", shipState )
            me.level2Info.Add( "LV2_ITEM_SHIP_ZIP", shipZip )
            me.level2Info.Add( "LV2_ITEM_SHIP_COUNTRY", shipCountry )
        End Sub

        ''' <summary>
        ''' Adds a line item for level 3 processing. Repeat method for each item up to 99 items.
        ''' For Canadian and AMEX processors, ensure required Level 2 information is present.
        ''' </summary>
        Public sub addLineItem(ByVal unitCost As String, ByVal quantity As String, Optional ByVal itemSKU As String = "", Optional ByVal descriptor As String = "", Optional ByVal commodityCode As String = "", Optional ByVal productCode As String = "", Optional ByVal measureUnits As String = "", Optional ByVal itemDiscount As String = "", Optional ByVal taxRate As String = "", Optional ByVal goodsTaxRate As String = "", Optional ByVal taxAmount As String = "", Optional ByVal goodsTaxAmount As String = "", Optional ByVal cityTaxRate As String = "", Optional ByVal cityTaxAmount As String = "", Optional ByVal countyTaxRate As String = "", Optional ByVal countyTaxAmount As String = "", Optional ByVal stateTaxRate As String = "", Optional ByVal stateTaxAmount As String = "", Optional ByVal custSKU As String = "", Optional ByVal custPO As String = "", Optional ByVal supplementalData As String = "", Optional ByVal glAccountNumber As String = "", Optional ByVal divisionNumber As String = "", Optional ByVal poLineNumber As String = "", Optional ByVal lineItemTotal As String = "")
            dim i As String = (me.lineItems.Count + 1 ).ToString
            dim prefix As String = "LV3_ITEM" & i & "_"

            dim item As Dictionary(Of String, String) = new Dictionary(Of String, String)

            item.Add( prefix + "UNIT_COST", unitCost )
            item.Add( prefix + "QUANTITY", quantity )
            item.Add( prefix + "ITEM_SKU", itemSKU )
            item.Add( prefix + "ITEM_DESCRIPTOR", descriptor )
            item.Add( prefix + "COMMODITY_CODE", commodityCode )
            item.Add( prefix + "PRODUCT_CODE", productCode )
            item.Add( prefix + "MEASURE_UNITS", measureUnits )
            item.Add( prefix + "ITEM_DISCOUNT", itemDiscount )
            item.Add( prefix + "TAX_RATE", taxRate )
            item.Add( prefix + "GOODS_TAX_RATE", goodsTaxRate )
            item.Add( prefix + "TAX_AMOUNT", taxAmount )
            item.Add( prefix + "GOODS_TAX_AMOUNT", goodsTaxAmount )
            item.Add( prefix + "CITY_TAX_RATE", cityTaxRate )
            item.Add( prefix + "CITY_TAX_AMOUNT", cityTaxAmount )
            item.Add( prefix + "COUNTY_TAX_RATE", countyTaxRate )
            item.Add( prefix + "COUNTY_TAX_AMOUNT", countyTaxAmount )
            item.Add( prefix + "STATE_TAX_RATE", stateTaxRate )
            item.Add( prefix + "STATE_TAX_AMOUNT", stateTaxAmount )
            item.Add( prefix + "CUST_SKU", custSKU )
            item.Add( prefix + "CUST_PO", custPO )
            item.Add( prefix + "SUPPLEMENTAL_DATA", supplementalData )
            item.Add( prefix + "GL_ACCOUNT_NUMBER", glAccountNumber )
            item.Add( prefix + "DIVISION_NUMBER", divisionNumber )
            item.Add( prefix + "PO_LINE_NUMBER", poLineNumber )
            item.Add( prefix + "LINE_ITEM_TOTAL", lineItemTotal )

            me.lineItems.Add(item)

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
        ''' Runs a Sale Transaction
        ''' </summary>
        ''' <param name="amount"></param>
        ''' <param name="customerToken"></param>
        ''' 
        Public Sub sale(ByVal amount As String, ByVal customerToken As String)
            Me.transType = "SALE"
            Me.amount = amount
            Me.custToken = customerToken
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
        ''' Runs an Auth Transaction
        ''' </summary>
        ''' <param name="amount"></param>
        ''' <param name="newCustomerToken"></param>
        ''' 
        Public Sub auth(ByVal amount As String, ByVal newCustomerToken As String)
            Me.transType = "AUTH"
            Me.amount = amount
            If newCustomerToken.ToLower() = "true" Then
                Me.newCustToken = GetRandomString(16)
            Elseif newCustomerToken.ToLower() <> "false" Then
                Me.newCustToken = newCustomerToken
            End If 
            Me.api = "bp10emu"
        End Sub

        ''' <summary>
        ''' Creates a random alphanumeric string
        ''' </summary>
        ''' <param name="length"></param>
        ''' <param name="newCustomerToken"></param>
        ''' 
        Public Function GetRandomString(ByVal iLength As Integer) As String
            Dim sResult As String = ""
            Dim rdm As New Random()

            Dim characters() As String = {"a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z","1","2","3","4","5","6","7","8","9","0","A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z","1","2","3","4","5","6","7","8","9","0"}

            For i As Integer = 1 To iLength
                sResult &= characters(rdm.Next(0, characters.length - 1 ))
            Next

            Return sResult
        End Function

        ''' <summary>
        ''' Runs an Auth Transaction
        ''' </summary>
        ''' <param name="amount"></param>
        ''' <param name="customerToken"></param>
        ''' 
        Public Sub auth(ByVal amount As String, ByVal customerToken As String)
            Me.transType = "AUTH"
            Me.amount = amount
            Me.custToken = customerToken
            Me.api = "bp10emu"
        End Sub
       
        ''' <summary>
        ''' Updates a Transaction
        ''' </summary>
        ''' <param name="masterID"></param>
        ''' 
        Public Sub update(ByVal masterID As String)
            Me.transType = "UPDATE"
            Me.masterID = masterID
            Me.api = "bp10emu"
        End Sub

        ''' <summary>
        ''' Updates a Transaction
        ''' </summary>
        ''' <param name="masterID"></param>
        ''' <param name="amount"></param>
        ''' <remarks></remarks>
        Public Sub update(ByVal transactionID As String, ByVal amount As String)
            Me.transType = "UPDATE"
            Me.masterID = transactionID
            Me.amount = amount
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

        ''' <summary>
        ''' Sets Company_Name Field
        ''' </summary>
        ''' <param name="companyName"></param>
        ''' 
        Public Sub setCompanyName(ByVal companyName As String)
            Me.companyName = companyName
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
        ''' <param name="backendID"></param>
        ''' <param name="origin"></param>
        ''' 
        Public Sub getTransactionReport(
            ByVal reportStartDate As String,
            ByVal reportEndDate As String, 
            Optional ByVal queryByHierarchy As String="",
            Optional ByVal backendID As String="",
            Optional ByVal origin As String="",			
            Optional ByVal doNotEscape As String="", 
            Optional ByVal excludeErrors As String = "" 
            )
            Me.queryBySettlement = "0"
            Me.api = "bpdailyreport2"
            Me.reportStartDate = reportStartDate
            Me.reportEndDate = reportEndDate
            Me.queryByHierarchy = queryByHierarchy
            Me.backendID = backendID
            Me.origin = origin
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
        ''' Generates the TAMPER_PROOF_SEAL to used to validate each transaction
        ''' </summary>
        '''
        Public Function generateTPS(ByVal message As String, ByVal hashType As String) As String
            Dim result As String
            Dim encode As ASCIIEncoding = New ASCIIEncoding
            If hashType = "HMAC_SHA256" Then
                Dim secretKeyBytes() As Byte = encode.GetBytes(Me.secretKey)
                Dim messageBytes() As Byte = encode.GetBytes(message)
                Dim hmac As HMACSHA256 = New HMACSHA256(secretKeyBytes)
                Dim hashBytes() As Byte = hmac.ComputeHash(messageBytes)
                result = ByteArrayToString(hashBytes)
            Elseif hashType = "SHA512" Then
                Dim sha512 As SHA512 = New SHA512Managed()
                Dim hash() As Byte
                Dim buffer() As Byte = encode.GetBytes(Me.secretKey + message)
                hash = sha512.ComputeHash(buffer)
                result = ByteArrayToString(hash)
            Elseif hashType = "SHA256" Then
                Dim sha256 As SHA256 = New SHA256Managed()
                Dim hash() As Byte
                Dim buffer() As Byte = encode.GetBytes(Me.secretKey + message)
                hash = sha256.ComputeHash(buffer)
                result = ByteArrayToString(hash)
            Elseif hashType = "MD5" Then
                Dim md5 As MD5 = New MD5CryptoServiceProvider
                Dim hash() As Byte
                Dim buffer() As Byte = encode.GetBytes(Me.secretKey + message)
                hash = md5.ComputeHash(buffer)
                result = ByteArrayToString(hash)
            Else
                Dim secretKeyBytes() As Byte = encode.GetBytes(Me.secretKey)
                Dim messageBytes() As Byte = encode.GetBytes(message)
                Dim hmac As HMACSHA512 = New HMACSHA512(secretKeyBytes)
                Dim hashBytes() As Byte = hmac.ComputeHash(messageBytes)
                result = ByteArrayToString(hashBytes)
            End If
            Return result 
        End Function

        ''' <summary>
        ''' Calculates TAMPER_PROOF_SEAL for bp10emu API
        ''' </summary>
        '''
        Public Sub calcTPS()
            Dim tps As String = Me.accountID _
                        + Me.transType _
                        + Me.amount _
                        + Me.doRebill _
                        + Me.rebillFirstDate _
                        + Me.rebillExpr _
                        + Me.rebillCycles _
                        + Me.rebillAmount _
                        + Me.masterID _
                        + Me.mode
            Me.TPS = generateTPS(tps, Me.tpsHashType)
        End Sub

        ''' <summary>
        ''' Calculates TAMPER_PROOF_SEAL for stq and bpdailyreport2 API
        ''' </summary>
        '''
        Public Sub calcReportTPS()
            Dim tps As String = Me.accountID _
                        + Me.reportStartDate _
                        + Me.reportEndDate
            Me.TPS = generateTPS(tps, Me.tpsHashType)
        End Sub


        ''' <summary>
        ''' Calculates TAMPER_PROOF_SEAL for bp20rebadmin API
        ''' </summary>
        '''
        Public Sub calcRebillTPS()
            Dim tps As String = Me.accountID _
                        + Me.transType _
                        + Me.rebillID
            Me.TPS = generateTPS(tps, Me.tpsHashType)
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

        ''' <summary>
        ''' Calls the methods necessary to generate a SHPF URL
        ''' Required arguments for generate_url:
        ''' merchantName: Merchant name that will be displayed in the payment page.
        ''' returnURL: Link to be displayed on the transacton results page. Usually the merchant's web site home page.
        ''' transactionType: SALE/AUTH -- Whether the customer should be charged or only check for enough credit available.
        ''' acceptDiscover: Yes/No -- Yes for most US merchants. No for most Canadian merchants.
        ''' acceptAmex: Yes/No -- Has an American Express merchant account been set up?
        ''' amount: The amount if the merchant is setting the initial amount.
        ''' protectAmount: Yes/No -- Should the amount be protected from changes by the tamperproof seal?
        ''' rebilling: Yes/No -- Should a recurring transaction be set up?
        ''' paymentTemplate: Select one of our payment form template IDs or your own customized template ID. If the customer should not be allowed to change the amount, add a 'D' to the end of the template ID. Example: 'mobileform01D'
            ''' mobileform01 -- Credit Card Only - White Vertical (mobile capable) 
            ''' default1v5 -- Credit Card Only - Gray Horizontal 
            ''' default7v5 -- Credit Card Only - Gray Horizontal Donation
            ''' default7v5R -- Credit Card Only - Gray Horizontal Donation with Recurring
            ''' default3v4 -- Credit Card Only - Blue Vertical with card swipe
            ''' mobileform02 -- Credit Card & ACH - White Vertical (mobile capable)
            ''' default8v5 -- Credit Card & ACH - Gray Horizontal Donation
            ''' default8v5R -- Credit Card & ACH - Gray Horizontal Donation with Recurring
            ''' mobileform03 -- ACH Only - White Vertical (mobile capable)
        ''' receiptTemplate: Select one of our receipt form template IDs, your own customized template ID, or "remote_url" if you have one.
            ''' mobileresult01 -- Default without signature line - White Responsive (mobile)
            ''' defaultres1 -- Default without signature line – Blue
            ''' V5results -- Default without signature line – Gray
            ''' V5Iresults -- Default without signature line – White
            ''' defaultres2 -- Default with signature line – Blue
            ''' remote_url - Use a remote URL
        ''' receiptTempRemoteURL: Your remote URL ** Only required if receipt_template = "remote_url".

        ''' Optional arguments for generate_url:
        ''' rebProtect: Yes/No -- Should the rebilling fields be protected by the tamperproof seal?
        ''' rebAmount: Amount that will be charged when a recurring transaction occurs.
        ''' rebCycles: Number of times that the recurring transaction should occur. Not set if recurring transactions should continue until canceled.
        ''' rebStartDate: Date (yyyy-mm-dd) or period (x units) until the first recurring transaction should occur. Possible units are DAY, DAYS, WEEK, WEEKS, MONTH, MONTHS, YEAR or YEARS. (ex. 2016-04-01 or 1 MONTH)
        ''' rebFrequency: How often the recurring transaction should occur. Format is 'X UNITS'. Possible units are DAY, DAYS, WEEK, WEEKS, MONTH, MONTHS, YEAR or YEARS. (ex. 1 MONTH) 
        ''' customID1: A merchant defined custom ID value.
        ''' protectCustomID1: Yes/No -- Should the Custom ID value be protected from change using the tamperproof seal?
        ''' customID2: A merchant defined custom ID 2 value.
        ''' protectCustomID2: Yes/No -- Should the Custom ID 2 value be protected from change using the tamperproof seal?
        ''' </summary>
        ''' <param name="merchantName"></param>
        ''' <param name="returnURL"></param>
        ''' <param name="transactionType"></param>
        ''' <param name="acceptDiscover"></param>
        ''' <param name="acceptAmex"></param>
        ''' <param name="amount"></param>
        ''' <param name="protectAmount"></param>
        ''' <param name="rebilling"></param>
        ''' <param name="rebProtect"></param>
        ''' <param name="rebAmount"></param>
        ''' <param name="rebCycles"></param>
        ''' <param name="rebStartDate"></param>
        ''' <param name="rebFrequency"></param>
        ''' <param name="customID1"></param>
        ''' <param name="protectCustomID1"></param>
        ''' <param name="customID2"></param>
        ''' <param name="protectCustomID2"></param>
        ''' <param name="paymentTemplate"></param>
        ''' <param name="receiptTemplate"></param>
        ''' <param name="receiptTempRemoteURL"></param>
        Public Function generateURL(Optional ByVal merchantName As String = "", Optional ByVal returnURL As String = "", Optional ByVal transactionType As String = "",  Optional ByVal acceptDiscover As String = "", Optional ByVal acceptAmex As String = "", Optional ByVal amount As String = "", Optional ByVal protectAmount As String = "No", Optional ByVal rebilling As String = "No", Optional ByVal rebProtect As String = "Yes", Optional ByVal rebAmount As String = "", Optional ByVal rebCycles As String = "", Optional ByVal rebStartDate As String = "", Optional ByVal rebFrequency As String = "", Optional ByVal customID1 As String = "", Optional ByVal protectCustomID1 = "No", Optional ByVal customID2 As String = "", Optional ByVal protectCustomID2 As String = "No", Optional ByVal paymentTemplate As String = "mobileform01", Optional ByVal receiptTemplate As String = "mobileresult01", Optional ByVal receiptTempRemoteURL As String = "", Optional ByVal tpsHashType As String = "") As String
            Me.dba = merchantName
            Me.returnURL = returnURL
            Me.transType = transactionType
            Me.discoverImage = If(Regex.IsMatch(acceptDiscover, "^[yY]"), "discvr.gif", "spacer.gif")
            Me.amexImage = If(Regex.IsMatch(acceptAmex, "^[yY]"), "amex.gif", "spacer.gif")
            Me.amount = amount
            Me.protectAmount = protectAmount
            Me.doRebill = If(Regex.IsMatch(rebilling, "^[yY]"), "1", "0")
            Me.rebillProtect = rebProtect
            Me.rebillAmount = rebAmount
            Me.rebillCycles = rebCycles
            Me.rebillFirstDate = rebStartDate
            Me.rebillExpr = rebFrequency
            Me.customID1 = customID1
            Me.protectCustomID1 = protectCustomID1
            Me.customID2 = customID2
            Me.protectCustomID2 = protectCustomID2
            Me.shpfFormID = paymentTemplate
            Me.receiptFormID = receiptTemplate
            Me.remoteURL = receiptTempRemoteURL
            Me.shpfTpsHashType = "HMAC_SHA512"
            Me.receiptTpsHashType = Me.shpfTpsHashType
            Me.tpsHashType = setHashType(tpsHashType)
            Me.cardTypes = setCardTypes()
            Me.receiptTpsDef = "SHPF_ACCOUNT_ID SHPF_FORM_ID RETURN_URL DBA AMEX_IMAGE DISCOVER_IMAGE SHPF_TPS_DEF SHPF_TPS_HASH_TYPE"
            Me.receiptTpsString = setReceiptTpsString()
            Me.receiptTamperProofSeal = generateTPS(Me.receiptTpsString, Me.receiptTpsHashType)
            Me.receiptURL = setReceiptURL()
            Me.bp10emuTpsDef = addDefProtectedStatus("MERCHANT APPROVED_URL DECLINED_URL MISSING_URL MODE TRANSACTION_TYPE TPS_DEF TPS_HASH_TYPE")
            Me.bp10emuTpsString = setBp10emuTpsString()
            Me.bp10emuTamperProofSeal = generateTPS(Me.bp10emuTpsString, Me.tpsHashType)
            Me.shpfTpsDef = addDefProtectedStatus("SHPF_FORM_ID SHPF_ACCOUNT_ID DBA TAMPER_PROOF_SEAL AMEX_IMAGE DISCOVER_IMAGE TPS_DEF TPS_HASH_TYPE SHPF_TPS_DEF SHPF_TPS_HASH_TYPE")
            Me.shpfTpsString = setShpfTpsString()
            Me.shpfTamperProofSeal = generateTPS(Me.shpfTpsString, Me.shpfTpsHashType)
            Return calcURLResponse()
        End Function

        ''' <summary>
        ''' Validates chosen hash type or returns default hash type
        ''' </summary>
        Public Function setHashType(ByVal chosenHash As String) As String
            Dim defaultHash As String = "HMAC_SHA512"
            chosenHash = chosenHash.ToUpper()
            Dim result As String = ""
            Dim hashes As ArrayList = New ArrayList(4)
            hashes.Add("MD5")
            hashes.Add("SHA256")
            hashes.Add("SHA512")
            hashes.Add("HMAC_SHA256")
            If hashes.Contains(chosenHash) Then
                result = chosenHash
            Else
                result = defaultHash
            End If 
            Return result
        End Function
        
        ''' <summary>
        ''' Sets the types of credit card images to use on the Simple Hosted Payment Form, used in public string GenerateURL()
        ''' </summary>
        Public Function setCardTypes() As String
            Dim creditCards As String = "vi-mc"
            creditCards = If(Me.discoverImage Is "discvr.gif", (creditCards + "-di"), creditCards)
            creditCards = If(Me.amexImage Is "amex.gif", (creditCards + "-am"), creditCards)
            Return creditCards
        End Function

        ''' <summary>
        ''' Sets the receipt Tamperproof Seal string, used in public string GenerateURL()
        ''' </summary>
        Public Function setReceiptTpsString() As String
            Return Me.accountID + Me.receiptFormID + Me.returnURL + Me.dba + Me.amexImage + Me.discoverImage + Me.receiptTpsDef + Me.receiptTpsHashType
        End Function

        ''' <summary>
        ''' Sets the bp10emu string that will be used to create a Tamperproof Seal, used in public string GenerateURL()
        ''' </summary>
        Public Function setBp10emuTpsString() As String
            Dim bp10emu As String = Me.accountID + Me.receiptURL + Me.receiptURL + Me.receiptURL + Me.mode + Me.transType + Me.bp10emuTpsDef + Me.tpsHashType
            Return addStringProtectedStatus(bp10emu)
        End Function

        ''' <summary>
        ''' Sets the Simple Hosted Payment Form string that will be used to create a Tamperproof Seal, used in public string GenerateURL()
        ''' </summary>
        Public Function setShpfTpsString() As String
            Dim shpf As String = Me.shpfFormID + Me.accountID + Me.dba + Me.bp10emuTamperProofSeal + Me.amexImage + Me.discoverImage + Me.bp10emuTpsDef + Me.tpsHashType + Me.shpfTpsDef + Me.shpfTpsHashType
            Return addStringProtectedStatus(shpf)
        End Function

        ''' <summary>
        ''' Sets the receipt url or uses the remote url provided, used in public string GenerateURL()
        ''' </summary>
        Public Function setReceiptURL() As String
            Dim output As String = ""
            If Me.receiptFormID Is "remote_url"
                output = Me.remoteURL
            Else 
                output =  "https://secure.bluepay.com/interfaces/shpf?SHPF_FORM_ID=" + Me.receiptFormID + _
                "&SHPF_ACCOUNT_ID="     + Me.accountID +  _
                "&SHPF_TPS_DEF="        + encodeURL(Me.receiptTpsDef) +  _
                "&SHPF_TPS_HASH_TYPE="  + encodeURL(Me.receiptTpsHashType) +  _
                "&SHPF_TPS="            + encodeURL(Me.receiptTamperProofSeal) +  _
                "&RETURN_URL="          + encodeURL(Me.returnURL) + _
                "&DBA="                 + encodeURL(Me.dba) +  _
                "&AMEX_IMAGE="          + encodeURL(Me.amexImage) +  _
                "&DISCOVER_IMAGE="      + encodeURL(Me.discoverImage)
            End If
            Return output
        End Function

        ''' <summary>
        ''' Adds optional protected keys to a string, used in public string GenerateURL()
        ''' </summary>
        Public Function addDefProtectedStatus(Optional ByVal input As String = "") As String
            If (Me.protectAmount Is "Yes") Then input += " AMOUNT"
            If (Me.rebillProtect Is "Yes") Then input += " REBILLING REB_CYCLES REB_AMOUNT REB_EXPR REB_FIRST_DATE"
            If (Me.protectCustomID1 Is "Yes") Then input += " CUSTOM_ID"
            If (Me.protectCustomID2 Is "Yes") Then input += " CUSTOM_ID2"
            Return input
        End Function

        ''' <summary>
        ''' Adds optional protected values to a string, used in public string GenerateURL()
        ''' </summary>
        Public Function addStringProtectedStatus(Optional ByVal input As String = "") As String
            If (protectAmount Is "Yes") Then input += Me.amount
            If (rebillProtect Is "Yes") Then input += Me.doRebill + Me.rebillCycles + Me.rebillAmount + Me.rebillExpr + Me.rebillFirstDate
            If (protectCustomID1 Is "Yes") Then input += Me.customID1
            If (protectCustomID2 Is "Yes") Then input += Me.customID2
            Return input
        End Function

        ''' <summary>
        ''' Encodes a string into a URL, used in public string GenerateURL()
        ''' </summary>
        Public Function encodeURL(Optional ByVal input As String = "") As String
            Dim encodedString As New StringBuilder()
            For Each character As Char In input
                If Char.IsLetterOrDigit(character)
                    encodedString.Append(character.ToString())
                Else 
                    Dim value As Integer = Convert.ToInt32(character)
                    Dim hexOutput As String = String.Format("{0:X}", value)
                    encodedString.Append("%").Append(hexOutput)
                End If
            Next
            Return encodedString.ToString()
        End Function

        ''' <summary>
        ''' Generates a Tamperproof Seal for a url, used in public string GenerateURL()
        ''' </summary>
        Public Function calcURLTps(Optional ByVal input As String = "") As String
            Dim md5 As MD5 = New MD5CryptoServiceProvider
            Dim hash() As Byte
            Dim encode As ASCIIEncoding = New ASCIIEncoding
            Dim buffer() As Byte = encode.GetBytes(input)
            hash = md5.ComputeHash(buffer)
            Return ByteArrayToString(hash)
        End Function

        ''' <summary>
        ''' Generates the final url for the Simple Hosted Payment Form, used in public string GenerateURL()
        ''' </summary>
        Public Function calcURLResponse() As String
            Return  _
            "https://secure.bluepay.com/interfaces/shpf?"                    +  _
            "SHPF_FORM_ID="         + encodeURL(Me.shpfFormID)               +  _
            "&SHPF_ACCOUNT_ID="     + encodeURL(Me.accountID)                +  _
            "&SHPF_TPS_DEF="        + encodeURL(Me.shpfTpsDef)               +  _
            "&SHPF_TPS_HASH_TYPE="  + encodeURL(Me.shpfTpsHashType)          +  _
            "&SHPF_TPS="            + encodeURL(Me.shpfTamperProofSeal)      +  _
            "&MODE="                + encodeURL(Me.mode)                     +  _
            "&TRANSACTION_TYPE="    + encodeURL(Me.transType)                +  _
            "&DBA="                 + encodeURL(Me.dba)                      +  _
            "&AMOUNT="              + encodeURL(Me.amount)                   +  _
            "&TAMPER_PROOF_SEAL="   + encodeURL(Me.bp10emuTamperProofSeal)   +  _
            "&CUSTOM_ID="           + encodeURL(Me.customID1)                +  _
            "&CUSTOM_ID2="          + encodeURL(Me.customID2)                +  _
            "&REBILLING="           + encodeURL(Me.doRebill)                 +  _
            "&REB_CYCLES="          + encodeURL(Me.rebillCycles)             +  _
            "&REB_AMOUNT="          + encodeURL(Me.rebillAmount)             +  _
            "&REB_EXPR="            + encodeURL(Me.rebillExpr)               +  _
            "&REB_FIRST_DATE="      + encodeURL(Me.rebillFirstDate)          +  _
            "&AMEX_IMAGE="          + encodeURL(Me.amexImage)                +  _
            "&DISCOVER_IMAGE="      + encodeURL(Me.discoverImage)            +  _
            "&REDIRECT_URL="        + encodeURL(Me.receiptURL)               +  _
            "&TPS_DEF="             + encodeURL(Me.bp10emuTpsDef)            +  _
            "&TPS_HASH_TYPE="       + encodeURL(Me.tpsHashType)              +  _
            "&CARD_TYPES="          + encodeURL(Me.cardTypes)
        End Function

        Public Function process() As String
            Dim postData As String = "MODE=" + HttpUtility.UrlEncode(Me.mode)
            postData = postData + "&RESPONSEVERSION=8"
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
                "&COMPANY_NAME=" + HttpUtility.UrlEncode(Me.companyName) + _
                "&STORED_INDICATOR=" + HttpUtility.UrlEncode(Me.storedIndicator) + _
                "&STORED_TYPE=" + HttpUtility.UrlEncode(Me.storedType) + _
                "&STORED_ID=" + HttpUtility.UrlEncode(Me.storedId) + _
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
                "&CUSTOM_ID2=" + HttpUtility.UrlEncode(Me.customID2) + _
                "&TPS_HASH_TYPE=" + HttpUtility.UrlEncode(Me.tpsHashType)
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
                    "&CVCCVV2=" + HttpUtility.UrlEncode(Me.cvv2)
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
                "&STATUS=" + HttpUtility.UrlEncode(Me.rebillStatus) + _
                "&TPS_HASH_TYPE=" + HttpUtility.UrlEncode(Me.tpsHashType)
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
                "&DO_NOT_ESCAPE=" + HttpUtility.UrlEncode(Me.doNotEscape) + _
                "&TPS_HASH_TYPE=" + HttpUtility.UrlEncode(Me.tpsHashType)				
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
                "&EXCLUDE_ERRORS=" + HttpUtility.UrlEncode(Me.excludeErrors) + _
                "&TPS_HASH_TYPE=" + HttpUtility.UrlEncode(Me.tpsHashType)				
                If Not (Me.backendID) Is Nothing Then
                postData = postData + _ "&BACKEND_ID=" + HttpUtility.UrlEncode(Me.backendID)
                If Not (Me.origin) Is Nothing Then
                postData = postData + _ "&ORIGIN=" + HttpUtility.UrlEncode(Me.origin)
            End If

            ' Add Level 2 data, if available
            For Each field As KeyValuePair(Of String, String) in me.level2Info
                postData = postData & "&" & field.Key & "=" & field.Value
            Next

            ' Add Level 3 item data, if available
            For Each item As Dictionary (Of String, String) in me.lineItems
                For Each field As KeyValuePair(Of String, String) in item
                    postData = postData & "&" & field.Key & "=" & field.Value
                Next
            Next

            'Add customer token data, if available
            If (Me.custToken <> "") Then postData = postData & "&CUST_TOKEN=" & Me.custToken

            If (Me.newCustToken <> "") Then postData = postData & "&NEW_CUST_TOKEN=" & Me.newCustToken

            ' Create HTTPS POST object and send to BluePay
                Dim httpRequest As HttpWebRequest = HttpWebRequest.Create(Me.URL)
                httpRequest.Method = "POST"
                httpRequest.AllowAutoRedirect = False
                Dim byteArray As Byte() = Text.Encoding.UTF8.GetBytes(postData)
                httpRequest.UserAgent= "BluePay Visual Basic Library/" + RELEASE_VERSION    
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
            Dim responseFromServer As String = ""
            If (Me.api="bp10emu") Then
                responseFromServer = httpResponse.Headers.Item("Location")
                Me.response = responseFromServer
            Else
                Dim dataStream As Stream = httpResponse.GetResponseStream()
                Dim reader As New StreamReader(dataStream)
                responseFromServer = reader.ReadToEnd()
                Me.response = HttpUtility.UrlDecode(responseFromServer)
                reader.Close()
            End If
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
        ''' Returns STORED_ID from response
        ''' </summary>
        '''
        Public Function getStoredId() As String
            Dim r As Regex = New Regex("STORED_ID=([^&$]+)")
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

        ''' <summary>
        ''' Returns cust_token from response
        ''' </summary>
        '''
        Public Function getCustomerToken() As String
            Dim r As Regex = New Regex("CUST_TOKEN=([^&$]+)")
            Dim m As Match = r.Match(Me.response)
            If m.Success Then
                Return m.Value.Substring(11)
            Else
                Return ""
            End If
        End Function

    End Class
End Namespace
