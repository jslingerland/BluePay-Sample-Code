<?php
/**
* BluePay PHP Sample Code
*
* This code sample runs a $0.00 Credit Card Auth transaction
* against a customer using test payment information.
* Once the rebilling cycle is created, this sample shows how to
* update the rebilling cycle. See comments below
* on the details of the initial setup of the rebilling cycle as well as the
* updated rebilling cycle.
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
    'cardExpire' => '1215', // Card Expire: 12/15
    'cvv2' => '123' // Card CVV2: 123
    )
);

$rebill->setRebillingInformation(array(
   'rebillFirstDate' => '2015-01-05', // Rebill Start Date: Jan. 5, 2015
   'rebillExpression' => '1 MONTH', // Rebill Frequency: 1 MONTH
   'rebillCycles' => '5', // Rebill # of Cycles: 5
   'rebillAmount' => '3.50' // Rebill Amount: $3.50
));

$rebill->auth('0.00');

$rebill->process();

// If transaction was approved..
if ($rebill->isSuccessfulResponse()) {

    $updateRebillPaymentInformation = new BluePay(
        $accountID,
        $secretKey,
        $mode
    );

    // Sets an updated credit card expiration date
    $updateRebillPaymentInformation->setCCInformation(array(
        'cardExpire' => '0121'
    ));

    // Stores new card expiration date
    $updateRebillPaymentInformation->auth(
        "0.00", 
        $rebill->getTransID() // the id of the rebill to update
    );

    // Makes the API Request to update the payment information
    $updateRebillPaymentInformation->process();

    // Creates a request to update the rebill
    $updateRebill = new BluePay(
        $accountID,
        $secretKey,
        $mode
    );

    // Updates the rebill
    $updateRebill->updateRebill(array(
        'rebillID' => $rebill->getRebillID(), // The ID of the rebill to be updated.
        'templateID' => $updateRebillPaymentInformation->getTransID(), // Updates the payment information portion of the rebilling cycle with the new card expiration date entered above
        'rebNextDate' => '2015-03-01', // Rebill Start Date: March 1, 2015
        'rebExpr' => '1 MONTH', // Rebill Frequency: 1 MONTH
        'rebCycles' => '8', // Rebill # of Cycles: 8
        'rebAmount' => '5.15', // Rebill Amount: $5.15
        'rebNextAmount' => '1.50' //Rebill Next Amount: $1.50
    ));

    // Makes the API Request to update the rebill
    $updateRebill->process();

    # Read response from BluePay
    echo 
    'Rebill Status: ' . $updateRebill->getRebStatus() . "\n" .
    'Rebill ID: ' . $updateRebill->getRebID() . "\n" .
    'Template ID: ' . $updateRebill->getTemplateID() . "\n" .
    'Rebill Creation Date: ' . $updateRebill->getCreationDate() . "\n" .
    'Rebill Next Date: ' . $updateRebill->getNextDate() . "\n" .
    'Rebill Last Date: ' . $updateRebill->getLastDate() . "\n" .
    'Rebill Expression: ' . $updateRebill->getSchedExpr() . "\n" .
    'Rebill Cycles Remaining: ' . $updateRebill->getCyclesRemaining() . "\n" .
    'Rebill Amount: ' . $updateRebill->getRebAmount() . "\n" .
    'Rebill Next Amount Charged: ' . $updateRebill->getNextAmount();
} else {
    echo $rebill->getMessage();
}
?>