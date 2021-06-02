<?php
/**
* BluePay PHP Sample Code
*
* This code sample runs a $0.00 Credit Card Auth transaction
* against a customer using test payment information.
* This stores the customer's payment information securely in
* BluePay to be used for further transactions.
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
    #'storedIndicator' => 'F',
    #'storedType' => 'C',
    #'storedId'   => 'TESTID526957756',
    'email' => 'test@bluepay.com'

));

$payment->setCCInformation(array(
    'cardNumber' => '4111111111111111', // Card Number: 4111111111111111
    'cardExpire' => '1225', // Card Expire: 12/25
    'cvv2' => '123' // Card CVV2: 123
));

$payment->auth(array(
    'amount' => '0.00' // Card Authorization amount: $0.00
));

// Makes the API request with BluePay
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
    #'Stored ID: ' . $payment->getStoredId() . "\n" .
    'Authorization Code: ' . $payment->getAuthCode() . "\n" ;
} else{
    echo $payment->getMessage() . "\n";
}

?>
