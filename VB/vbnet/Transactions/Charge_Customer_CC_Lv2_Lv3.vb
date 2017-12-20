' *
' * Bluepay VB.NET Sample code.
' *
' * This code sample runs a $3.00 Credit Card Sale transaction
' * against a customer using test payment information.
' * If using TEST mode, odd dollar amounts will return
' * an approval and even dollar amounts will return a decline.
' *

Imports System
Imports System.Collections.Generic
Imports Level3.BPVB

Namespace Transactions

    Public Class ChargeCustomerCCLv2Lv3

        Public Shared Sub run()

            Dim accountID As String = "Merchant's Account ID Here"
            Dim secretKey As String = "Merchant's Secret Key Here"
            Dim mode As String = "TEST" 

            Dim payment As BluePay = New BluePay(
                accountID,
                secretKey,
                mode
            )

            payment.setCustomerInformation(
                firstName:="Bob",
                lastName:="Tester",
                address1:="123 Test St.",
                address2:="Apt #500",
                city:="Testville",
                state:="IL",
                zipCode:="54321", 
                country:="USA",
                phone:="123-123-12345",
                email:="test@bluepay.com"
            )

            payment.setCCInformation(
                ccNumber:="4111111111111111", 
                ccExpiration:="1225", 
                cvv2:="123" 
            )

            ' Set Level 2 Information
            payment.setInvoiceId(invoiceID:="123456789")
            payment.setAmountTax(amountTax:="0.91")

            ' Add line item for Level 3 Processing
            payment.addLineItem(
            quantity:="1", ' The number of units of item. Max: 5 digits.
            unitCost:="3.00", ' The cost per unit of item. Max: 9 digits decimal.
            descriptor:="test1", ' Description of the item purchased. Max: 26 character.
            commodityCode:="123412341234", ' Commodity Codes can be found at http://www.census.gov/svsd/www/cfsdat/2002data/cfs021200.pdf. Max: 12 characters.
            productCode:="432143214321", ' Merchant-defined code for the product or service being purchased. Max: 12 characters.
            measureUnits:="EA", ' The unit of measure of the item purchase. Normally EA. Max: 3 characters.
            taxRate:="7%", ' Tax rate for the item. Max: 4 digits.
            taxAmount:="0.21", ' Tax amount for the item. unit_cost * quantity * tax_rate = tax_amount. Max: 9 digits.
            itemDiscount:="0.00", ' The amount of any discounts on the item. Max: 12 digits.
            lineItemTotal:="3.21" ' The total amount for the item including taxes and discounts.
            )

            payment.addLineItem(
            quantity:="2",
            unitCost:="5.00",
            descriptor:="test2",
            commodityCode:="123412341234",
            productCode:="098709870987",
            measureUnits:="EA",
            taxRate:="7%",
            taxAmount:="0.70",
            itemDiscount:="0.00",
            lineItemTotal:="10.70"
            )

            payment.sale(amount:="13.91")

            payment.process()

            If payment.isSuccessfulTransaction() Then
                Console.Write("Transaction Status: " + payment.getStatus() + Environment.NewLine)
                Console.Write("Transaction Message: " + payment.getMessage() + Environment.NewLine)
                Console.Write("Transaction ID: " + payment.getTransID() + Environment.NewLine)
                Console.Write("AVS Result: " + payment.getAVS() + Environment.NewLine)
                Console.Write("CVV2 Result: " + payment.getCVV2() + Environment.NewLine)
                Console.Write("Masked Payment Account: " + payment.getMaskedPaymentAccount() + Environment.NewLine)
                Console.Write("Card Type: " + payment.getCardType() + Environment.NewLine)
                Console.Write("Authorization Code: " + payment.getAuthCode() + Environment.NewLine)
            Else
                Console.Write("Transaction Error: " + payment.getMessage() + Environment.NewLine)
            End If
        End Sub
    End Class
End Namespace