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
    'unit_cost' => '3.00', // The cost per unit of item. Max: 9 digits decimal
    'descriptor' => 'test1', // Description of the item purchased. Max: 26 character
    'commodity_code' => '123412341234', // Commodity Codes can be found at http://www.census.gov/svsd/www/cfsdat/2002data/cfs021200.pdf. Max: 12 characters
    'product_code' => '432143214321', // Merchant-defined code for the product or service being purchased. Max: 12 characters 
    'measure_units' => 'EA', // The unit of measure of the item purchase. Normally EA. Max: 3 characters
    'tax_rate' => '7%', // Tax rate for the item. Max: 4 digits
    'tax_amount' => '0.21', // Tax amount for the item. unit_cost * quantity * tax_rate = tax_amount. Max: 9 digits.
    'item_discount' => '0.00', // The amount of any discounts on the item. Max: 12 digits.
    'line_item_total' => '3.21' // The total amount for the item including taxes and discounts.
));

$payment->addLineItem(array(
  'quantity' => '2',
  'unit_cost' => '5.00',
  'descriptor' => 'test2',
  'commodity_code' => '123412341234',
  'product_code' => '098709870987',
  'measure_units' => 'EA',
  'tax_rate' => '7%',
  'tax_amount' => '0.70',
  'item_discount' => '0.00',
  'line_item_total' => '10.70'
));

$payment->sale('13.91'); // Sale Amount: $13.91
 
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
