#include <string>
#include <vector>
#include <sstream>
#include <iterator>
#include <Windows.h> 
#include <wininet.h>
#pragma comment(lib, "Wininet")

#pragma once

#include <stdio.h>
#include <tchar.h>

#include "md5.h"
#include "curl.h"
#define ToHex(Y) (Y>='0'&&Y<='9'?Y-'0':Y-'A'+10)


class BluePayPayment {
private:
  // required for every transaction
  std::string accountId;
  char * URL;
  std::string secretKey;
  std::string mode;

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

  // rebill fields
  std::string reportStartDate;
  std::string reportEndDate;
  std::string doNotEscape;
  std::string queryBySettlement;
  std::string queryByHierarchy;
  std::string excludeErrors;

  char *queryResponse;
  std::string response;
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
  BluePayPayment();
  BluePayPayment(std::string, std::string, std::string);

  void setCustomerInformation(std::string name1, std::string name2, std::string addr1, std::string city, std::string state, std::string zip);
  void setCustomerInformation(std::string name1, std::string name2, std::string addr1, std::string addr2, std::string city, std::string state,
    std::string zip);
  void setCustomerInformation(std::string name1, std::string name2, std::string addr1, std::string addr2, std::string city, std::string state,
    std::string zip, std::string country);
  void setCCInformation(std::string cardNum, std::string cardExpire);
  void setCCInformation(std::string cardNum, std::string cardExpire, std::string cvv2);
  void setACHInformation(std::string routingNum, std::string accountNum, std::string accountType);
  void setACHInformation(std::string routingNum, std::string accountNum, std::string accountType, std::string docType);
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
  void voidTransaction(std::string masterID);
  void capture(std::string masterID);
  void capture(std::string masterID, std::string amount);

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

  void calcTps();
  void calcRebillTps();
  void calcReportTps();
  static std::string calcTransNotifyTps(std::string secretKey, std::string transId, std::string transStatus, std::string transType,
    std::string amount, std::string batchId, std::string batchStatus, std::string totalCount, std::string totalAmount,
    std::string batchUploadId, std::string rebillId, std::string rebillAmount, std::string rebillStatus);

  void addHeader(const std::string& s);
  size_t rcvHeaders(void *buffer, size_t size, size_t nmemb, void *userp);
  char* process();
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


};

