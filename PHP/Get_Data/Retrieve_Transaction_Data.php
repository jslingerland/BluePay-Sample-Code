<?php
/**
* BluePay PHP Sample Code
*
* This code sample runs a report that grabs data from the
* BluePay gateway based on certain criteria. See comments below
* on the details of the report.
* If using TEST mode, only TEST transactions will be returned.
*/
  
include('../BluePay.php');
  
$accountID = "100228390579";
$secretKey = 'AKGIF9X9WT9CLQCWDFONC8N3HXRL9Y5K';
$mode = 'TEST';
  
$report = new BluePay(
    $accountID,
    $secretKey,
    $mode
);
  
/* RUN A TRANSACTION REPORT */
$report->getTransactionReport(array(
    'reportStart' => '2015-01-01', // Report Start Date: YYYY-MM-DD
    'reportEnd' => '2015-04-30', // Report End Date: YYYY-MM-DD
    'subaccountsSearched' => '1', // Also search subaccounts? Yes
    'doNotEscape' => '1', // Output response without commas? Yes
    'errors'=> '1' // Do not include errored transactions? Yes
));

// Makes the API request with BluePay  
$report->process();
  
// Reads the response from BluePay
echo 
'Response: '. $report->getResponse() . "\n";
?>