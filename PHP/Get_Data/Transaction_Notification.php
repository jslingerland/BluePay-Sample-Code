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
$batchID = isset($_REQUEST['batch_id']) ? $_REQUEST['batch_id'] : null;
$batchStatus = isset($_REQUEST['batch_status']) ? $_REQUEST['batch_status'] : null;
$totalCount = isset($_REQUEST['total_count']) ? $_REQUEST['total_count'] : null;
$totalAmount = isset($_REQUEST['total_amount']) ? $REQUEST['total_amount'] : null;
$batchUploadID = isset($_REQUEST['bupload_id']) ? $REQUEST['bupload_id'] : null;
$rebillID = isset($_REQUEST['rebill_id']) ? $_REQUEST['rebill_id'] : null;
$rebillAmount = isset($_REQUEST['reb_amount']) ? $_REQUEST['reb_amount'] : null;
$rebillStatus = isset($_REQUEST['status']) ? $_REQUEST['status'] : null;
$tpsHashType = isset($_REQUEST['TPS_HASH_TYPE']) ? $_REQUEST['TPS_HASH_TYPE'] : null;

// calculate expected bp_stamp
$bpStamp = $tps->createTPSHash(
    $transID +
    $transStatus +
    $transType +
    $amount +
    $batchID +
    $batchStatus +
    $totalCount +
    $totalAmount +
    $batchUploadID +
    $rebillID +
    $rebillAmount +
    $rebillStatus,
    $tpsHashType);

// check if expected bp_stamp = actual bp_stamp
if (isset($_REQUEST['BP_STAMP'])) {

    if ($bpStamp == $_REQUEST['BP_STAMP']) {

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