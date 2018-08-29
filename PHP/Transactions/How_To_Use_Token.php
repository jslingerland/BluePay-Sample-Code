<?php
/**
* BluePay PHP Sample Code
*
* Charges a customer $3.00 using the payment information from a previous transaction. 
* If using TEST mode, odd dollar amounts will return
* an approval and even dollar amounts will return a decline.
*/

include('../BluePay.php');

$accountID = "Merchant's Account ID Here";
$secretKey = "Merchant's Secret Key Here";
$mode = "TEST";
$token = "Transaction ID here"; 

$payment = new BluePay(
    $accountID,
    $secretKey,
    $mode
);

$payment->sale(array(
	'amount' => '3.00', 
	'masterID' => $token 
));

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
