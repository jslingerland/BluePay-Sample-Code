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

$secretKey = '';

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

// calculate expected bp_stamp
$bpStamp = BluePay::calcTransNotifyTPS(
    $secretKey,
    $transID,
    $transStatus,
    $transType,
    $amount,
    $batchID,
    $batchStatus,
    $totalCount,
    $totalAmount,
    $batchUploadID,
    $rebillID,
    $rebillAmount,
    $rebillStatus);

// check if expected bp_stamp = actual bp_stamp
if (isset($_REQUEST['bp_stamp'])) {

    if ($bpStamp == $_REQUEST['bp_stamp']) {

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