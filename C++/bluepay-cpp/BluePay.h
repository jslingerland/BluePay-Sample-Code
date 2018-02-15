#include <string>
#include <vector>
#include <map>
#include <sstream>
#include <iterator>
//#include <Windows.h> 
//#include <wininet.h>
// #pragma comment(lib, "Wininet")

#pragma once
// #include <iostream>
#include <stdio.h>
//#include <tchar.h>

#include "hmac.h"
#include "sha512.h"
#include "sha256.h"
#include "md5.h"
#include <curl/curl.h>
#define ToHex(Y) (Y>='0'&&Y<='9'?Y-'0':Y-'A'+10)


class BluePay {
private:
  // required for every transaction
  std::string accountId;
  std::string URL;
  std::string secretKey;
  std::string mode;

  std::string tpsHashType = "HMAC_SHA512";

  // required for auth or sale
  std::string paymentAccount;
  std::string cvv2;
  std::string cardExpire;
  std::string routingNum;
  std::string accountNum;
  std::string accountType;
  std::string docType;
  std::string name1;
  std::string name2;
  std::string addr1;
  std::string city;
  std::string state;
  std::string zip;

  // optional for auth or sale
  std::string addr2;
  std::string phone;
  std::string email;
  std::string country;

  // transaction variables
  std::string amount;
  std::string transType;
  std::string paymentType;
  std::string masterId;
  std::string rebillId;
  
  std::string trackData;

  // rebill variables
  std::string doRebill;
  std::string rebillAmount;
  std::string rebillFirstDate;
  std::string rebillExpr;
  std::string rebillCycles;
  std::string rebillNextAmount;
  std::string rebillNextDate;
  std::string rebillStatus;
  std::string templateId;

  // level2 variables
  std::string customId1;
  std::string customId2;
  std::string invoiceId;
  std::string orderId;
  std::string amountTip;
  std::string amountTax;
  std::string amountFood;
  std::string amountMisc;
  std::string memo;
  std::map<std::string, std::string> level2Info;
    
  // level3 fields
  std::vector<std::map<std::string, std::string>> lineItems;
    
  // rebill fields
  std::string reportStartDate;
  std::string reportEndDate;
  std::string doNotEscape;
  std::string queryBySettlement;
  std::string queryByHierarchy;
  std::string excludeErrors;

  // Generating Simple Hosted Payment Form URL fields
  std::string dba;
  std::string returnURL;
  std::string discoverImage;
  std::string amexImage;
  std::string protectAmount;
  std::string rebillProtect;
  std::string protectCustomID1;
  std::string protectCustomID2;
  std::string shpfFormID;
  std::string receiptFormID;
  std::string remoteURL;
  std::string shpfTpsHashType;
  std::string receiptTpsHashType;
  std::string cardTypes;
  std::string receiptTpsDef;
  std::string receiptTpsString;
  std::string receiptTamperProofSeal;
  std::string receiptURL;
  std::string bp10emuTpsDef;
  std::string bp10emuTpsString;
  std::string bp10emuTamperProofSeal;
  std::string shpfTpsDef;
  std::string shpfTpsString;
  std::string shpfTamperProofSeal;

  std::string api;

  char *queryResponse;
  std::string response;
  std::map<std::string, std::string> responseFields;
  char result[128];
  char message[128];
  char transId[16];
  char cvv2Response[8];
  char avsResponse[8];
  char maskedAccount[32];
  char cardType[8];
  char bank[64];
  char authCode[16];
  char rebId[16];
  char rebStatus[16];
  char rebCreationDate[32];
  char rebNextDate[32];
  char rebLastDate[32];
  char rebCyclesRemaining[8];
  char rebSchedExpr[16];
  char rebAmount[16];
  char rebNextAmount[16];
  std::string Tps;
  std::string BPheaderstring;

  size_t size;
  char* data;
  std::vector<std::string> flds;

public:
  BluePay();
  BluePay(std::string, std::string, std::string);

  void setCustomerInformation(std::string name1, std::string name2, std::string addr1, std::string city, std::string state, std::string zip);
  void setCustomerInformation(std::string name1, std::string name2, std::string addr1, std::string addr2, std::string city, std::string state,
    std::string zip);
  void setCustomerInformation(std::string name1, std::string name2, std::string addr1, std::string addr2, std::string city, std::string state,
    std::string zip, std::string country);
  void setCustomerInformation(std::string name1, std::string name2, std::string addr1, std::string addr2, std::string city, 
    std::string state, std::string zip, std::string country, std::string phone, std::string email);
  void setCCInformation(std::string cardNum, std::string cardExpire);
  void setCCInformation(std::string cardNum, std::string cardExpire, std::string cvv2);
  void setACHInformation(std::string routingNum, std::string accountNum, std::string accountType);
  void setACHInformation(std::string routingNum, std::string accountNum, std::string accountType, std::string docType);
  void addLevel2Information(std::map<std::string, std::string> params);
  void addLineItem(std::map<std::string, std::string> params);
  void setRebillingInformation(std::string rebAmount, std::string rebFirstDate, std::string rebExpr, std::string rebCycles);
  void updateRebillingInformation(std::string rebillID, std::string rebNextDate, std::string rebExpr, std::string rebCycles,
    std::string rebAmount, std::string rebNextAmount);
  void updateRebillPaymentInformation(std::string templateID);
  void cancelRebilling(std::string rebillID);
  void getRebillStatus(std::string rebillID);

  void getTransactionReport(std::string reportStart, std::string reportEnd, std::string subaccountsSearched);
  void getTransactionReport(std::string reportStart, std::string reportEnd, std::string subaccountsSearched, std::string doNotEscape);
  void getTransactionReport(std::string reportStart, std::string reportEnd, std::string subaccountsSearched,
    std::string doNotEscape, std::string errors);
  void getTransactionSettledReport(std::string reportStart, std::string reportEnd, std::string subaccountsSearched);
  void getTransactionSettledReport(std::string reportStart, std::string reportEnd, std::string subaccountsSearched, std::string doNotEscape);
  void getTransactionSettledReport(std::string reportStart, std::string reportEnd, std::string subaccountsSearched,
    std::string doNotEscape, std::string errors);
  void getSingleTransQuery(std::string reportStart, std::string reportEnd);
  void getSingleTransQuery(std::string reportStart, std::string reportEnd, std::string errors);
    void getSingleTransQuery(std::string transactionID, std::string reportStart, std::string reportEnd, std::string errors);
  void queryByTransactionId(std::string transID);
  void queryByPaymentType(std::string payType);
  void queryByTransType(std::string transType);
  void queryByAmount(std::string amount);
  void queryByName1(std::string name1);
  void queryByName2(std::string name2);
  void sale(std::string amount);
  void sale(std::string amount, std::string masterId);
  void auth(std::string amount);
  void auth(std::string amount, std::string masterId);
  void refund(std::string masterID);
  void refund(std::string masterID, std::string amount);
  void update(std::string masterID);
  void update(std::string masterID, std::string amount);
  void voidTransaction(std::string masterID);
  void capture(std::string masterID);
  void capture(std::string masterID, std::string amount);
  void swipe(std::string trackData);

  void setCustomId1(std::string customId1);
  void setCustomId2(std::string customId2);
  void setInvoiceId(std::string invoiceId);
  void setOrderId(std::string orderId);
  void setAmountTip(std::string amountTip);
  void setAmountTax(std::string amountTax);
  void setAmountFood(std::string amountFood);
  void setAmountMisc(std::string amountMisc);
  void setMemo(std::string memo);
  void setPhone(std::string phone);
  void setEmail(std::string email);

  std::string generateTps(std::string message, std::string hashType);
  void calcTps();
  void calcRebillTps();
  void calcReportTps();

  std::string setCardTypes();
  std::string setReceiptTpsString();
  std::string calcURLTps(std::string);
  std::string setReceiptURL();
  std::string encodeURL(std::string);
  std::string urlDecode(std::string);
  std::string addStringProtectedStatus(std::string);
  std::string addDefProtectedStatus(std::string);
  std::string setBp10emuTpsString();
  std::string setShpfTpsString();
  std::string calcURLResponse(); 
  std::string generateURL(std::string merchantName, std::string returnURL, std::string transactionType, std::string acceptDiscover, std::string acceptAmex, std::string amount, std::string protectAmount , std::string paymentTemplate = "mobileform01", std::string receiptTemplate = "mobileresult01", std::string receiptTempRemoteURL = "", std::string rebilling = "No", std::string rebProtect = "Yes", std::string rebAmount = "", std::string rebCycles = "", std::string rebStartDate = "", std::string rebFrequency = "", std::string customID1 = "", std::string protectCustomID1 = "No", std::string customID2 = "", std::string protectCustomID2 = "No", std::string tpsHashType = "");
  std::string setHashType(std::string chosenHash);

  void addHeader(const std::string& s);
  size_t rcvHeaders(void *buffer, size_t size, size_t nmemb, void *userp);
  std::vector<std::string> split(const std::string &s, char delim);
  std::vector<std::string> split(const std::string &s, char delim, std::vector<std::string> &elems);
  char* process();
  std::map<std::string, std::string> mapResponsePairs(std::string responseString);
  std::string getResponse();
  char* getResult();
  char* getTransId();
  char* getMessage();
  char* getCvv2();
  char* getAvs();
  char* getMaskedPaymentAccount();
  char* getCardType();
  char* getBank();
  char* getAuthCode();
  char* getRebillId();
  char* getStatus();
  char* getCreationDate();
  char* getNextDate();
  char* getLastDate();
  char* getSchedExpr();
  char* getCyclesRemain();
  char* getRebillAmount();
  char* getNextAmount();

  bool isSuccessfulTransaction();


};