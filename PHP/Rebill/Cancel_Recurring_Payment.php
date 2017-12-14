<?php
/**
* BluePay PHP Sample Code
*
* This code sample runs a $0.00 Credit Card Auth transaction
* against a customer using test payment information, sets up
* a rebilling cycle, and also shows how to cancel that rebilling cycle. See comments below
* on the details of the initial setup of the rebilling cycle.
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
    'addr1' => '12345 Test St.', 
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
    'cardExpire' => '1225', // Card Expire: 12/25
    'cvv2' => '123' // Card CVV2: 123
));

$rebill->setRebillingInformation(array(
   'rebillFirstDate' => '2015-01-01', // Rebill Start Date: Jan. 1, 2015
   'rebillExpression' => '11 MONTH', // Rebill Frequency: 1 MONTH
   'rebillCycles' => '12', // Rebill # of Cycles: 12
   'rebillAmount' => '15.00' // Rebill Amount: $15.00
));

$rebill->auth('0.00');

// Makes the API Request to create a rebill
$rebill->process();

// If transaction was approved..
if ($rebill->isSuccessfulResponse()) {

    $rebillCancel = new BluePay(
        $accountID,
        $secretKey,
        $mode
    );

    // Find rebill by id and cancel rebilling cycle 
    $rebillCancel->cancelRebillingCycle($rebill->getRebillID());

    // Makes the API request to cancel the rebill
    $rebillCancel->process();

    // Reads the response from BluePay
    echo 
    'Rebill Status: ' . $rebillCancel->getRebStatus() . "\n" .
    'Rebill ID: ' . $rebillCancel->getRebID() . "\n" .
    'Rebill Creation Date: ' . $rebillCancel->getCreationDate() . "\n" .
    'Rebill Next Date: ' . $rebillCancel->getNextDate() . "\n" .
    'Rebill Last Date: ' . $rebillCancel->getLastDate() . "\n" .
    'Rebill Expression: ' . $rebillCancel->getSchedExpr() . "\n" .
    'Rebill Cycles Remaining: ' . $rebillCancel->getCyclesRemaining() . "\n" .
    'Rebill Amount: ' . $rebillCancel->getRebAmount() . "\n" .
    'Rebill Next Amount Charged: ' . $rebillCancel->getNextAmount() . "\n"; 
} else {
    echo $rebill->getMessage();
}
?>