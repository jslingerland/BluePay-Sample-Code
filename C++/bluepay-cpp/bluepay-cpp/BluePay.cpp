#include "BluePay.h"

/// <summary>
/// BluePay constructor
/// </summary>
/// <param name="accountID"></param>
/// <param name="secretKey"></param>
/// <param name="mode"></param>
BluePay::BluePay(std::string accountId, std::string secretKey, std::string mode)
{
  this->accountId = accountId;
  this->secretKey = secretKey;
  this->mode = mode;
}

/// <summary>
/// Sets Customer Information
/// </summary>
/// <param name="name1"></param>
/// <param name="name2"></param>
/// <param name="addr1"></param>
/// <param name="city"></param>
/// <param name="state"></param>
/// <param name="zip"></param>
void BluePay::setCustomerInformation(std::string name1, std::string name2, std::string addr1, std::string city, 
    std::string state, std::string zip)
{
  this->name1 = name1;
  this->name2 = name2;
  this->addr1 = addr1;
  this->city = city;
  this->state = state;
  this->zip = zip;
}

/// <summary>
/// Sets Customer Information
/// </summary>
/// <param name="name1"></param>
/// <param name="name2"></param>
/// <param name="addr1"></param>
/// <param name="addr2"></param>
/// <param name="city"></param>
/// <param name="state"></param>
/// <param name="zip"></param>
void BluePay::setCustomerInformation(std::string name1, std::string name2, std::string addr1, std::string addr2, std::string city, 
    std::string state, std::string zip)
{
  this->name1 = name1;
  this->name2 = name2;
  this->addr1 = addr1;
  this->addr2 = addr2;
  this->city = city;
  this->state = state;
  this->zip = zip;
}

/// <summary>
/// Sets Customer Information
/// </summary>
/// <param name="name1"></param>
/// <param name="name2"></param>
/// <param name="addr1"></param>
/// <param name="addr2"></param>
/// <param name="city"></param>
/// <param name="state"></param>
/// <param name="zip"></param>
/// <param name="country"></param>
void BluePay::setCustomerInformation(std::string name1, std::string name2, std::string addr1, std::string addr2, std::string city, 
    std::string state, std::string zip, std::string country)
{
  this->name1 = name1;
  this->name2 = name2;
  this->addr1 = addr1;
  this->addr2 = addr2;
  this->city = city;
  this->state = state;
  this->zip = zip;
  this->country = country;
}

/// <summary>
/// Sets Customer Information
/// </summary>
/// <param name="name1"></param>
/// <param name="name2"></param>
/// <param name="addr1"></param>
/// <param name="addr2"></param>
/// <param name="city"></param>
/// <param name="state"></param>
/// <param name="zip"></param>
/// <param name="country"></param>
/// <param name="phone"></param>
/// <param name="email"></param>
void BluePay::setCustomerInformation(std::string name1, std::string name2, std::string addr1, std::string addr2, std::string city, std::string state, std::string zip, std::string country, std::string phone, std::string email)
{
  this->name1 = name1;
  this->name2 = name2;
  this->addr1 = addr1;
  this->addr2 = addr2;
  this->city = city;
  this->state = state;
  this->zip = zip;
  this->country = country;
  this->phone = phone;
  this->email = email;
}


/// <summary>
/// Sets Credit Card Information
/// </summary>
/// <param name="cardNum"></param>
/// <param name="cardExpire"></param>
void BluePay::setCCInformation(std::string cardNum, std::string cardExpire)
{
  this->paymentType = "CREDIT";
  this->paymentAccount = cardNum;
  this->cardExpire = cardExpire;
}

/// <summary>
/// Sets Credit Card Swipe Information
/// </summary>
/// <param name="trackData"></param>
void BluePay::swipe(std::string trackData)
{
  // encodes track data string and sets it equal to instance variable
  CURL *curl = curl_easy_init();
  this->trackData = curl_easy_escape(curl, trackData.c_str(), 0);
}

/// <summary>
/// Sets Credit Card Information
/// </summary>
/// <param name="cardNum"></param>
/// <param name="cardExpire"></param>
/// <param name="cvv2"></param>
void BluePay::setCCInformation(std::string cardNum, std::string cardExpire, std::string cvv2)
{
  this->paymentType = "CREDIT";
  this->paymentAccount = cardNum;
  this->cardExpire = cardExpire;
  this->cvv2 = cvv2;
}

/// <summary>
/// Sets ACH Information
/// </summary>
/// <param name="routingNum"></param>
/// <param name="accountNum"></param>
/// <param name="accountType"></param>
void BluePay::setACHInformation(std::string routingNum, std::string accountNum, std::string accountType)
{
  this->paymentType = "ACH";
  this->routingNum = routingNum;
  this->accountNum = accountNum;
  this->accountType = accountType;
}

/// <summary>
/// Sets ACH Information
/// </summary>
/// <param name="routingNum"></param>
/// <param name="accountNum"></param>
/// <param name="accountType"></param>
/// <param name="docType"></param>
void BluePay::setACHInformation(std::string routingNum, std::string accountNum, std::string accountType, std::string docType)
{
  this->paymentType = "ACH";
  this->routingNum = routingNum;
  this->accountNum = accountNum;
  this->accountType = accountType;
  this->docType = docType;
}

/// <summary>
/// Sets Rebilling Cycle Information. To be used with other functions to create a transaction.
/// </summary>
/// <param name="rebAmount"></param>
/// <param name="rebFirstDate"></param>
/// <param name="rebExpr"></param>
/// <param name="rebCycles"></param>
void BluePay::setRebillingInformation(std::string rebAmount, std::string rebFirstDate, std::string rebExpr, std::string rebCycles)
{
  this->doRebill = "1";
  this->rebillAmount = rebAmount;
  this->rebillFirstDate = rebFirstDate;
  this->rebillExpr = rebExpr;
  this->rebillCycles = rebCycles;
}

/// <summary>
/// Updates Rebilling Cycle
/// </summary>
/// <param name="rebillId"></param>
/// <param name="rebNextDate"></param>
/// <param name="rebExpr"></param>
/// <param name="rebCycles"></param>
/// <param name="rebAmount"></param>
/// <param name="rebNextAmount"></param>
void BluePay::updateRebillingInformation(std::string rebillId, std::string rebNextDate, std::string rebExpr, std::string rebCycles, 
    std::string rebAmount, std::string rebNextAmount)
{
  this->transType = "SET";
  this->api = "bp20rebadmin";
  this->rebillId = rebillId;
  this->rebillNextDate = rebNextDate;
  this->rebillExpr = rebExpr;
  this->rebillCycles = rebCycles;
  this->rebillAmount = rebAmount;
  this->rebillNextAmount = rebNextAmount;
}

/// <summary>
/// Updates a rebilling cycle's payment information
/// </summary>
/// <param name="templateId"></param>
void BluePay::updateRebillPaymentInformation(std::string templateId)
{
  this->templateId = templateId;
  this->api = "bp20rebadmin";
}

/// <summary>
/// Cancels Rebilling Cycle
/// </summary>
/// <param name="rebillId"></param>
void BluePay::cancelRebilling(std::string rebillId)
{
  this->transType = "SET";
  this->api = "bp20rebadmin"; 
  this->rebillId = rebillId;
  this->rebillStatus = "stopped";
}

/// <summary>
/// Gets a existing rebilling cycle's status
/// </summary>
/// <param name="rebillId"></param>
void BluePay::getRebillStatus(std::string rebillId)
{
  this->transType = "GET";
  this->api = "bp20rebadmin";
  this->rebillId = rebillId;
}

/// <summary>
/// Gets Report of Transaction Data 
/// </summary>
/// <param name="reportStart"></param>
/// <param name="reportEnd"></param>
/// <param name="subaccountsSearched"></param>
void BluePay::getTransactionReport(std::string reportStart, std::string reportEnd, std::string subaccountsSearched)
{
  this->queryBySettlement = "0";
  this->api = "bpdailyreport2";
  this->reportStartDate = reportStart;
  this->reportEndDate = reportEnd;
  this->queryByHierarchy = subaccountsSearched;
}

/// <summary>
/// Gets Report of Transaction Data
/// </summary>
/// <param name="reportStart"></param>
/// <param name="reportEnd"></param>
/// <param name="subaccountsSearched"></param>
/// <param name="doNotEscape"></param>
void BluePay::getTransactionReport(std::string reportStart, std::string reportEnd, std::string subaccountsSearched,
    std::string doNotEscape)
{
  this->queryBySettlement = "0";
  this->api = "bpdailyreport2";
  this->reportStartDate = reportStart;
  this->reportEndDate = reportEnd;
  this->queryByHierarchy = subaccountsSearched;
  this->doNotEscape = doNotEscape;
}

/// <summary>
/// Gets Report of Transaction Data
/// </summary>
/// <param name="reportStart"></param>
/// <param name="reportEnd"></param>
/// <param name="subaccountsSearched"></param>
/// <param name="doNotEscape"></param>
/// <param name="errors"></param>
void BluePay::getTransactionReport(std::string reportStart, std::string reportEnd, std::string subaccountsSearched,
    std::string doNotEscape, std::string errors)
{
  this->queryBySettlement = "0";
  this->api = "bpdailyreport2";
  this->reportStartDate = reportStart;
  this->reportEndDate = reportEnd;
  this->queryByHierarchy = subaccountsSearched;
  this->doNotEscape = doNotEscape;
  this->excludeErrors = errors;
}

/// <summary>
/// Gets Report of Settled Transaction Data
/// </summary>
/// <param name="reportStart"></param>
/// <param name="reportEnd"></param>
/// <param name="subaccountsSearched"></param>
void BluePay::getTransactionSettledReport(std::string reportStart, std::string reportEnd, std::string subaccountsSearched)
{
  this->queryBySettlement = "1";
  this->api = "bpdailyreport2";
  this->reportStartDate = reportStart;
  this->reportEndDate = reportEnd;
  this->queryByHierarchy = subaccountsSearched;
}

/// <summary>
/// Gets Report of Settled Transaction Data
/// </summary>
/// <param name="reportStart"></param>
/// <param name="reportEnd"></param>
/// <param name="subaccountsSearched"></param>
/// <param name="doNotEscape"></param>
void BluePay::getTransactionSettledReport(std::string reportStart, std::string reportEnd, std::string subaccountsSearched,
    std::string doNotEscape)
{
  this->queryBySettlement = "1";
  this->api = "bpdailyreport2";
  this->reportStartDate = reportStart;
  this->reportEndDate = reportEnd;
  this->queryByHierarchy = subaccountsSearched;
  this->doNotEscape = doNotEscape;
}

/// <summary>
/// Gets Report of Settled Transaction Data
/// </summary>
/// <param name="reportStart"></param>
/// <param name="reportEnd"></param>
/// <param name="subaccountsSearched"></param>
/// <param name="doNotEscape"></param>
/// <param name="errors"></param>
void BluePay::getTransactionSettledReport(std::string reportStart, std::string reportEnd, std::string subaccountsSearched,
    std::string doNotEscape, std::string errors)
{
  this->queryBySettlement = "1";
  this->api = "bpdailyreport2";
  this->reportStartDate = reportStart;
  this->reportEndDate = reportEnd;
  this->queryByHierarchy = subaccountsSearched;
  this->doNotEscape = doNotEscape;
  this->excludeErrors = errors;
}

/// <summary>
/// Gets Details of a Transaction
/// </summary>
/// <param name="reportStart"></param>
/// <param name="reportEnd"></param>
void BluePay::getSingleTransQuery(std::string reportStart, std::string reportEnd)
{
  this->api = "stq";
  this->reportStartDate = reportStart;
  this->reportEndDate = reportEnd;
}

/// <summary>
/// Gets Details of a Transaction
/// </summary>
/// <param name="reportStart"></param>
/// <param name="reportEnd"></param>
/// <param name="errors"></param>
void BluePay::getSingleTransQuery(std::string reportStart, std::string reportEnd, std::string errors)
{
  this->api = "stq";
  this->reportStartDate = reportStart;
  this->reportEndDate = reportEnd;
  this->excludeErrors = errors;
}

/// <summary>
/// Gets Details of a Transaction
/// </summary>
/// <param name="transactionID"></param>
/// <param name="reportStart"></param>
/// <param name="reportEnd"></param>
/// <param name="errors"></param>
void BluePay::getSingleTransQuery(std::string transactionID, std::string reportStart, std::string reportEnd, std::string errors)
{
  this->api = "stq";
  this->masterId = transactionID;
  this->reportStartDate = reportStart;
  this->reportEndDate = reportEnd;
  this->excludeErrors = errors;
}


/// <summary>
/// Queries by Transaction ID. To be used with getSingleTransQuery
/// </summary>
/// <param name="transId"></param>
void BluePay::queryByTransactionId(std::string transId)
{
  this->masterId = transId;
}

/// <summary>
/// Queries by Payment Type. To be used with getSingleTransQuery
/// </summary>
/// <param name="payType"></param>
void BluePay::queryByPaymentType(std::string payType)
{
  this->paymentType = payType;
}

/// <summary>
/// Queries by Transaction Type. To be used with getSingleTransQuery
/// </summary>
/// <param name="transType"></param>
void BluePay::queryByTransType(std::string transType)
{
  this->transType = transType;
}

/// <summary>
/// Queries by Transaction Amount. To be used with getSingleTransQuery
/// </summary>
/// <param name="amount"></param>
void BluePay::queryByAmount(std::string amount)
{
  this->amount = amount;
}

/// <summary>
/// Queries by First Name (NAME1) . To be used with getSingleTransQuery
/// </summary>
/// <param name="name1"></param>
void BluePay::queryByName1(std::string name1)
{
  this->name1 = name1;
}

/// <summary>
/// Queries by Last Name (NAME2) . To be used with getSingleTransQuery
/// </summary>
/// <param name="name2"></param>
void BluePay::queryByName2(std::string name2)
{
  this->name2 = name2;
}

/// <summary>
/// Runs a Sale Transaction
/// </summary>
/// <param name="amount"></param>
void BluePay::sale(std::string amount)
{
  this->transType = "SALE";
  this->api = "bp10emu";
  this->amount = amount;
}

/// <summary>
/// Runs a Sale Transaction
/// </summary>
/// <param name="amount"></param>
/// <param name="masterId"></param>
void BluePay::sale(std::string amount, std::string masterId)
{
  this->transType = "SALE";
  this->amount = amount;
  this->masterId = masterId;
  this->api = "bp10emu";
}

/// <summary>
/// Runs an Auth Transaction
/// </summary>
/// <param name="amount"></param>
void BluePay::auth(std::string amount)
{
  this->transType = "AUTH";
  this->api = "bp10emu";
  this->amount = amount;

}

/// <summary>
/// Runs an Auth Transaction
/// </summary>
/// <param name="amount"></param>
/// <param name="masterId"></param>
void BluePay::auth(std::string amount, std::string masterId)
{
  this->transType = "AUTH";
  this->api = "bp10emu";
  this->amount = amount;
  this->masterId = masterId;
}

/// <summary>
/// Runs a Refund Transaction
/// </summary>
/// <param name="masterId"></param>
void BluePay::refund(std::string masterId)
{
  this->transType = "REFUND";
  this->api = "bp10emu";
  this->masterId = masterId;
}

/// <summary>
/// Runs a Refund Transaction
/// </summary>
/// <param name="masterId"></param>
/// <param name="amount"></param>
void BluePay::refund(std::string masterId, std::string amount)
{
  this->transType = "REFUND";
  this->api = "bp10emu";
  this->masterId = masterId;
  this->amount = amount;
}

void BluePay::voidTransaction(std::string masterId)
{
  this->transType = "VOID";
  this->api = "bp10emu";
  this->masterId = masterId;

}

/// <summary>
/// Runs a Capture Transaction
/// </summary>
/// <param name="masterID"></param>
void BluePay::capture(std::string masterId)
{
  this->transType = "CAPTURE";
  this->api = "bp10emu";
  this->masterId = masterId;
}

/// <summary>
/// Runs a Capture Transaction
/// </summary>
/// <param name="masterId"></param>
/// <param name="amount"></param>
void BluePay::capture(std::string masterId, std::string amount)
{
  this->transType = "CAPTURE";
  this->api = "bp10emu";
  this->masterId = masterId;
  this->amount = amount;
}

/// <summary>
/// Sets Custom ID Field
/// </summary>
/// <param name="customId1"></param>
void BluePay::setCustomId1(std::string customId1)
{
  this->customId1 = customId1;
}

/// <summary>
/// Sets Custom ID2 Field
/// </summary>
/// <param name="customId2"></param>
void BluePay::setCustomId2(std::string customId2)
{
  this->customId2 = customId2;
}

/// <summary>
/// Sets Invoice ID Field
/// </summary>
/// <param name="invoiceId"></param>
void BluePay::setInvoiceId(std::string invoiceId)
{
  this->invoiceId = invoiceId;
}

/// <summary>
/// Sets Order ID Field
/// </summary>
/// <param name="orderID"></param>
void BluePay::setOrderId(std::string orderId)
{
  this->orderId = orderId;
}

/// <summary>
/// Sets Amount Tip Field
/// </summary>
/// <param name="amountTip"></param>
void BluePay::setAmountTip(std::string amountTip)
{
  this->amountTip = amountTip;
}

/// <summary>
/// Sets Amount Tax Field
/// </summary>
/// <param name="amountTax"></param>
void BluePay::setAmountTax(std::string amountTax)
{
  this->amountTax = amountTax;
}

/// <summary>
/// Sets Amount Food Field
/// </summary>
/// <param name="amountFood"></param>
void BluePay::setAmountFood(std::string amountFood)
{
  this->amountFood = amountFood;
}

/// <summary>
/// Sets Amount Misc Field
/// </summary>
/// <param name="amountMisc"></param>
void BluePay::setAmountMisc(std::string amountMisc)
{
  this->amountMisc = amountMisc;
}

/// <summary>
/// Sets Memo Field
/// </summary>
/// <param name="memo"></param>
void BluePay::setMemo(std::string memo)
{
  this->memo = memo;
}

/// <summary>
/// Sets Phone Field
/// </summary>
/// <param name="Phone"></param>
void BluePay::setPhone(std::string Phone)
{
  this->phone = Phone;
}

/// <summary>
/// Sets Email Field
/// </summary>
/// <param name="Email"></param>
void BluePay::setEmail(std::string Email)
{
  this->email = Email;
}

/// <summary>
/// Calculates TAMPER_PROOF_SEAL for bp20post API
/// </summary>
void BluePay::calcTps()
{
  std::string tamper_proof_seal = this->secretKey + this->accountId + this->transType + this->amount + this->doRebill + this->rebillFirstDate +
    this->rebillExpr + this->rebillCycles + this->rebillAmount + this->masterId + this->mode;
  this->Tps = md5(tamper_proof_seal);
}

/// <summary>
/// Calculates TAMPER_PROOF_SEAL for bp20rebadmin API
/// </summary>
void BluePay::calcRebillTps()
{
  std::string tamper_proof_seal = this->secretKey + this->accountId + this->transType + this->rebillId;
  this->Tps = md5(tamper_proof_seal);
}

/// <summary>
/// Calculates TAMPER_PROOF_SEAL for bpdailyreport2 and stq APIs
/// </summary>
void BluePay::calcReportTps()
{
  std::string tamper_proof_seal = this->secretKey + this->accountId + this->reportStartDate + this->reportEndDate;
  this->Tps = md5(tamper_proof_seal);
}

/// <summary>
/// Calculates BP_STAMP for trans notify post API
/// </summary>
/// <param name="secretKey"></param>
/// <param name="transId"></param>
/// <param name="transStatus"></param>
/// <param name="transType"></param>
/// <param name="amount"></param>
/// <param name="batchId"></param>
/// <param name="batchStatus"></param>
/// <param name="totalCount"></param>
/// <param name="totalAmount"></param>
/// <param name="batchUploadId"></param>
/// <param name="rebillId"></param>
/// <param name="rebillAmount"></param>
/// <param name="rebillStatus"></param>
/// <returns></returns>
std::string BluePay::calcTransNotifyTps(std::string secretKey, std::string transId, std::string transStatus, std::string transType,
    std::string amount, std::string batchId, std::string batchStatus, std::string totalCount, std::string totalAmount,
    std::string batchUploadId, std::string rebillId, std::string rebillAmount, std::string rebillStatus)
{
  std::string tamper_proof_seal = secretKey + transId + transStatus + transType + amount + batchId + batchStatus +
    totalCount + totalAmount + batchUploadId + rebillId + rebillAmount + rebillStatus;
  return md5(tamper_proof_seal);
}

static size_t WriteCallback(void *contents, size_t size, size_t nmemb, void *userp)
{
  ((std::string*)userp)->append((char*)contents, size * nmemb);
  return size * nmemb;
}

std::vector<std::string> &split(const std::string &s, char delim, std::vector<std::string> &elems) 
{
  std::stringstream ss(s);
  std::string item;
  while (std::getline(ss, item, delim))  {
    elems.push_back(item);
  }
  return elems;
}


std::vector<std::string> split(const std::string &s, char delim) 
{
  std::vector<std::string> elems;
  split(s, delim, elems);
  return elems;
}

char* getResponseField(char *nvp, const char *Name, char *Value) 
{
  char *pos1 = strstr(nvp, Name);

  if (pos1) {
    pos1 += strlen(Name);

    if (*pos1 == '=') {
      pos1++;

      while (*pos1 && *pos1 != '&') {
        if (*pos1 == '%') {
          *Value++ = (char)ToHex(pos1[1]) * 16 + ToHex(pos1[2]);
          pos1 += 3;
        } else if (*pos1 == '+') {
          *Value++ = ' ';
          pos1++;
        } else {
          *Value++ = *pos1++;
        }
      }

      *Value++ = '\0';
      return Value;
    }
  }
    return "Error";
}

char* BluePay::process()
{  
    std::string postData = "";

  if (this->api == "bpdailyreport2") {
    calcReportTps();
    this->URL = "https://secure.bluepay.com/interfaces/bpdailyreport2";
    postData += "ACCOUNT_ID=" + this->accountId +
      "&MODE=" + (this->mode) +
      "&TAMPER_PROOF_SEAL=" + (this->Tps) +
      "&REPORT_START_DATE=" + (this->reportStartDate) +
      "&REPORT_END_DATE=" + (this->reportEndDate) +
      "&DO_NOT_ESCAPE=" + (this->doNotEscape) +
      "&QUERY_BY_SETTLEMENT=" + (this->queryBySettlement) +
      "&QUERY_BY_HIERARCHY=" + (this->queryByHierarchy) +
      "&EXCLUDE_ERRORS=" + (this->excludeErrors);
  // } else if (this->reportStartDate != "") {
  } else if (this->api == "stq") {
    calcReportTps();
    this->URL = "https://secure.bluepay.com/interfaces/stq";
    postData += "ACCOUNT_ID=" + (this->accountId) +
      "&MODE=" + (this->mode) +
      "&TAMPER_PROOF_SEAL=" + (this->Tps) +
      "&REPORT_START_DATE=" + (this->reportStartDate) +
      "&REPORT_END_DATE=" + (this->reportEndDate) +
      "&EXCLUDE_ERRORS=" + (this->excludeErrors);
    postData += (this->masterId != "") ? "&id=" + (this->masterId) : "";
    postData += (this->paymentType != "") ? "&payment_type=" + (this->paymentType) : "";
    postData += (this->transType != "") ? "&trans_type=" + (this->transType) : "";
    postData += (this->amount != "") ? "&amount=" + (this->amount) : "";
    postData += (this->name1 != "") ? "&name1=" + (this->name1) : "";
    postData += (this->name2 != "") ? "&name2=" + (this->name2) : "";
  } else if (this->api == "bp10emu") {
    calcTps();
    this->URL = "https://secure.bluepay.com/interfaces/bp10emu";
    postData += "MERCHANT=" + (this->accountId) +
      "&MODE=" + (this->mode) +
      "&TRANSACTION_TYPE=" + (this->transType) +
      "&TAMPER_PROOF_SEAL=" + (this->Tps) +
      "&NAME1=" + (this->name1) +
      "&NAME2=" + (this->name2) +
      "&AMOUNT=" + (this->amount) +
      "&ADDR1=" + (this->addr1) +
      "&ADDR2=" + (this->addr2) +
      "&CITY=" + (this->city) +
      "&STATE=" + (this->state) +
      "&ZIPCODE=" + (this->zip) +
      "&COMMENT=" + (this->memo) +
      "&PHONE=" + (this->phone) +
      "&EMAIL=" + (this->email) +
      "&REBILLING=" + (this->doRebill) +
      "&REB_FIRST_DATE=" + (this->rebillFirstDate) +
      "&REB_EXPR=" + (this->rebillExpr) +
      "&REB_CYCLES=" + (this->rebillCycles) +
      "&REB_AMOUNT=" + (this->rebillAmount) +
      "&RRNO=" + (this->masterId) +
      "&PAYMENT_TYPE=" + (this->paymentType) +
      "&CUSTOM_ID=" + (this->customId1) +
      "&CUSTOM_ID2=" + (this->customId2) +
      "&INVOICE_ID=" + (this->invoiceId) +
      "&ORDER_ID=" + (this->orderId) +
      "&AMOUNT_TIP=" + (this->amountTip) +
      "&AMOUNT_TAX=" + (this->amountTax) +
      "&AMOUNT_FOOD=" + (this->amountFood) +
      "&AMOUNT_MISC=" + (this->amountMisc) +
      "&SWIPE=" + (this->trackData) +
      "&RESPONSEVERSION=1"
      ;
      // std::cout << postData;
      
    if (this->paymentType == "CREDIT") {
      postData = postData + "&CC_NUM=" + (this->paymentAccount) +
        "&CC_EXPIRES=" + (this->cardExpire) +
        "&CVCVV2=" + (this->cvv2);
    } else {
      postData = postData + "&ACH_ROUTING=" + (this->routingNum) +
        "&ACH_ACCOUNT=" + (this->accountNum) +
        "&ACH_ACCOUNT_TYPE=" + (this->accountType) +
        "&DOC_TYPE=" + (this->docType);
    }
  } else if (this->api == "bp20rebadmin"){
    calcRebillTps();
    this->URL = "https://secure.bluepay.com/interfaces/bp20rebadmin";
    postData += "ACCOUNT_ID=" + (this->accountId) +
      "&TAMPER_PROOF_SEAL=" + (this->Tps) +
      "&TRANS_TYPE=" + (this->transType) +
      "&REBILL_ID=" + (this->rebillId) +
      "&TEMPLATE_ID=" + (this->templateId) +
      "&REB_EXPR=" + (this->rebillExpr) +
      "&REB_CYCLES=" + (this->rebillCycles) +
      "&REB_AMOUNT=" + (this->rebillAmount) +
      "&NEXT_AMOUNT=" + (this->rebillNextAmount) +
      "&STATUS=" + (this->rebillStatus);
  } 
  //Create HTTPS POST object and send to BluePay
  CURL *curl;
  CURLcode res;
  std::string readBuffer;
  curl_global_init(CURL_GLOBAL_ALL);
  char *postToBp = (char*)postData.c_str();

  curl = curl_easy_init();
  if (curl) {
    curl_easy_setopt(curl, CURLOPT_URL, this->URL);
    curl_easy_setopt(curl, CURLOPT_POSTFIELDS, postToBp);
    curl_easy_setopt(curl, CURLOPT_SSL_VERIFYPEER, 0);
    curl_easy_setopt(curl, CURLOPT_FOLLOWLOCATION, 0);
    if (this->URL == "https://secure.bluepay.com/interfaces/bp10emu") {
      curl_easy_setopt(curl, CURLOPT_HEADER, 1);
    }
    curl_easy_setopt(curl, CURLOPT_WRITEFUNCTION, WriteCallback);
    curl_easy_setopt(curl, CURLOPT_WRITEDATA, &readBuffer);

    res = curl_easy_perform(curl);
    if (res != CURLE_OK) {
      fprintf(stderr, "curl_easy_perform() failed: %s\n",
      curl_easy_strerror(res));
    }
    curl_easy_cleanup(curl);
  }
  curl_global_cleanup();
  std::istringstream stream(readBuffer);
  std::string line;
  //std::cout << readBuffer;
  while (std::getline(stream, line))  {
    if (line.find("Location") == 0) {
      std::vector<std::string> nvp = split(line, '?');
      queryResponse = (char*)nvp[1].c_str();
      getResponseField(queryResponse, "Result", result);
      getResponseField(queryResponse, "MESSAGE", message);
      getResponseField(queryResponse, "TRANS_ID", transId);
      getResponseField(queryResponse, "CVV2_RESULT", cvv2Response);
      getResponseField(queryResponse, "AVS_RESULT", avsResponse);
      getResponseField(queryResponse, "PAYMENT_ACCOUNT", maskedAccount);
      getResponseField(queryResponse, "CARD_TYPE", cardType);
      getResponseField(queryResponse, "BANK_NAME", bank);
      getResponseField(queryResponse, "AUTH_CODE", authCode);
      getResponseField(queryResponse, "REBID", rebId);
      break;
    } else if (this->URL != "https://secure.bluepay.com/interfaces/bp10emu") {
      std::vector<std::string> nvp = split(line, '?');
      queryResponse = (char*)line.c_str();
      getResponseField(queryResponse, "rebill_id", rebId);
      getResponseField(queryResponse, "status", rebStatus);
      getResponseField(queryResponse, "creation_date", rebCreationDate);
      getResponseField(queryResponse, "next_date", rebNextDate);
      getResponseField(queryResponse, "last_date", rebLastDate);
      getResponseField(queryResponse, "sched_expr", rebSchedExpr);
      getResponseField(queryResponse, "cycles_remain", rebCyclesRemaining);
      getResponseField(queryResponse, "reb_amount", rebAmount);
      getResponseField(queryResponse, "next_amount", rebNextAmount);
      break;
    }
  }
  this->response = readBuffer;
  //std::cout << "response:" + this->getResponse();
  return getMessage();
}

/// <summary>
/// Returns response
/// </summary>
/// <returns></returns>
std::string BluePay::getResponse()
{
  return this->response;
}

/// <summary>
/// Returns STATUS or status from response
/// </summary>
/// <returns></returns>
char* BluePay::getResult()
{
  return this->result;
}

/// <summary>
/// Returns TRANS_ID from response
/// </summary>
/// <returns></returns>
char* BluePay::getTransId()
{
  return this->transId;
}

/// <summary>
/// Returns MESSAGE from Response
/// </summary>
/// <returns></returns>
char* BluePay::getMessage()
{
  return this->message;
}

/// <summary>
/// Returns CVV2 from Response
/// </summary>
/// <returns></returns>
char* BluePay::getCvv2()
{
  return this->cvv2Response;
}

/// <summary>
/// Returns AVS from Response
/// </summary>
/// <returns></returns>
char* BluePay::getAvs()
{
  return this->avsResponse;
}

/// <summary>
/// Returns PAYMENT_ACCOUNT from response
/// </summary>
/// <returns></returns>
char* BluePay::getMaskedPaymentAccount()
{
  return this->maskedAccount;
}

/// <summary>
/// Returns CARD_TYPE from response
/// </summary>
/// <returns></returns>
char* BluePay::getCardType()
{
  return this->cardType;
}

/// <summary>
/// Returns BANK_NAME from Response
/// </summary>
/// <returns></returns>
char* BluePay::getBank()
{
  return this->bank;
}

/// <summary>
/// Returns AUTH_CODE from Response
/// </summary>
/// <returns></returns>
char* BluePay::getAuthCode()
{
  return this->authCode;
}

/// <summary>
/// Returns REBID or rebill_id from Response
/// </summary>
/// <returns></returns>
char* BluePay::getRebillId()
{
  return this->rebId;
}

/// <summary>
/// Returns status from Response
/// </summary>
/// <returns></returns>
char* BluePay::getStatus()
{
  return this->rebStatus;
}

/// <summary>
/// Returns creation_date from Response
/// </summary>
/// <returns></returns>
char* BluePay::getCreationDate()
{
  return this->rebCreationDate;
}

/// <summary>
/// Returns next_date from Response
/// </summary>
/// <returns></returns>
char* BluePay::getNextDate()
{
  return this->rebNextDate;
}

/// <summary>
/// Returns last_date from Response
/// </summary>
/// <returns></returns>
char* BluePay::getLastDate()
{
  return this->rebLastDate;
}

/// <summary>
/// Returns sched_expr from Response
/// </summary>
/// <returns></returns>
char* BluePay::getSchedExpr()
{
  return this->rebSchedExpr;
}

/// <summary>
/// Returns cycles_remain from Response
/// </summary>
/// <returns></returns>
char* BluePay::getCyclesRemain()
{
  return this->rebCyclesRemaining;
}

/// <summary>
/// Returns reb_amount from Response
/// </summary>
/// <returns></returns>
char* BluePay::getRebillAmount()
{
  return this->rebAmount;
}

/// <summary>
/// Returns next_amount from Response
/// </summary>
/// <returns></returns>
char* BluePay::getNextAmount()
{
  return this->rebNextAmount;
}

/// <summary>
/// Returns true if the transaction is successful
/// </summary>
bool BluePay::isSuccessfulTransaction()
{
  std::string status; 
  std::string message;
  status = this->getResult();
  message = this->getMessage();
  if ((status == "APPROVED") && (message != "DUPLICATE")){
    return true;
  } else {
    return false;
  }
  // std::cout << status;
  // std::cout << message;
  // return true;
}