<?php
/**
* BluePay PHP Sample Code
*
* This code sample runs a Credit Card sales transaction, 
* including sample Level 2 and 3 processing information,
* against a customer using test payment information.
* If using TEST mode, odd dollar amounts will return
* an approval and even dollar amounts will return a decline.
*
*/

include('../BluePay.php');

$accountID = "Merchant's Account ID Here";
$secretKey = "Merchant's Secret Key Here";
$mode = "TEST";
 
$payment = new BluePay(
    $accountID,
    $secretKey,
    $mode
);

$payment->setCustomerInformation(array(
    'firstName' => 'Bob', 
    'lastName' => 'Tester', 
    'addr1' => '1234 Test St.', 
    'addr2' => 'Apt #500', 
    'city' => 'Testville', 
    'state' => 'IL', 
    'zip' =>'54321', 
    'country' => 'USA', 
    'phone' => '1231231234', 
    'email' => 'test@bluepay.com' 
));
 
$payment->setCCInformation(array(
    'cardNumber' => '4111111111111111', // Card Number: 4111111111111111
    'cardExpire' => '1225', // Card Expire: 12/25
    'cvv2' => '123' // Card CVV2: 123
));

// Set Level 2 Information
$payment->setInvoiceID("123456789");
$payment->setAmountTax("0.91");

// Set Level 3 line item information. Repeat for each item up to 99.
$payment->addLineItem(array(
  'quantity' => '1', // The number of units of item. Max: 5 digits
  'unitCost' => '3.00', // The cost per unit of item. Max: 9 digits decimal
  'descriptor' => 'test1', // Description of the item purchased. Max: 26 character
  'commodityCode' => '123412341234', // Commodity Codes can be found at http://www.census.gov/svsd/www/cfsdat/2002data/cfs021200.pdf. Max: 12 characters
  'productCode' => '432143214321', // Merchant-defined code for the product or service being purchased. Max: 12 characters 
  'measureUnits' => 'EA', // The unit of measure of the item purchase. Normally EA. Max: 3 characters
  'taxRate' => '7%', // Tax rate for the item. Max: 4 digits
  'taxAmount' => '0.21', // Tax amount for the item. unit_cost * quantity * tax_rate = tax_amount. Max: 9 digits.
  'itemDiscount' => '0.00', // The amount of any discounts on the item. Max: 12 digits.
  'lineItemTotal' => '3.21' // The total amount for the item including taxes and discounts.
));

$payment->addLineItem(array(
  'quantity' => '2',
  'unitCost' => '5.00',
  'descriptor' => 'test2',
  'commodityCode' => '123412341234',
  'productCode' => '098709870987',
  'measureUnits' => 'EA',
  'taxRate' => '7%',
  'taxAmount' => '0.70',
  'itemDiscount' => '0.00',
  'lineItemTotal' => '10.70'
));

$payment->sale(array(
  'amount' => '13.91' // Sale Amount: $13.91
));

 // Makes the API request with BluePAy
$payment->process();
 
// Reads the response from BluePay
if($payment->isSuccessfulResponse()){
    echo 
    'Transaction Status: '. $payment->getStatus() . "\n" .
    'Transaction Message: '. $payment->getMessage() . "\n" .
    'Transaction ID: '. $payment->getTransID() . "\n" .
    'AVS Response: ' . $payment->getAVSResponse() . "\n" .
    'CVS Response: ' . $payment->getCVV2Response() . "\n" .
    'Masked Account: ' . $payment->getMaskedAccount() . "\n" .
    'Card Type: ' . $payment->getCardType() . "\n" .
    'Authorization Code: ' . $payment->getAuthCode() . "\n";
} else{
    echo $payment->getMessage() . "\n";
}

?>
