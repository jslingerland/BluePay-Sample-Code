<?php
/**
*
* BluePay PHP sample code.
* This sample code is meant to be used with a BluePay gateway account. If a demo
* gateway account is used, make sure the MODE is set to TEST.
*
*/

class BluePay { 
    
    private $api;

    // Merchant fields
    private $accountID;
    private $secretKey;
    private $mode;

    // Customer fields
    private $name1;
    private $name2;
    private $addr1;
    private $addr2;
    private $city;
    private $state;
    private $zip;
    private $email;
    private $phone;
    private $country;

    // Optional transaction fields
    private $customID1;
    private $customID2;
    private $invoiceID;
    private $orderID;
    private $amountTip;
    private $amountTax;
    private $amountFood;
    private $amountMisc;
    private $memo;

    // Credit card fields
    private $ccNum;
    private $cardExpire;
    private $cvv2;
    private $trackData;

    // ACH fields
    private $accountType;
    private $docType;
    private $accountNum;
    private $routingNum;
  
    // Transaction data
    private $amount;
    private $paymentType;
    private $transType;
    private $masterID;
  
    // Rebilling fields
    private $doRebill;
    private $rebillFirstDate;
    private $rebillNextDate;
    private $rebillExpr;
    private $rebillCycles;
    private $rebillAmount;
    private $rebillNextAmount;
    private $rebillStatus;
    private $templateID;
 
    //Reporting fields
    private $reportStartDate;
    private $reportEndDate;
    private $queryBySettlement; 
    private $subaccountsSearched;
    private $doNotEscape;
    private $excludeErrors;
 
    // Generating Simple Hosted Payment Form URL fields
    private $dba;
    private $returnURL;
    private $discoverImage;
    private $amexImage;
    private $protectAmount;
    private $rebillProtect;
    private $protectCustomID1;
    private $protectCustomID2;
    private $shpfFormID;
    private $receiptFormID;
    private $remoteURL;

    // Response fields
    private $transID;
    private $maskedAccount;
    private $cardType;
    private $customerBank;

    private $postURL;
 
    // Class constructor. Accepts:
    // $accID : Merchant's Account ID
    // $secretKey : Merchant's Secret Key
    // $mode : Transaction mode of either LIVE or TEST (default)
    public function BluePay($accID, $secretKey, $mode) {
        $this->accountID = $accID;
        $this->secretKey = $secretKey;
        $this->mode = $mode;
    }

    // Performs a SALE
    public function sale($amount, $masterID=null) {
        $this->api = 'bp10emu';
        $this->transType = "SALE";
        $this->amount = $amount;
        $this->masterID = $masterID;
        }

    // Performs an AUTH
    public function auth($amount, $masterID=null) {
        $this->api = 'bp10emu';
        $this->transType = "AUTH";
        $this->amount = $amount;
        $this->masterID = $masterID;
    }

    // Performs a CAPTURE
    public function capture($masterID, $amount=null) {
        $this->api = 'bp10emu';
        $this->transType = "CAPTURE";
        $this->masterID = $masterID;
        $this->amount = $amount;
    }

    // performs a REFUND
    public function refund($masterID, $amount=null) {
        $this->api = 'bp10emu';
        $this->transType = "REFUND";
        $this->masterID = $masterID;
        $this->amount = $amount;
    }

    // performs a VOID
    public function void($masterID) {
        $this->api = 'bp10emu';
        $this->transType = "VOID";
        $this->masterID = $masterID;
    }

    // Passes customer information into the transaction
    public function setCustomerInformation($params){
        if(isset($params["firstName"])) {
            $this->name1 = $params["firstName"];
        }
        if(isset($params["lastName"])) {
            $this->name2 = $params["lastName"];
        }
        if(isset($params["addr1"])) {
            $this->addr1 = $params["addr1"];
        }
        if(isset($params["addr2"])) {
            $this->addr2 = $params["addr2"];
        }
        if(isset($params["city"])) {
            $this->city = $params["city"];
        }
        if(isset($params["state"])) {
            $this->state = $params["state"];
        }
        if(isset($params["zip"])) {
            $this->zip = $params["zip"];
        }
        if(isset($params["country"])) {
            $this->country = $params["country"];
        }
    }

    // Passes credit card information into the transaction
    public function setCCInformation($params){
        $this->paymentType = "CREDIT";        
        if(isset($params["cardNumber"])) {
            $this->ccNum = $params["cardNumber"];
        }        
        if(isset($params["cardExpire"])) {
            $this->cardExpire = $params["cardExpire"];
        }        
        if(isset($params["cvv2"])) {
            $this->cvv2 = $params["cvv2"];
        }
    }

    // Sets payment information for a swiped credit card transaction
    public function swipe($trackData) {
        $this->trackData = $trackData;
    }

    // Passes ACH information into the transaction
    public function setACHInformation($params) {
        $this->paymentType = "ACH";
        $this->routingNum = $params['routingNumber'];
        $this->accountNum = $params['accountNumber'];
        $this->accountType = $params['accountType'];
        if(isset($params['documentType'])){
            $this->docType = $params['documentType']; // optional
        }
    }


    // Passes rebilling information into the transaction
    public function setRebillingInformation($params) {
        $this->doRebill = '1';
        $this->rebillFirstDate = $params['rebillFirstDate'];
        $this->rebillExpr = $params['rebillExpression'];
        $this->rebillCycles = $params['rebillCycles'];
        $this->rebillAmount = $params['rebillAmount']; 
    }

    // ### OPTIONAL INPUT PARAMETERS ###
    // Passes value into CUSTOM_ID field
    public function setCustomID1($customID1) {
        $this->customID1 = $customID1;
    }

    // Passes value into CUSTOM_ID2 field
    public function setCustomID2($customID2) {
        $this->customID2 = $customID2;
    }

    // Passes value into INVOICE_ID field
    public function setinvoiceID($invoiceID) {
        $this->invoiceID = $invoiceID;
    }

    // Passes value into ORDER_ID field
    public function setOrderID($orderID) {
        $this->orderID = $orderID;
    } 

    // Passes value into AMOUNT_TIP field
    public function setAmountTip($amountTip) {
        $this->amountTip = $amountTip;
    }

    // Passes value into AMOUNT_TAX field
    public function setAmountTax($amountTax) {
        $this->amountTax = $amountTax;
    }

    // Passes value into AMOUNT_FOOD field
    public function setAmountFood($amountFood) {
        $this->amountFood = $amountFood;
    }

    // Passes value into AMOUNT_MISC field
    public function setAmountMisc($amountMisc) {
        $this->amountMisc = $amountMisc;
    }

    // Passes value into MEMO field
    public function setMemo($memo) {
        $this->memo = $memo;
    }

    // Passes value into PHONE field
    public function setPhone($phone) {
        $this->phone = $phone;
    }

    // Passes value into EMAIL field
    public function setEmail($email) {
        $this->email = $email;
    }

    // Passes value into NEXT_DATE field
    public function setRebillNextDate($rebillNextDate) {
        $this->rebillNextDate = $rebillNextDate;
    }

    // Passes value into REB_EXPR field
    public function setRebillExpression($rebillExpression) {
        $this->rebillExpr = $rebillExpression;
    }

    // Passes value into REB_CYCLES field
    public function setRebillCycles($rebillCycles) {
        $this->rebillCycles = $rebillCycles;
    }

    // Passes value into REB_AMOUNT field
    public function setRebillAmount($rebillAmount) {
        $this->rebillAmount = $rebillAmount;
    }
  
    // Passes value into NEXT_AMOUNT field
    public function setRebillNextAmount($rebillNextAmount) {
        $this->rebillNextAmount = $rebillNextAmount;
    }

    // Passes value into REB_STATUS field
    public function setRebillStatus($rebillStatus) {
        $this->rebillStatus = $rebillStatus;
    }

    // Passes rebilling information for a rebill update
    public function updateRebill($params) {
        $this->api = "bp20rebadmin";
        $this->transType = "SET";
        $this->rebillID = $params["rebillID"];
        if(isset($params["rebNextDate"])) {
                $this->rebillNextDate = $params["rebNextDate"];
        }
        if(isset($params["rebExpr"])) {
                $this->rebillExpr = $params["rebExpr"];
        }
        if(isset($params["rebCycles"])) {
                $this->rebillCycles = $params["rebCycles"];
        }
        if(isset($params["rebAmount"])) {
                $this->rebillAmount = $params["rebAmount"];
        }
        if(isset($params["rebNextAmount"])) {
                $this->rebillNextAmount = $params["rebNextAmount"];
        }
        if(isset($params["templateID"])) {
                $this->templateID = $params["templateID"];
        }
    }

    // Updates an existing rebilling cycle's payment information.   
    public function updateRebillingPaymentInformation($templateID) {
        $this->api = "bp20rebadmin";
        $this->templateID = $templateID;
    }

    // Passes rebilling information for a rebill cancel
    public function cancelRebillingCycle($rebillID) {
        $this->api = "bp20rebadmin";
        $this->transType = "SET";
        $this->rebillStatus = "stopped";
        $this->rebillID = $rebillID;
    }

    // Gets a existing rebilling cycle's status.  
    public function getRebillStatus($rebillID) {
        $this->api = "bp20rebadmin";
        $this->transType = "GET";
        $this->rebillID = $rebillID;
    }

    // Passes values for a call to the bpdailyreport2 API to get all transactions based on start/end dates
    public function getTransactionReport($params) {
        $this->api = "bpdailyreport2";
        $this->queryBySettlement = '0';
        $this->reportStartDate = $params['reportStart'];
        $this->reportEndDate = $params['reportEnd'];
        $this->subaccountsSearched = $params['subaccountsSearched'];
        if(isset($params["doNotEscape"])) {
                $this->doNotEscape = $params["doNotEscape"];
        }
        if(isset($params["errors"])) {
                $this->excludeErrors = $params["errors"];
        }
    }

    // Passes values for a call to the bpdailyreport2 API to get settled transactions based on start/end dates
    public function getSettledTransactionReport($params) {
        $this->api = "bpdailyreport2";
        $this->queryBySettlement = '1';
        $this->reportStartDate = $params['reportStart'];
        $this->reportEndDate = $params['reportEnd'];
        $this->subaccountsSearched = $params['subaccountsSearched'];
        if(isset($params["doNotEscape"])) {
                $this->doNotEscape = $params["doNotEscape"];
        }
        if(isset($params["errors"])) {
                $this->excludeErrors = $params["errors"];
        }
    }

    // Passes values for a call to the stq API to get information on a single transaction
    public function getSingleTransQuery($params) {
        $this->api = "stq";
        $this->transID = $params['transID'];
        $this->reportStartDate = $params['reportStart'];
        $this->reportEndDate = $params['reportEnd'];
        if(isset($params["errors"])) {
                $this->excludeErrors = $params["errors"];
        }
    }

    // Queries transactions by a specific Payment Type. Must be used with getSingleTransQuery
    public function queryByPaymentType($payType) {
        $this->paymentType = $paymentType;
    }
 
    // Queries transactions by a specific Transaction Type. Must be used with getSingleTransQuery
    public function queryBytransType($transType) {
        $this->transType = $transType;
    }

    // Queries transactions by a specific Transaction Amount. Must be used with getSingleTransQuery
    public function queryByAmount($amount) {
        $this->amount = $amount;
    }

    // Queries transactions by a specific First Name. Must be used with getSingleTransQuery
    public function queryByName1($name1) {
        $this->name1 = $name1;
    }

    // Queries transactions by a specific Last Name. Must be used with getSingleTransQuery
    public function queryByName2($name2) {
        $this->name2 = $name2;
    }

    // Functions for calculating the TAMPER_PROOF_SEAL
    public final function calcTPS() {
        $tpsString = $this->secretKey . $this->accountID . $this->transType . $this->amount . $this->doRebill . $this->rebillFirstDate .
        $this->rebillExpr . $this->rebillCycles . $this->rebillAmount . $this->masterID . $this->mode;
        return bin2hex(hash('sha512', $tpsString, true));
    }

    public final function calcRebillTPS() {
        $tpsString = $this->secretKey . $this->accountID . $this->transType . $this->rebillID;
        return bin2hex(md5($tpsString, true));
    }

    public final function calcReportTPS() {
        $tpsString = $this->secretKey . $this->accountID . $this->reportStartDate . $this->reportEndDate;
        return bin2hex(md5($tpsString, true));
    }

    public static final function calcTransNotifyTPS($secretKey, $transID, $transStatus, $transType, $amount, $batchID, $batchStatus, 
        $totalCount, $totalAmount, $batchUploadID, $rebillID, $rebillAmount, $rebillStatus) {
        $tpsString = $secretKey + $transID + $transStatus + $transType + $amount + $batchID + $batchStatus + $totalCount +
        $totalAmount + $batchUploadID + $rebillID + $rebillAmount + $rebillStatus;
        return bin2hex(md5($tpsString, true));
    }

    // Required arguments for generate_url:
    // merchantName: Merchant name that will be displayed in the payment page.
    // returnURL: Link to be displayed on the transacton results page. Usually the merchant's web site home page.
    // transactionType: SALE/AUTH -- Whether the customer should be charged or only check for enough credit available.
    // acceptDiscover: Yes/No -- Yes for most US merchants. No for most Canadian merchants.
    // acceptAmex: Yes/No -- Has an American Express merchant account been set up?
    // amount: The amount if the merchant is setting the initial amount.
    // protectAmount: Yes/No -- Should the amount be protected from changes by the tamperproof seal?
    // rebilling: Yes/No -- Should a recurring transaction be set up?
    // paymentTemplate: Select one of our payment form template IDs or your own customized template ID. If the customer should not be allowed to change the amount, add a 'D' to the end of the template ID. Example: 'mobileform01D'
        // mobileform01 -- Credit Card Only - White Vertical (mobile capable) 
        // default1v5 -- Credit Card Only - Gray Horizontal 
        // default7v5 -- Credit Card Only - Gray Horizontal Donation
        // default7v5R -- Credit Card Only - Gray Horizontal Donation with Recurring
        // default3v4 -- Credit Card Only - Blue Vertical with card swipe
        // mobileform02 -- Credit Card & ACH - White Vertical (mobile capable)
        // default8v5 -- Credit Card & ACH - Gray Horizontal Donation
        // default8v5R -- Credit Card & ACH - Gray Horizontal Donation with Recurring
        // mobileform03 -- ACH Only - White Vertical (mobile capable)
    // receiptTemplate: Select one of our receipt form template IDs, your own customized template ID, or "remote_url" if you have one.
        // mobileresult01 -- Default without signature line - White Responsive (mobile)
        // defaultres1 -- Default without signature line – Blue
        // V5results -- Default without signature line – Gray
        // V5Iresults -- Default without signature line – White
        // defaultres2 -- Default with signature line – Blue
        // remote_url - Use a remote URL
    // receiptTempRemoteURL: Your remote URL ** Only required if receipt_template = "remote_url".

    // Optional arguments for generate_url:
    // rebProtect: Yes/No -- Should the rebilling fields be protected by the tamperproof seal?
    // rebAmount: Amount that will be charged when a recurring transaction occurs.
    // rebCycles: Number of times that the recurring transaction should occur. Not set if recurring transactions should continue until canceled.
    // rebStartDate: Date (yyyy-mm-dd) or period (x units) until the first recurring transaction should occur. Possible units are DAY, DAYS, WEEK, WEEKS, MONTH, MONTHS, YEAR or YEARS. (ex. 2016-04-01 or 1 MONTH)
    // rebFrequency: How often the recurring transaction should occur. Format is 'X UNITS'. Possible units are DAY, DAYS, WEEK, WEEKS, MONTH, MONTHS, YEAR or YEARS. (ex. 1 MONTH) 
    // customID1: A merchant defined custom ID value.
    // protectCustomID1: Yes/No -- Should the Custom ID value be protected from change using the tamperproof seal?
    // customID2: A merchant defined custom ID 2 value.
    // protectCustomID2: Yes/No -- Should the Custom ID 2 value be protected from change using the tamperproof seal?
     
    public function generateURL($params){
        $this->dba = $params['merchantName'];
        $this->returnURL = $params['returnURL'];
        $this->transType = $params['transactionType'];
        $this->discoverImage = empty(preg_match('/^[yY]/', $params['acceptDiscover'], $matches)) ? 'spacer.gif' : 'discvr.gif';
        $this->amexImage = empty(preg_match('/^[yY]/', $params['acceptAmex'], $matches)) ? 'spacer.gif' : 'amex.gif';
        $this->amount = empty($params['amount']) ? '' : $params['amount'];
        $this->protectAmount = empty($params['protectAmount']) ? 'No' : $params['protectAmount'];
        $this->doRebill = empty(preg_match('/^[yY]/', $params['rebilling'], $matches)) ? '0' : '1';
        $this->rebillProtect = empty($params['rebProtect']) ? 'Yes' : $params['rebProtect'];
        $this->rebillAmount = empty($params['rebAmount']) ? '' : $params['rebAmount'];
        $this->rebillCycles = empty($params['rebCycles']) ? '' : $params['rebCycles'];
        $this->rebillFirstDate = empty($params['rebStartDate']) ? '' : $params['rebStartDate'];
        $this->rebillExpr = empty($params['rebFrequency']) ? '' : $params['rebFrequency'];
        $this->customID1 = empty($params['customID1']) ? '' : $params['customID1'];
        $this->protectCustomID1 = empty($params['protectCustomID1']) ? 'No' : $params['protectCustomID1'];
        $this->customID2 = empty($params['customID2']) ? '' : $params['customID2'];
        $this->protectCustomID2 = empty($params['protectCustomID2']) ? 'No' : $params['protectCustomID2'];
        $this->shpfFormID = empty($params['paymentTemplate']) ? 'mobileform01' : $params['paymentTemplate'];
        $this->receiptFormID = empty($params['receiptTemplate']) ? 'mobileresult01' : $params['receiptTemplate'];
        $this->remoteURL = empty($params['receiptTempRemoteURL']) ? '' : $params['receiptTempRemoteURL'];
        $this->cardTypes = $this->setCardTypes();
        $this->receiptTpsDef = 'SHPF_ACCOUNT_ID SHPF_FORM_ID RETURN_URL DBA AMEX_IMAGE DISCOVER_IMAGE SHPF_TPS_DEF';
        $this->receiptTpsString = $this->setReceiptTpsString();
        $this->receiptTamperProofSeal = $this->calcURLTps($this->receiptTpsString);
        $this->receiptURL = $this->setReceiptURL();
        $this->bp10emuTpsDef = $this->addDefProtectedStatus('MERCHANT APPROVED_URL DECLINED_URL MISSING_URL MODE TRANSACTION_TYPE TPS_DEF');
        $this->bp10emuTpsString = $this->setBp10emuTpsString();
        $this->bp10emuTamperProofSeal = $this->calcURLTps($this->bp10emuTpsString);
        $this->shpfTpsDef = $this->addDefProtectedStatus('SHPF_FORM_ID SHPF_ACCOUNT_ID DBA TAMPER_PROOF_SEAL AMEX_IMAGE DISCOVER_IMAGE TPS_DEF SHPF_TPS_DEF');
        $this->shpfTpsString = $this->setShpfTpsString();
        $this->shpfTamperProofSeal = $this->calcURLTps($this->shpfTpsString);
        return $this->calcURLResponse();
    }
    // Sets the types of credit card images to use on the Simple Hosted Payment Form. Must be used with generateURL.
    public function setCardTypes() {
        $creditCards = 'vi-mc';
        if($this->discoverImage == 'discvr.gif') $creditCards .= '-di';
        if($this->amexImage == 'amex.gif') $creditCards .= '-am';
        return $creditCards; 
    }

    // Sets the receipt Tamperproof Seal string. Must be used with generateURL.
    public function setReceiptTpsString() {
        return $this->secretKey . $this->accountID . $this->receiptFormID. $this->returnURL . $this->dba . $this->amexImage . $this->discoverImage . $this->receiptTpsDef; 
    }

    // Sets the bp10emu string that will be used to create a Tamperproof Seal. Must be used with generateURL.
    public function setBp10emuTpsString() {
        $bp10emu = $this->secretKey . $this->accountID . $this->receiptURL . $this->receiptURL . $this->receiptURL . $this->mode . $this->transType . $this->bp10emuTpsDef;
        return $this->addStringProtectedStatus($bp10emu);
    }

    // Sets the Simple Hosted Payment Form string that will be used to create a Tamperproof Seal. Must be used with generateURL.
    public function setShpfTpsString() {
        $shpf = $this->secretKey . $this->shpfFormID . $this->accountID . $this->dba . $this->bp10emuTamperProofSeal . $this->amexImage . $this->discoverImage . $this->bp10emuTpsDef . $this->shpfTpsDef; 
        return $this->addStringProtectedStatus($shpf);
    }

    // Sets the receipt url or uses the remote url provided. Must be used with generateURL.
    public function setReceiptURL() {
        if($this->receiptFormID== 'remote_url') {
            return $this->remoteURL;
        } else {
            return 'https://secure.bluepay.com/interfaces/shpf?SHPF_FORM_ID=' . $this->receiptFormID. 
            '&SHPF_ACCOUNT_ID=' . $this->accountID . 
            '&SHPF_TPS_DEF='    . $this->encodeURL($this->receiptTpsDef) . 
            '&SHPF_TPS='        . $this->encodeURL($this->receiptTamperProofSeal) . 
            '&RETURN_URL='      . $this->encodeURL($this->returnURL) .
            '&DBA='             . $this->encodeURL($this->dba) . 
            '&AMEX_IMAGE='      . $this->encodeURL($this->amexImage) . 
            '&DISCOVER_IMAGE='  . $this->encodeURL($this->discoverImage);
        }
    }

    // Adds optional protected keys to a string. Must be used with generateURL.
    public function addDefProtectedStatus($string) {
        if($this->protectAmount == 'Yes') $string .= ' AMOUNT';
        if($this->rebillProtect == 'Yes') $string .= ' REBILLING REB_CYCLES REB_AMOUNT REB_EXPR REB_FIRST_DATE';
        if($this->protectCustomID1 == 'Yes') $string .= ' CUSTOM_ID';
        if($this->protectCustomID2 == 'Yes') $string .= ' CUSTOM_ID2';
        return $string;
    }

    // Adds optional protected values to a string. Must be used with generateURL.
    public function addStringProtectedStatus($string) {
        if($this->protectAmount == 'Yes') $string .= $this->amount;
        if($this->rebillProtect == 'Yes') $string .= $this->doRebill . $this->rebillCycles . $this->rebillAmount . $this->rebillExpr . $this->rebillFirstDate;
        if($this->protectCustomID1 == 'Yes') $string .= $this->customID1;
        if($this->protectCustomID2 == 'Yes') $string .= $this->customID2;
        return $string;
    }

   // Encodes a string into a URL. Must be used with generateURL.
    public function encodeURL($string) {
        $encodedString = '';
        for( $i = 0; $i < strlen($string); $i++ ) {
            $char = substr($string, $i, 1);
            if(preg_match('/[A-Za-z0-9]/', $char, $match)) {
                $matchString = implode(',', $match);
                $encodedString .= $matchString;
            } else {
                $encodedChar = '%' . strtoupper(dechex(ord($char)));
                $encodedString .= $encodedChar;
            }
        }
        return $encodedString;
    }

    // Generates a Tamperproof Seal for a url. Must be used with generateURL.
    public final function calcURLTps($tpsType) {
        return bin2hex(md5($tpsType, true));
    }

    // Generates the final url for the Simple Hosted Payment Form. Must be used with generateURL.
    public function calcURLResponse() {
        return                  
        'https://secure.bluepay.com/interfaces/shpf?'                               .
        'SHPF_FORM_ID='         . $this->encodeURL($this->shpfFormID)               .
        '&SHPF_ACCOUNT_ID='     . $this->encodeURL($this->accountID)                .
        '&SHPF_TPS_DEF='        . $this->encodeURL($this->shpfTpsDef)               .
        '&SHPF_TPS='            . $this->encodeURL($this->shpfTamperProofSeal)      .
        '&MODE='                . $this->encodeURL($this->mode)                     .
        '&TRANSACTION_TYPE='    . $this->encodeURL($this->transType)                .
        '&DBA='                 . $this->encodeURL($this->dba)                      .
        '&AMOUNT='              . $this->encodeURL($this->amount)                   .
        '&TAMPER_PROOF_SEAL='   . $this->encodeURL($this->bp10emuTamperProofSeal)   .
        '&CUSTOM_ID='           . $this->encodeURL($this->customID1)                .
        '&CUSTOM_ID2='          . $this->encodeURL($this->customID2)                .
        '&REBILLING='           . $this->encodeURL($this->doRebill)                 .
        '&REB_CYCLES='          . $this->encodeURL($this->rebillCycles)             .
        '&REB_AMOUNT='          . $this->encodeURL($this->rebillAmount)             .
        '&REB_EXPR='            . $this->encodeURL($this->rebillExpr)               .
        '&REB_FIRST_DATE='      . $this->encodeURL($this->rebillFirstDate)          .
        '&AMEX_IMAGE='          . $this->encodeURL($this->amexImage)                .
        '&DISCOVER_IMAGE='      . $this->encodeURL($this->discoverImage)            .
        '&REDIRECT_URL='        . $this->encodeURL($this->receiptURL)               .
        '&TPS_DEF='             . $this->encodeURL($this->bp10emuTpsDef)            .
        '&CARD_TYPES='          . $this->encodeURL($this->cardTypes);
    }

    public function process() {
        $post["MODE"] = $this->mode;
        // Case Statement based on which api is used
        switch ($this->api) {
            case "bp10emu":
                $post["MERCHANT"] = $this->accountID;
                $post["TRANSACTION_TYPE"] = $this->transType;
                $post["PAYMENT_TYPE"] = $this->paymentType;
                $post["AMOUNT"] = $this->amount;
                $post["NAME1"] = $this->name1;
                $post["NAME2"] = $this->name2;
                $post["ADDR1"] = $this->addr1;
                $post["ADDR2"] = $this->addr2;
                $post["CITY"] = $this->city;
                $post["STATE"] = $this->state;
                $post["ZIPCODE"] = $this->zip;
                $post["PHONE"] = $this->phone;
                $post["EMAIL"] = $this->email;
                $post["COUNTRY"] = $this->country;
                $post["RRNO"] = $this->masterID;
                $post["CUSTOM_ID"] = $this->customID1;
                $post["CUSTOM_ID2"] = $this->customID2;
                $post["INVOICE_ID"] = $this->invoiceID;
                $post["ORDER_ID"] = $this->orderID;
                $post["AMOUNT_TIP"] = $this->amountTip;
                $post["AMOUNT_TAX"] = $this->amountTax;
                $post["AMOUNT_FOOD"] = $this->amountFood;
                $post["AMOUNT_MISC"] = $this->amountMisc;
                $post["COMMENT"] = $this->memo;
                $post["CC_NUM"] = $this->ccNum;
                $post["CC_EXPIRES"] = $this->cardExpire;
                $post["CVCVV2"] = $this->cvv2;
                $post["ACH_ROUTING"] = $this->routingNum;
                $post["ACH_ACCOUNT"] = $this->accountNum;
                $post["ACH_ACCOUNT_TYPE"] = $this->accountType;
                $post["DOC_TYPE"] = $this->docType;            
                $post["REBILLING"] = $this->doRebill;
                $post["REB_FIRST_DATE"] = $this->rebillFirstDate;
                $post["REB_EXPR"] = $this->rebillExpr;
                $post["REB_CYCLES"] = $this->rebillCycles;
                $post["REB_AMOUNT"] = $this->rebillAmount;
                $post["SWIPE"] = $this->trackData;  
                $post["TAMPER_PROOF_SEAL"] = $this->calcTPS();  
                $post["TPS_HASH_TYPE"] = "SHA512";
                if(isset($_SERVER["REMOTE_ADDR"])){
                    $post["REMOTE_IP"] = $_SERVER["REMOTE_ADDR"];
                }
                $this->postURL = "https://secure.bluepay.com/interfaces/bp10emu";
                break;
            case "bpdailyreport2":
                $post["ACCOUNT_ID"] = $this->accountID;
                $post["REPORT_START_DATE"] = $this->reportStartDate;
                $post["REPORT_END_DATE"] = $this->reportEndDate;
                $post["TAMPER_PROOF_SEAL"] = $this->calcReportTPS();
                $post["DO_NOT_ESCAPE"] = $this->doNotEscape;
                $post["QUERY_BY_SETTLEMENT"] = $this->queryBySettlement;
                $post["QUERY_BY_HIERARCHY"] = $this->subaccountsSearched;
                $post["EXCLUDE_ERRORS"] = $this->excludeErrors;
                $this->postURL = "https://secure.bluepay.com/interfaces/bpdailyreport2";
                break;
            case "stq":
                $post["ACCOUNT_ID"] = $this->accountID;
                $post["REPORT_START_DATE"] = $this->reportStartDate;
                $post["REPORT_END_DATE"] = $this->reportEndDate;
                $post["TAMPER_PROOF_SEAL"] = $this->calcReportTPS();
                $post["EXCLUDE_ERRORS"] = $this->excludeErrors;
                $post["id"] = $this->transID;
                $post["payment_type"] = $this->paymentType;
                $post["trans_type"] = $this->transType;
                $post["amount"] = $this->amount;
                $post["name1"] = $this->name1;
                $post["name2"] = $this->name2; 
                $this->postURL = "https://secure.bluepay.com/interfaces/stq";
                break;
            case "bp20rebadmin":
                $post["ACCOUNT_ID"] = $this->accountID;
                $post["REBILL_ID"] = $this->rebillID;
                $post["TEMPLATE_ID"] = $this->templateID;
                $post["TRANS_TYPE"] = $this->transType;
                $post["NEXT_DATE"] = $this->rebillNextDate;
                $post["REB_EXPR"] = $this->rebillExpr;
                $post["REB_CYCLES"] = $this->rebillCycles;
                $post["REB_AMOUNT"] = $this->rebillAmount;
                $post["NEXT_AMOUNT"] = $this->rebillNextAmount;
                $post["STATUS"] = $this->rebillStatus;
                $post["TAMPER_PROOF_SEAL"] = $this->calcRebillTPS();
                $this->postURL = "https://secure.bluepay.com/interfaces/bp20rebadmin";
            default:
        }

            /* perform the transaction */
            $ch = curl_init();
            curl_setopt($ch, CURLOPT_URL, $this->postURL); // Set the URL
            curl_setopt($ch, CURLOPT_USERAGENT, "Bluepay Payment");
            curl_setopt($ch, CURLOPT_POST, 1); // Perform a POST
            curl_setopt($ch, CURLOPT_HEADER, true);
            curl_setopt($ch, CURLOPT_FOLLOWLOCATION, false);
            curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, 0); // Turns off verification of the SSL certificate.
            curl_setopt($ch, CURLOPT_RETURNTRANSFER, 1); // If not set, curl prints output to the browser
            curl_setopt($ch, CURLOPT_POSTFIELDS, http_build_query($post));
            if ($this->postURL == "https://secure.bluepay.com/interfaces/bp10emu") {
                $responseHeader = curl_exec($ch);           
                list($headers, $response) = explode("\r\n\r\n", $responseHeader, 2);
                $headers = explode("\n", $headers);
                foreach($headers as $header) {
                    if (stripos($header, 'Location:') !== false) {
                        $this->response = $header;
                    }
                }
            } else {
                $this->response = curl_exec($ch);
            }
            curl_close($ch);
            /* parse the response */
            $this->parseResponse();
        } // end Process function

    protected function parseResponse() {
        parse_str($this->response);
        $this->status = isset($Result) ? $Result : null;
        $this->message = isset($MESSAGE) ? $MESSAGE : null;
        $this->transID = isset($RRNO) ? $RRNO : null;
        $this->maskedAccount = isset($PAYMENT_ACCOUNT) ? $PAYMENT_ACCOUNT : null;
        $this->cardType = isset($CARD_TYPE) ? $CARD_TYPE : null;
        $this->customerBank = isset($BANK_NAME) ? $BANK_NAME : null;
        $this->avsResp = isset($AVS) ? $AVS : null;
        $this->cvv2Resp = isset($CVV2) ? $CVV2 : null;
        $this->authCode = isset($AUTH_CODE) ? $AUTH_CODE : null;
        $this->rebid = isset($REBID) ? $REBID : null;

        /* Rebilling response parameters */
        $this->rebillID = isset($rebill_id) ? $rebill_id : null;
        $this->templateID = isset($template_id) ? $template_id : null;
        $this->rebillStatus = isset($status) ? $status : null;
        $this->creationDate = isset($creation_date) ? $creation_date : null;
        $this->nextDate = isset($next_date) ? $next_date : null;
        $this->lastDate = isset($last_date) ? $last_date : null;
        $this->scheduleExpression = isset($sched_expr) ? $sched_expr : null;
        $this->cyclesRemaining = isset($cycles_remain) ? $cycles_remain : null;
        $this->rebAmount = isset($reb_amount) ? $reb_amount : null;
        $this->nextAmount = isset($next_amount) ? $next_amount : null;

        /* Reporting response parameters */
        $this->masterID = isset($id) ? $id : null;
        $this->name1 = isset($name1) ? $name1 : null;
        $this->name2 = isset($name2) ? $name2 : null;
        $this->paymentType = isset($payment_type) ? $payment_type : null;
        $this->transType = isset($trans_type) ? $trans_type : null;
        $this->amount = isset($amount) ? $amount : null;
    }

    public function getResponse() { return $this->response; }

    public function getStatus() { return $this->status; }
    public function getMessage() { return $this->message; }
    public function getTransID() { return $this->transID; }
    public function getMaskedAccount() { return $this->maskedAccount; }
    public function getCardType() { return $this->cardType; }
    public function getBank() { return $this->customerBank; }
    public function getAVSResponse() { return $this->avsResp; }
    public function getCVV2Response() { return $this->cvv2Resp; }
    public function getAuthCode() { return $this->authCode; }
    public function getRebillID() { return $this->rebid; }

    public function getRebID() { return $this->rebillID; }
    public function getTemplateID() { return $this->templateID; }
    public function getRebStatus() { return $this->rebillStatus; }
    public function getCreationDate() { return $this->creationDate; }
    public function getNextDate() { return $this->nextDate; }
    public function getLastDate() { return $this->lastDate; }
    public function getSchedExpr() { return $this->scheduleExpression; }
    public function getCyclesRemaining() { return $this->cyclesRemaining; }
    public function getRebAmount() { return $this->rebAmount; }
    public function getNextAmount() { return $this->nextAmount; }

    public function getID() { return $this->masterID; }
    public function getName1() { return $this->name1; }
    public function getName2() { return $this->name2; }
    public function getPaymentType() { return $this->paymentType; }
    public function getTransType() { return $this->transType; }
    public function getAmount() { return $this->amount; }

    // Returns true if the transaction was approved and not a duplicate
    public function isSuccessfulResponse() {
        // return true;
        return ($this->getStatus() == "APPROVED" && $this->getMessage() != "DUPLICATE"); 
    }
}
?>
