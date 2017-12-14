<?php
/**
* BluePay PHP Sample Code
*
* This code sample runs a $3.00 Credit Card Sale transaction
* against a customer using test payment information.
* If approved, a 2nd transaction is run to cancel this transaction
* If using TEST mode, odd dollar amounts will return
* an approval and even dollar amounts will return a decline.
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
    )
);

$payment->sale('3.00');

$payment->process();

// If transaction was approved..
if ($payment->isSuccessfulResponse()) {

    $cancelPayment = new BluePay(
        $accountID,
        $secretKey,
        $mode
    );

    $cancelPayment->void($payment->getTransID());

    $cancelPayment->process();

    // Read response from BluePay
    echo 
    'Transaction Status: '. $payment->getStatus() . "\n" .
    'Transaction Message: '. $payment->getMessage() . "\n" .
    'Transaction ID: '. $payment->getTransID() . "\n" .
    'AVS Response: ' . $payment->getAVSResponse() . "\n" .
    'CVS Response: ' . $payment->getCVV2Response() . "\n" .
    'Masked Account: ' . $payment->getMaskedAccount() . "\n" .
    'Card Type: ' . $payment->getCardType() . "\n" .
    'Authorization Code: ' . $payment->getAuthCode() . "\n";
} else {
    echo $payment->getMessage();
}
?>