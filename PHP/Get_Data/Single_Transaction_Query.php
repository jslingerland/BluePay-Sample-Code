<?php
/**
* BluePay PHP Sample code.
*
* This code sample runs a report that grabs a single transaction
* from the BluePay gateway based on certain criteria.
* See comments below on the details of the report.
* If using TEST mode, only TEST transactions will be returned.
*/

include('../BluePay.php');

$accountID = "100228390579";
$secretKey = 'AKGIF9X9WT9CLQCWDFONC8N3HXRL9Y5K';
$mode = 'TEST';
$transactionID = "100229767898";

$report = new BluePay(
    $accountID,
    $secretKey,
    $mode
);

$report->getSingleTransQuery(array(
		'transID' => '100230382782', // required
		'reportStart' => '2015-01-01', // Report Start Date: YYYY-MM-DD; required
    'reportEnd' => '2015-05-30', // Report End Date: YYYY-MM-DD; required
    'errors'=> '1' // Do not include errored transactions? Yes
));

// Makes the API request with BluePay 
$report->process();

// Reads the response from BluePay
echo 
'Response: ' . $report->getResponse() . "\n" .
'First Name: ' . $report->getName1() . "\n" .
'Last Name:  ' . $report->getName2() . "\n" .
'Transaction ID: ' . $report->getID() . "\n" .
'Payment Type ' . $report->getPaymentType() . "\n" .
'Transaction Type: ' . $report->getTransType() . "\n" .
'Amount: ' . $report->getAmount() . "\n";
?>