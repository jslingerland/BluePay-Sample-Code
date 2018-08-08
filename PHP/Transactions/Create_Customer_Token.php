<?php
/**
* BluePay PHP Sample Code
*
* This code sample runs a $0.00 AUTH transaction
* and creates a customer token using test payment information,
* which is then used to run a separate $3.99 sale.
*/

include('../BluePay.php');

$accountID = "Merchant's Account ID Here";
$secretKey = "Merchant's Secret Key Here";
$mode = "TEST";

$auth = new BluePay(
    $accountID,
    $secretKey,
    $mode
);

$auth->setCustomerInformation(array(
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

$auth->setCCInformation(array(
    'cardNumber' => '4111111111111111', // Card Number: 4111111111111111
    'cardExpire' => '1225', // Card Expire: 12/25
    'cvv2' => '123' // Card CVV2: 123
));

$auth->auth(array(
    'amount' => '0.00', // Card Authorization amount: $0.00
    'newCustomerToken' => TRUE // TRUE generates random string. Other values will be used literally
));

// Makes the API request with BluePay
$auth->process();

// Try again if we accidentally create a non-unique token
if (strpos($auth->getMessage(), "Customer Tokens must be unique") !== false) {
    $auth->setCustomerInformation(array(
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

    $auth->auth(array(
    'amount' => '0.00',
    'newCustomerToken' => TRUE
    ));

    $auth->process();
}

if($auth->isSuccessfulResponse()){
    $payment = new BluePay(
        $accountID,
        $secretKey,
        $mode
    );

    $payment->sale(array(
        'amount' => "3.99",
        'customerToken' => $auth->getCustToken()
    ));

    $payment->process();

    if($payment->isSuccessfulResponse()){
        // Reads the response from BluePay
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
} else{
    echo $auth->getMessage() . "\n";
}

?>