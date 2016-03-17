<?php
/**
* BluePay PHP Sample Code
* This code sample creates a recurring payment charging $15.00 per month for one year.
*/

include('../BluePay.php');

$accountID = "100228390579";
$secretKey = 'AKGIF9X9WT9CLQCWDFONC8N3HXRL9Y5K';
$mode = 'TEST';

$rebill = new BluePay(
    $accountID,
    $secretKey,
    $mode
);

$rebill->setCustomerInformation(array(
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

$rebill->setACHInformation(array(
    'routingNumber' =>'123123123', // Routing Number: 123123123
    'accountNumber' => '1234567890', // Account Number: 1234567890
    'accountType' => 'C', // Account Type: Checking
    'documentType' => 'WEB' // ACH Document Type: WEB
));

$rebill->setRebillingInformation(array(
   'rebillFirstDate' => '2015-01-01', // Rebill Start Date: Jan. 1, 2015
   'rebillExpression' => '1 MONTH', // Rebill Frequency: 1 MONTH
   'rebillCycles' => '12', // Rebill # of Cycles: 12
   'rebillAmount' => '15.00' // Rebill Amount: $15.00
));

$rebill->auth('0.00');

# Makes the API Request with BluePay
$rebill->process();

if ($rebill->isSuccessfulResponse()) {
    # Read response from BluePay
    echo 
    'Transaction ID: '. $rebill->getTransID() . "\n" .
    'Rebill ID: ' . $rebill->getRebillID() . "\n" .
    'Status: '. $rebill->getStatus() . "\n" .
    'Message: '. $rebill->getMessage() . "\n" .
    'Masked Account: ' . $rebill->getMaskedAccount() . "\n" .
    'Customer Bank: ' . $rebill->getBank() . "\n";
} else {
    echo $rebill->getMessage();
}

?>