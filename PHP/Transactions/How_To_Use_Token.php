<?php
/**
* BluePay PHP Sample Code
*
* Charges a customer $3.00 using the payment information from a previous transaction. 
* If using TEST mode, odd dollar amounts will return
* an approval and even dollar amounts will return a decline.
*/

include('../BluePay.php');

$accountID = '100228390579';
$secretKey = 'AKGIF9X9WT9CLQCWDFONC8N3HXRL9Y5K';
$mode = 'TEST';

$token = "100228422096"; # The transaction ID of a previous sale

$payment = new BluePay(
    $accountID,
    $secretKey,
    $mode
);

$payment->sale(
	'3.00', 
	$token # The transaction ID of a previous sale
);

$payment->process();

if($payment->isSuccessfulResponse()){
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
} else{
    echo $payment->getMessage() . "\n";
}
?>