<?php
/**
* BluePay PHP Sample code.
*
* This code sample reads the values from a BP10emu redirect
* and authenticates the message using the the BP_STAMP
* provided in the response. Point the REDIRECT_URL of your 
* BP10emu request to the location of this script on your server.
*/

include('BluePay.php');

$accountID = "Merchant's Account ID Here";
$secretKey = "Merchant's Secret Key Here";
$mode = "TEST";

if (array_key_exists('BP_STAMP', $_REQUEST)) { // Check whether BP_STAMP is provided
    
    $bp = new BluePay(
        $accountID,
        $secretKey,
        $mode
    );
    
    $bpStampFields = explode(' ', $_REQUEST['BP_STAMP_DEF']); // Split BP_STAMP_DEF on whitespace
    
    // Concatenate values used to calculate expected BP_STAMP
    $bpStampString = '';
    foreach ($bpStampFields as $field) {
        $bpStampString .= $_REQUEST[$field];
    }
    
    $expectedStamp = strtoupper( $bp->createTPSHash($bpStampString, $_REQUEST['TPS_HASH_TYPE']) ); // Calculate expected BP_STAMP using hash function specified in response
    
    if ($expectedStamp == $_REQUEST['BP_STAMP']) { // Compare expected BP_STAMP with received BP_STAMP
        // Validate BP_STAMP and reads the response results
        echo "VALID BP_STAMP: TRUE<br/>";
        foreach ($_REQUEST as $key => $value){
            echo $key . ': ' . $value . "<br/>";
        }
    } else {
        echo "ERROR: BP_STAMP VALUES DO NOT MATCH<br/>";
    }
} else {
    echo "ERROR: BP_STAMP NOT FOUND. CHECK MESSAGE & RESPONSEVERSION<br/>"; 
} 
?>