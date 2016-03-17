<?php
/**
* BluePay PHP Sample Code
*
* This code sample runs a $3.00  sales transaction using the payment information obtained from a credit card swipe.
* If using TEST mode, odd dollar amounts will return an approval and even dollar amounts will return a decline.*
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

# Set payment information for a swiped credit card transaction 
$payment->swipe("%B4111111111111111^TEST/BLUEPAY^1911101100001100000000667000000?;4111111111111111=191110110000667?");
 
$payment->sale('3.00'); // Sale Amount: $3.00
 
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