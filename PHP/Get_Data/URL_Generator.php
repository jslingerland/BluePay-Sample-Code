<?php

include('BluePay.php');

$accountID = "Merchant's Account ID Here";
$secretKey = "Merchant's Secret Key Here";
$mode = "TEST";

$testURL = new BluePay(
    $accountID,
    $secretKey,
    $mode
);

$phpGeneratedURL = $testURL->generateURL(array(
    "merchantName" => "Test Merchant",
    "returnURL" => "www.google.com",
    "transactionType" => "SALE", 
    "acceptDiscover" => "Yes", 
    "acceptAmex" => "Yes", 
    "amount" => "99.99", 
    "protectAmount" => "Yes",
    "rebilling" => "Yes",
    "rebProtect" => "Yes", 
    "rebAmount" => "50", 
    "rebCycles" => "12", 
    "rebStartDate" => "1 MONTH", 
    "rebFrequency" => "1 MONTH", 
    "customID1" => "MyCustomID1.1234", 
    "protectCustomID1" => "Yes", 
    "customID2" => "MyCustomID2.12345678910", 
    "protectCustomID2" => "Yes", 
    "paymentTemplate" => "mobileform01", 
    "receiptTemplate" => "defaultres2", 
    "receiptTempRemoteURL" => "" 
));

echo "Hosted Payment Form URL: " . $phpGeneratedURL;

?>