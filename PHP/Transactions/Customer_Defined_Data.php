<?php
/**
* BluePay PHP Sample Code
*
* This code sample runs a $25.00 Credit Card Sale transaction
* against a customer using test payment information.
* Optional transaction data is also sent.
* If using TEST mode, odd dollar amounts will return
* an approval and even dollar amounts will return a decline.
*/

include('../BluePay.php');

$accountID = "100228390579";
$secretKey = 'AKGIF9X9WT9CLQCWDFONC8N3HXRL9Y5K';
$mode = 'TEST';

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
    'cardExpire' => '1215', // Card Expire: 12/15
    'cvv2' => '123' // Card CVV2: 123
));

// # Optional fields users can set
$payment->setCustomID1("12345"); // Custom ID1: 12345
$payment->setCustomID2("09866"); // Custom ID2: 09866
$payment->setInvoiceID("500000"); // Invoice ID: 50000
$payment->setOrderID("10023145"); // Order ID: 10023145
$payment->setAmountTip("6.00"); // Tip Amount: $6.00
$payment->setAmountTax("3.50"); // Tax Amount: $3.50
$payment->setAmountFood("3.11"); // Food Amount: $3.11
$payment->setAmountMisc("5.00"); // Miscellaneous Amount: $5.00

$payment->sale('15.00'); // Sale Amount: $25.00

// Makes the API request with BluePay
$payment->process();

// Reads the response from BluePay
if($payment->isSuccessfulResponse()){
    echo 
    'Status: '. $payment->getStatus() . "\n" .
    'Message: '. $payment->getMessage() . "\n" .
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