<?php
/**
* BluePay PHP Sample Code
*
* This code sample runs a $0.00 Credit Card Auth transaction
* against a customer using test payment information.
* Once the rebilling cycle is created, this sample shows how to
* get information back on this rebilling cycle.
* See comments below on the details of the initial setup of the
* rebilling cycle.
*/

include('../BluePay.php');

$accountID = "Merchant's Account ID Here";
$secretKey = "Merchant's Secret Key Here";
$mode = "TEST";

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
 
$rebill->setCCInformation(array(
    'cardNumber' => '4111111111111111', // Card Number: 4111111111111111
    'cardExpire' => '1215', // Card Expire: 12/15
    'cvv2' => '123' // Card CVV2: 123
));

$rebill->setRebillingInformation(array(
   'rebillFirstDate' => '2015-01-05', // Rebill Start Date: Jan. 5, 2015
   'rebillExpression' => '1 MONTH', // Rebill Frequency: 1 MONTH
   'rebillCycles' => '5', // Rebill # of Cycles: 5
   'rebillAmount' => '3.50' // Rebill Amount: $3.50
));

$rebill->auth('0.00');

// Makes the API Request to create a recurring payment
$rebill->process();

// If transaction was approved..
if ($rebill->isSuccessfulResponse()) {

    $rebillStatus = new BluePay(
        $accountID,
        $secretKey,
        $mode);

    // Find the rebill by ID and get rebill status 
    $rebillStatus->getRebillStatus($rebill->getRebillID());

    // Makes the API Request to get the rebill status
    $rebillStatus->process();

    // Read response from BluePay
    echo 
    'Rebill Status: ' . $rebillStatus->getRebStatus() . "\n" .
    'Rebill ID: ' . $rebillStatus->getRebID() . "\n" .
    'Template ID: ' . $rebillStatus->getTemplateID() . "\n" .
    'Rebill Creation Date: ' . $rebillStatus->getCreationDate() . "\n" .
    'Rebill Next Date: ' . $rebillStatus->getNextDate() . "\n" .
    'Rebill Last Date: ' . $rebillStatus->getLastDate() . "\n" .
    'Rebill Expression: ' . $rebillStatus->getSchedExpr() . "\n" .
    'Rebill Cycles Remaining: ' . $rebillStatus->getCyclesRemaining() . "\n" .
    'Rebill Amount: ' . $rebillStatus->getRebAmount() . "\n" .
    'Rebill Next Amount Charged: ' . $rebillStatus->getNextAmount();
} else {
    echo $rebill->getMessage();
}
?>