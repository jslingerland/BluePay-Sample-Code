<?php
/**
* BluePay PHP Sample code.
*
* This code sample shows a very based approach
* on handling data that is posted to a script running
* a merchant's server after a transaction is processed
* through their BluePay gateway account.
*/

include('../BluePay.php');

$accountID = "Merchant's Account ID Here";
$secretKey = "Merchant's Secret Key Here";
$mode = "TEST";

$tps = new BluePay(
    $accountID,
    $secretKey,
    $mode
);

// get POST parameters
$transID = isset($_REQUEST['trans_id']) ? $_REQUEST['trans_id'] : null;
$transStatus = isset($_REQUEST['trans_status']) ? $_REQUEST['trans_status'] : null;
$transType = isset($_REQUEST['trans_type']) ? $_REQUEST['trans_type'] : null;
$amount = isset($_REQUEST['amount']) ? $_REQUEST['amount'] : null;
$rebillID = isset($_REQUEST['rebill_id']) ? $_REQUEST['rebill_id'] : null;
$rebillAmount = isset($_REQUEST['reb_amount']) ? $_REQUEST['reb_amount'] : null;
$rebillStatus = isset($_REQUEST['status']) ? $_REQUEST['status'] : null;
$bpStamp = isset($_REQUEST['BP_STAMP']) ? $_REQUEST['BP_STAMP'] : null;
$bpStampDef = isset($_REQUEST['BP_STAMP_DEF']) ? $_REQUEST['BP_STAMP_DEF'] : null;
$tpsHashType = isset($_REQUEST['TPS_HASH_TYPE']) ? $_REQUEST['TPS_HASH_TYPE'] : null;

// calculate expected bp_stamp
$bpStampFields = explode(' ', $bpStampDef); // Split BP_STAMP_DEF on whitespace
$bpStampString = '';

$fieldValue = '';
foreach ($bpStampFields as $field) {
    $fieldValue = isset($_REQUEST[$field]) ? $_REQUEST[$field] : null;
    $bpStampString .= $fieldValue; // Concatenate values used to calculate expected BP_STAMP
}
$expectedStamp = $tps->createTPSHash($bpStampString, $tpsHashType);

// check if expected bp_stamp = actual bp_stamp
if (!empty($bpStamp)) {

    if ($bpStamp == $expectedStamp) {

        // Read response from BluePay
        echo 'Transaction ID: ' . $transID . '<br />' .
        'Transaction Status: ' . $transStatus . '<br />' .
        'Transaction Type: ' . $transType . '<br />' .
        'Transaction Amount: ' . $amount . '<br />' .
        'Rebill ID: ' . $rebillID . '<br />' .
        'Rebill Amount: ' . $rebillAmount . '<br />' .
        'Rebill Status: ' . $rebillStatus . '<br />';
    } else {
        echo 'ERROR IN RECEIVING DATA FROM BLUEPAY';
    }
}
?>