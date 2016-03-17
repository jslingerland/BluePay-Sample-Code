/**
 * BluePayPayment is an interface to Bluepay's payment gateway. Included are functions to call
 * numerous BluePay APIs for doing transactions, getting report data, etc.                           
 */

package bluepay;

import org.apache.http.HttpResponse;
import org.apache.http.NameValuePair;
import org.apache.http.client.ClientProtocolException;
import org.apache.http.client.HttpClient;
import org.apache.http.client.entity.UrlEncodedFormEntity;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.client.utils.URLEncodedUtils;
import org.apache.http.impl.client.HttpClientBuilder;
import org.apache.http.message.BasicNameValuePair;

import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;
import java.io.*;

import java.nio.charset.Charset;

public class BluePay
{
  // required parameters
  private String BP_URL = "";
  private String BP_MERCHANT = "";
  private String BP_SECRET_KEY = "";
  private String BP_MODE = "";

  // required for sale, auth
  private String TRANSACTION_TYPE = "";
  private String PAYMENT_TYPE = "";
  private String CARD_NUM = "";
  private String CVCVV2 = "";
  private String CARD_EXPIRE = "";
  private String ACH_ROUTING = "";
  private String ACH_ACCOUNT = "";
  private String ACH_ACCOUNT_TYPE = "";
  private String DOC_TYPE = "";
  private String AMOUNT = "";
  private String AMOUNT_TAX = "";
  private String AMOUNT_TIP = "";
  private String AMOUNT_FOOD = "";
  private String AMOUNT_MISC = "";
  private String NAME1 = "";
  private String NAME2 = "";
  private String ADDR1 = "";
  private String ADDR2 = "";
  private String CITY = "";
  private String STATE = "";
  private String ZIP = "";
  private String COUNTRY = "";
  private String PHONE = "";
  private String EMAIL = "";
  private String SWIPE = "";
  
  // optional parameters
  private String MEMO = "";
  private String CUSTOM_ID1 = "";
  private String CUSTOM_ID2 = "";
  private String ORDER_ID = "";
  private String INVOICE_ID = "";
  
  // rebilling parameters
  private String REBILLING = "0";
  private String REB_FIRST_DATE = "";
  private String REB_EXPR = "";
  private String REB_CYCLES = "";
  private String REB_AMOUNT = "";
  private String NEXT_DATE = "";
  private String NEXT_AMOUNT = "";
  private String REBILL_STATUS = "";
  private String REBILL_ID = "";
  private String TEMPLATE_ID = "";
  
  // reporting parameters
  private String REPORT_START = "";
  private String REPORT_END = "";
  private String DO_NOT_ESCAPE = "";
  private String QUERY_BY_SETTLEMENT = "";
  private String QUERY_BY_HIERARCHY = "";
  private String EXCLUDE_ERRORS = "";
  
  // private String MASTER_ID = "";
  private String RRNO = "";
  private String ID = "";
  private String API = "";
  
  private HashMap<String, String> response = new HashMap<String, String>();

  /**
   * Sole constructor.  Requires merchant credentials.
   *
   * @param merchant A string containing the merchant's Account ID.  A 12-digit numeral.
   * @param secretKey A string containing the merchant's Secret Key.  32 characters, alphanumeric.
   * @param mode A string indicating the desired processing mode, "TEST" or "LIVE"
   *
   */

  public BluePay(String merchant, String secretKey, String mode)
  {
    BP_MERCHANT = merchant;
    BP_SECRET_KEY = secretKey;
    BP_MODE = mode;
  }

  /**
   * Sets up object to process a SALE.  A SALE both authorizes the card and captures the funds in one step.
   * 
   * In general, a SALE is the correct transaction type to use.  Use AUTH only if you have special needs.
   *
   * @param amount A string containing the amount of the transaction, e.g. "10.00"
   * @param transactionID - the transaction id of a previous sale.  Uses the customer and payment information of previous transaction.    
   *
   */
  public void sale(HashMap<String, String> params) {
    TRANSACTION_TYPE = "SALE";
    AMOUNT = params.get("amount");
    API = "bp10emu";
    if (params.containsKey("transactionID")) {
      RRNO = params.get("transactionID");
    }
  }
  
  /**
   * Sets up the object to process an AUTH.  An auth authorizes payment and garuntees the funds for later 
   * CAPTURE, but it does not transfer funds.  You must perform a CAPTURE or use Autocap.
   * 
   * @param amount A string containing the amount of the transaction, e.g. "10.00"
   * @param transactionID An optional string containing the 12-digit transaction ID of the transaction to run a sale against.
   *
   */

  public void auth(HashMap<String, String> params) {
    TRANSACTION_TYPE = "AUTH";
    API = "bp10emu";
    AMOUNT = params.get("amount");
    if (params.containsKey("transactionID")) {
      RRNO = params.get("transactionID");
    }
  }
  
  /**
   * Sets up the object to perform a REFUND.  The actual transaction performed will depend on the
   * original transaction status, especially whether it's batch has settled or not.
   *
   * @param transactionID A string containing the 12-digit transaction ID of the transaction to refund.
   * @param amount  An optional string containing the amount to refund.  By default, the entire original transaction is refunded.
   * 
   */
  public void refund(HashMap<String, String> params) {  
    TRANSACTION_TYPE = "REFUND";
    RRNO = params.get("transactionID");
    AMOUNT = params.get("amount");
    API = "bp10emu";
  }
  
  /**
   * Sets up the object to perform a VOID. 
   *
   * @param transactionID A string containing the 12-digit transaction ID of the transaction to refund.
   * 
   */
  public void voidTransaction(String transactionID) {
  	TRANSACTION_TYPE = "VOID";
    API = "bp10emu";
    AMOUNT = "";
    RRNO = transactionID;
  }
   
  /**
   * Sets up the object to CAPTURE a previous AUTH.  
   *
   * @param transID A string containing the 12-digit transaction ID of the AUTH to CAPTURE.
   * @param amount  An optional string containing the amount to capture.  By default, the entire original amount is captured.
   *
   */
  public void capture(HashMap<String, String> params) {
    API = "bp10emu";
    TRANSACTION_TYPE = "CAPTURE";
    AMOUNT = params.get("amount");
    RRNO = params.get("transactionID");
  }

  /**
   * Sets the credit card values.  Required for SALE and AUTH.
   *
   * @param cardnum A string containing the Credit card number, all digits -- do not include spaces or dashes.  
   * @param expire A string containing the card's expiration date in MMYY format.
   * @param cvv2 A (sometimes) optional string containing the Card Verification Value -- the 3 digit number printed on the back of most cards.  Whether it is in fact optional depends on your credit card processing network's requirements.
   *
   */
  
  public void setCCInformation(HashMap<String, String> params) {
    PAYMENT_TYPE = "CREDIT";
    CARD_NUM = params.get("cardNumber");
    CARD_EXPIRE = params.get("expirationDate");
    CVCVV2 = params.get("cvv2");
  }

  /**
   * Sets the credit card values for a swipe transaction using a card reader
   *
   * @param trackData A string containing both track 1 and track 2 date directly from the credit card reader.  
   */
  
  public void swipe(String trackData) {
    SWIPE = trackData;
  }

  
  /**
   * Sets the ACH values.  Required for SALE and AUTH.
   *
   * @param routingNum A string containing the routing number (9 digits). Make sure to include any leading zeros.
   * @param accountNum A string containing the account number. Make sure to include any leading zeros.
   * @param accountType Account type of ACH account. Checking ('C'), Savings ('S').
   * @param docType Documentation type of transaction. May be 'PPD', 'CCD', 'TEL', 'WEB', or 'ARC'.  Defaults to 'WEB' if not set.
   *
   */
  
  public void setACHInformation(HashMap<String, String> params) {
    PAYMENT_TYPE = "ACH";
    ACH_ROUTING = params.get("routingNum");
    ACH_ACCOUNT = params.get("accountNum");
    ACH_ACCOUNT_TYPE = params.get("accountType");
    DOC_TYPE = params.get("docType");
  }

  /**
   * Sets the customer information and billing address.  While this is technically optional, it is highly recommended.  Some 
   * payment processors may require this information.
   *
   * @param firstName A string containing the first name
   * @param lastName A string containing the last name
   * @param address1 A string containing the first line of the street address
   * @param address2 A string containing the second line of a street or unit number.
   * @param city A string containing the billing city
   * @param state A string containing the billing state, province, or regional equivalent
   * @param zip A string containing the postal code
   * @param country
   * @param phone
   * @param email
   *
   */

  public void setCustomerInformation(HashMap<String, String> params) {
    NAME1 = params.get("firstName");
    NAME2 = params.get("lastName");
    ADDR1 = params.get("address1");
    ADDR2 = params.get("address2");
    CITY = params.get("city");
    STATE = params.get("state");
    ZIP = params.get("zip");
    COUNTRY = params.get("country");
    PHONE = params.get("phone");
    EMAIL = params.get("email");
  }

  /**
   * Adds a custom ID1 to a transaction.
   *
   * @param customID1 A string containing an optional custom ID1.
   *
   */
  public void setCustomID1(String customID1) {
    CUSTOM_ID1 = customID1;
  }
  /**
   * Adds a custom ID2 to a transaction.
   *
   * @param customID2 A string containing an optional custom ID2.
   *
   */
  public void setCustomID2(String customID2) {
    CUSTOM_ID2 = customID2;
  }
  /**
   * Adds an order ID to a transaction.
   *
   * @param orderID A string containing an optional order ID.
   *
   */
  public void setOrderID(String orderID) {
    ORDER_ID = orderID;
  }
  /**
   * Adds an invoice ID to a transaction.
   *
   * @param invoiceID A string containing an optional invoice ID.
   *
   */
  public void setInvoiceID(String invoiceID) {
    INVOICE_ID = invoiceID;
  }
  /**
   * Adds a tax to a transaction.
   *
   * @param taxAmount A string containing an optional tax amount.
   *
   */
  public void setAmountTax(String taxAmount) {
    AMOUNT_TAX = taxAmount;
  }
  /**
   * Adds a tip to a transaction.
   *
   * @param tipAmount A string containing an optional tip amount.
   *
   */
  public void setAmountTip(String tipAmount) {
    AMOUNT_TIP = tipAmount;
  }
  
  /**
   * Adds a food amount to a transaction.
   *
   * @param foodAmount A string containing an optional food amount.
   *
   */
  public void setAmountFood(String foodAmount) {
    AMOUNT_FOOD = foodAmount;
  }
  
  /**
   * Adds Amount Misc to a transaction.
   *
   * @param miscAmount A string containing an optional misc amount.
   *
   */  
  public void setAmountMisc(String miscAmount) {
    AMOUNT_MISC = miscAmount;
  }

  /**
   * Adds a comment to a transaction.
   *
   * @param comment A string containing an optional comment.
   *
   */
  public void setMemo(String memo) {
    MEMO = memo;
  }

  /** 
   * Sets the customer's phone number.
   *
   * @param phonenum A string containing the phone number.  It should contain digits only, no punctuation.
   *
   */
  public void setPhone(String phonenum) {
    PHONE = phonenum;
  }

  /**
   * Sets the customer's email address.  Required if you expect them to get an email receipt from Bluepay.
   *
   * @param email A string containing the email address.  
   *
   */
  public void setEmail(String email) {
    EMAIL = email;
  }
  
  /**
   * Adds rebilling to an AUTH or SALE.  
   *
   * @param amount A string containing the amount to rebill.
   * @param firstDate  A string containing the first rebilling date; can contain either a date in ISO format or a date expression such as "1 MONTH" to begin rebilling 1 month from now.
   * @param expr A string containing the Rebilling Expression; this indicates how often to rebill.  E.g. "1 MONTH" will rebill monthly; "365 DAY" or "1 YEAR" will rebill annually.
   * @param cycles A string containing the number of times to rebill; optional.  Will rebill forever if not set.
   *
   */
  public void setRebillingInformation(HashMap<String, String> params) {
    REBILLING = "1";
    REB_AMOUNT = params.get("amount");
    REB_FIRST_DATE = params.get("firstDate");
    REB_EXPR = params.get("expr");
    REB_CYCLES = params.get("cycles");
  }
    
  /**
   * Updates an existing rebilling cycle.  
   *
   * @param rebillID A 12 digit string containing the rebill ID.
   * @template_id the id of a previous transaction to be used as the payment information for the rebill 
   * @param nextDate  A string containing the next rebilling date.
   * @param expr A string containing the Rebilling Expression; this indicates how often to rebill.  E.g. "1 MONTH" will rebill monthly; "365 DAY" or "1 YEAR" will rebill annually.
   * @param cycles A string containing the number of times to rebill; optional.  Will rebill forever if not set.
   * @param rebillAmount A string containing the amount to charge each time the rebilling is run.
   * @param nextAmount A string containing the *next* amount to charge the customer.
   *
   */  
  public void updateRebill(HashMap<String, String> params) {
	  TRANSACTION_TYPE = "SET";
	  REBILL_ID = params.get("rebillID");
    TEMPLATE_ID = params.get("templateID");
    NEXT_DATE = params.get("nextDate");
    REB_EXPR = params.get("expr");
    REB_CYCLES = params.get("cycles");
    REB_AMOUNT = params.get("rebillAmount");
    NEXT_AMOUNT = params.get("nextAmount"); 
    API = "bp20rebadmin";
  }
  
  /**
   * Cancels an existing rebilling cycle.  
   *
   * @param rebillID A 12 digit string containing the rebill ID.
   *
   */ 
  public void cancelRebill(String rebillID){
	  TRANSACTION_TYPE = "SET";
	  REBILL_STATUS = "stopped";
    REBILL_ID = rebillID;
    API = "bp20rebadmin";
  }
  
  /**
   * Gets a existing rebilling cycle's status.  
   *
   * @param rebillID A 12 digit string containing the rebill ID.
   *
   */ 
  public void getRebillStatus(String rebillID) {
	  TRANSACTION_TYPE = "GET";
	  REBILL_ID = rebillID;
    API = "bp20rebadmin";
  }
  
  /**
   * Gets report of transaction data based upon a start and end date range. 
   *
   * @param reportStart A string containing the date to start the report
   * @param reportEnd A string containing the date to stop the report
   * @param subaccountsSearched Either a 1 or a 0 to indicate whether to search subaccounts as well as the main account
   * @param doNotEscape Either a 1 or a 0 to indicate whether the report should take off quotes around the data
   * @param errors Either a 1 or a 0 to indicate whether the report should exclude errored transactions
   *
   */
  public void getTransactionReport(HashMap<String, String> params) {
	  QUERY_BY_SETTLEMENT = "0";
	  REPORT_START = params.get("reportStart");
	  REPORT_END = params.get("reportEnd");
	  QUERY_BY_HIERARCHY = params.get("subaccountsSearched");
    DO_NOT_ESCAPE = params.get("doNotEscape");
    EXCLUDE_ERRORS = params.get("excludeErrors");
    API = "bpdailyreport2";
  }
  
  /**
   * Gets report of settled transaction data based upon a start and end date range. 
   *
   * @param reportStart A string containing the date to start the report
   * @param reportEnd A string containing the date to stop the report
   * @param subaccountsSearched Either a 1 or a 0 to indicate whether to search subaccounts as well as the main account
   * @param doNotEscape Either a 1 or a 0 to indicate whether the report should take off quotes around the data
   * @param errors Either a 1 or a 0 to indicate whether the report should exclude errored transactions
   * 
   */

  public void getSettledTransactionReport(HashMap<String, String> params) {
	  QUERY_BY_SETTLEMENT = "1";
	  REPORT_START = params.get("reportStart");
	  REPORT_END = params.get("reportEnd");
	  QUERY_BY_HIERARCHY = params.get("subaccountsSearched");
	  DO_NOT_ESCAPE = params.get("doNotEscape");
	  EXCLUDE_ERRORS = params.get("excludeErrors");
    API = "bpdailyreport2";
  }
    
  /**
   * Gets information about a specific transaction
   * @param transactionID - the id number of the transaction to be queried 
   * @param reportStart A string containing the date to start the report; required for tamper proof seal
   * @param reportEnd A string containing the date to stop the report;  required for tamper proof seal
   * @param errors Either a 1 or a 0 to indicate whether the query should exclude errored transactions
   * 
   */
  public void getSingleTransQuery(HashMap<String, String> params) {
    ID = params.get("transactionID");
    REPORT_START = params.get("reportStart");
    REPORT_END = params.get("reportEnd");
    EXCLUDE_ERRORS = params.get("excludeErrors");
    API = "stq";
  }
  
  /**
   * Queries transactions by a specific Payment Type. Must be used with getSingleTransQuery
   *
   * @param transID A string containing the Payment Type to query.
   * 
   */
  public void queryByPaymentType(String payType) {
	  PAYMENT_TYPE = payType;
  }

  /**
   * Queries transactions by a specific Transaction Type. Must be used with getSingleTransQuery
   *
   * @param transID A string containing the Transaction Type to query.
   * 
   */
  public void queryBytransType(String transType) {
	  TRANSACTION_TYPE = transType;
  }

  /**
   * Queries transactions by a specific Transaction Amount. Must be used with getSingleTransQuery
   *
   * @param transID A string containing the Transaction Amount to query.
   * 
   */
  public void queryByAmount(String amount) {
	  AMOUNT = amount;
  }

  /**
   * Queries transactions by a specific First Name. Must be used with getSingleTransQuery
   *
   * @param transID A string containing the First Name to query.
   * 
   */
  public void queryByName1(String name1) {
	  NAME1 = name1;
  }

  /**
   * Queries transactions by a specific Last Name. Must be used with getSingleTransQuery
   *
   * @param transID A string containing the Last Name to query.
   * 
   */
  public void queryByName2(String name2) {
	  NAME2 = name2;
  }

  /**
   * Calculates a hex MD5 based on input.
   *
   * @param message String to calculate MD5 of.
   *
   */
  private String md5(String message) throws java.security.NoSuchAlgorithmException
  {
    MessageDigest md5 = null;
    try {
      md5 = MessageDigest.getInstance("MD5");
    }
    catch (java.security.NoSuchAlgorithmException ex) {
      ex.printStackTrace();
      throw ex;
    }
    byte[] dig = md5.digest((byte[]) message.getBytes());
    StringBuffer code = new StringBuffer();
    for (int i = 0; i < dig.length; ++i)
    {
      code.append(Integer.toHexString(0x0100 + (dig[i] & 0x00FF)).substring(1));
    }
    return code.toString();
  }

  /**
   * Calculates the TAMPER_PROOF_SEAL string to send with each transaction
   *
   * @return tps The Tamper Proof Seal
   *
   */
  private String calcTPS() throws java.security.NoSuchAlgorithmException {
    // System.out.println("RRNO:");
    // System.out.println(RRNO);
    String tps = BP_SECRET_KEY + BP_MERCHANT + TRANSACTION_TYPE + AMOUNT + REBILLING 
                 + REB_FIRST_DATE + REB_EXPR + REB_CYCLES + REB_AMOUNT + RRNO + BP_MODE;
    return md5(tps);
  }
  
  /**
   * Calculates the TAMPER_PROOF_SEAL string to send with each transaction
   *
   * @return tps The Tamper Proof Seal
   *
   */
  private String calcRebillTPS() throws java.security.NoSuchAlgorithmException {
    String tps = BP_SECRET_KEY + BP_MERCHANT + TRANSACTION_TYPE + REBILL_ID;
    return md5(tps);
  }
  
  /**
   * Calculates the TAMPER_PROOF_SEAL string to send with each transaction
   *
   * @return tps The Tamper Proof Seal
   *
   */
  private String calcReportTPS() throws java.security.NoSuchAlgorithmException {
    String tps = BP_SECRET_KEY + BP_MERCHANT + REPORT_START + REPORT_END;
    return md5(tps);
  }
  
  /**
   * Calculates the TAMPER_PROOF_SEAL string to send with each transaction
   *
   * @return tps The Tamper Proof Seal
   *
   */
  public String calcTransNotifyTPS(String secretKey, String transID, String transStatus, String transType, 
		  String amount, String batchID, String batchStatus, String totalCount, String totalAmount, 
		  String batchUploadID, String rebillID, String rebillAmount, String rebillStatus) 
  throws java.security.NoSuchAlgorithmException {
  	String tps = secretKey + transID + transStatus + transType + amount + batchID + batchStatus + 
  	totalCount + totalAmount + batchUploadID + rebillID + rebillAmount + rebillStatus;
  	return md5(tps);
  }

  /**
   * Processes a payment.
 * @throws IOException 
 * @throws ClientProtocolException 
 * @throws NoSuchAlgorithmException 
   * 
   */
  public HashMap<String,String> process() throws ClientProtocolException, IOException, NoSuchAlgorithmException {
    List <NameValuePair> nameValuePairs = new ArrayList <NameValuePair>();
	  nameValuePairs.add(new BasicNameValuePair("MODE", BP_MODE));	
	  if (API.equals("bpdailyreport2")) {
  		  BP_URL = "https://secure.bluepay.com/interfaces/bpdailyreport2";
  		  nameValuePairs.add(new BasicNameValuePair("ACCOUNT_ID", BP_MERCHANT));
  		  nameValuePairs.add(new BasicNameValuePair("TAMPER_PROOF_SEAL", calcReportTPS()));
  		  nameValuePairs.add(new BasicNameValuePair("REPORT_START_DATE", REPORT_START));
  		  nameValuePairs.add(new BasicNameValuePair("REPORT_END_DATE", REPORT_END));
  		  nameValuePairs.add(new BasicNameValuePair("DO_NOT_ESCAPE", DO_NOT_ESCAPE));
  		  nameValuePairs.add(new BasicNameValuePair("QUERY_BY_SETTLEMENT", QUERY_BY_SETTLEMENT));
  		  nameValuePairs.add(new BasicNameValuePair("QUERY_BY_HIERARCHY", QUERY_BY_HIERARCHY));
  		  nameValuePairs.add(new BasicNameValuePair("EXCLUDE_ERRORS", EXCLUDE_ERRORS));
	  } else if (API.equals("stq")) {
        BP_URL = "https://secure.bluepay.com/interfaces/stq";
  		  nameValuePairs.add(new BasicNameValuePair("ACCOUNT_ID", BP_MERCHANT));
  		  nameValuePairs.add(new BasicNameValuePair("TAMPER_PROOF_SEAL", calcReportTPS()));
  		  nameValuePairs.add(new BasicNameValuePair("REPORT_START_DATE", REPORT_START));
  		  nameValuePairs.add(new BasicNameValuePair("REPORT_END_DATE", REPORT_END));
  		  nameValuePairs.add(new BasicNameValuePair("EXCLUDE_ERRORS", EXCLUDE_ERRORS));
  		  nameValuePairs.add(new BasicNameValuePair("id", ID));
	  } else if(API.equals("bp10emu")) {
    	  BP_URL = "https://secure.bluepay.com/interfaces/bp10emu";
        nameValuePairs.add(new BasicNameValuePair("MERCHANT", BP_MERCHANT));
  	    nameValuePairs.add(new BasicNameValuePair("TAMPER_PROOF_SEAL", calcTPS()));
        nameValuePairs.add(new BasicNameValuePair("PAYMENT_TYPE", PAYMENT_TYPE));
        nameValuePairs.add(new BasicNameValuePair("TRANSACTION_TYPE", TRANSACTION_TYPE));
        nameValuePairs.add(new BasicNameValuePair("AMOUNT", AMOUNT));
        nameValuePairs.add(new BasicNameValuePair("NAME1", NAME1));
        nameValuePairs.add(new BasicNameValuePair("NAME2", NAME2));
        nameValuePairs.add(new BasicNameValuePair("ADDR1", ADDR1));
        nameValuePairs.add(new BasicNameValuePair("ADDR2", ADDR2));
        nameValuePairs.add(new BasicNameValuePair("CITY", CITY));
        nameValuePairs.add(new BasicNameValuePair("STATE", STATE));
        nameValuePairs.add(new BasicNameValuePair("ZIPCODE", ZIP));
        nameValuePairs.add(new BasicNameValuePair("PHONE", PHONE));
        nameValuePairs.add(new BasicNameValuePair("EMAIL", EMAIL));
        nameValuePairs.add(new BasicNameValuePair("COUNTRY", COUNTRY));
        nameValuePairs.add(new BasicNameValuePair("RRNO", RRNO));
        nameValuePairs.add(new BasicNameValuePair("CUSTOM_ID", CUSTOM_ID1));
        nameValuePairs.add(new BasicNameValuePair("CUSTOM_ID2", CUSTOM_ID2));
        nameValuePairs.add(new BasicNameValuePair("INVOICE_ID", INVOICE_ID));
        nameValuePairs.add(new BasicNameValuePair("ORDER_ID", ORDER_ID));
        nameValuePairs.add(new BasicNameValuePair("COMMENT", MEMO));
        nameValuePairs.add(new BasicNameValuePair("AMOUNT_TIP", AMOUNT_TIP));
        nameValuePairs.add(new BasicNameValuePair("AMOUNT_TAX", AMOUNT_TAX));
        nameValuePairs.add(new BasicNameValuePair("AMOUNT_FOOD", AMOUNT_FOOD));
        nameValuePairs.add(new BasicNameValuePair("AMOUNT_MISC", AMOUNT_MISC));
        nameValuePairs.add(new BasicNameValuePair("REBILLING", REBILLING));
        nameValuePairs.add(new BasicNameValuePair("REB_FIRST_DATE", REB_FIRST_DATE));
        nameValuePairs.add(new BasicNameValuePair("REB_EXPR", REB_EXPR));
        nameValuePairs.add(new BasicNameValuePair("REB_CYCLES", REB_CYCLES));
        nameValuePairs.add(new BasicNameValuePair("REB_AMOUNT", REB_AMOUNT));
        nameValuePairs.add(new BasicNameValuePair("SWIPE", SWIPE));
        if (PAYMENT_TYPE.equals("CREDIT")) {
        	  nameValuePairs.add(new BasicNameValuePair("CC_NUM", CARD_NUM));  
        	  nameValuePairs.add(new BasicNameValuePair("CC_EXPIRES", CARD_EXPIRE));
        	  nameValuePairs.add(new BasicNameValuePair("CVCVV2", CVCVV2));
        } else if (PAYMENT_TYPE.equals("ACH")) {
      	  nameValuePairs.add(new BasicNameValuePair("ACH_ROUTING", ACH_ROUTING));
      	  nameValuePairs.add(new BasicNameValuePair("ACH_ACCOUNT", ACH_ACCOUNT));
      	  nameValuePairs.add(new BasicNameValuePair("ACH_ACCOUNT_TYPE", ACH_ACCOUNT_TYPE));
      	  nameValuePairs.add(new BasicNameValuePair("DOC_TYPE", DOC_TYPE));
        }
    } else if (API.equals("bp20rebadmin")) {
    	  BP_URL = "https://secure.bluepay.com/interfaces/bp20rebadmin";
    	  nameValuePairs.add(new BasicNameValuePair("ACCOUNT_ID", BP_MERCHANT));
    	  nameValuePairs.add(new BasicNameValuePair("TAMPER_PROOF_SEAL", calcRebillTPS()));
    	  nameValuePairs.add(new BasicNameValuePair("TRANS_TYPE", TRANSACTION_TYPE));
    	  nameValuePairs.add(new BasicNameValuePair("REBILL_ID", REBILL_ID));
    	  nameValuePairs.add(new BasicNameValuePair("TEMPLATE_ID", TEMPLATE_ID));
    	  nameValuePairs.add(new BasicNameValuePair("NEXT_DATE", NEXT_DATE));
    	  nameValuePairs.add(new BasicNameValuePair("REB_EXPR", REB_EXPR));
    	  nameValuePairs.add(new BasicNameValuePair("REB_CYCLES", REB_CYCLES));
    	  nameValuePairs.add(new BasicNameValuePair("REB_AMOUNT", REB_AMOUNT));
    	  nameValuePairs.add(new BasicNameValuePair("NEXT_AMOUNT", NEXT_AMOUNT));
    	  nameValuePairs.add(new BasicNameValuePair("STATUS", REBILL_STATUS));
    }
    HttpClient httpclient = HttpClientBuilder.create().build();
    HttpPost httpost = new HttpPost(BP_URL);
    httpost.setEntity(new UrlEncodedFormEntity(nameValuePairs));
    HttpResponse responseString = httpclient.execute(httpost);
    if (BP_URL.equals("https://secure.bluepay.com/interfaces/bp10emu")) {
        String queryString = responseString.getFirstHeader("location").getValue();
        //BufferedReader rd = new BufferedReader(new InputStreamReader(responseString.getEntity().getContent()));
        //String line = "";
        Map<String, String> map = getQueryMap(queryString);  
        Set<String> keys = map.keySet();  
        for (String key : keys) {  
    	   response.put(key, map.get(key));
        }
        return response;
    } else {
    	  BufferedReader rd = new BufferedReader(new InputStreamReader(responseString.getEntity().getContent()));
        String line = "";
        while ((line = rd.readLine()) != null) {
          List<NameValuePair> params = URLEncodedUtils.parse(line, Charset.defaultCharset());
          for (NameValuePair nameValuePair : params) {
            response.put(nameValuePair.getName(), nameValuePair.getValue());
          }
        }
        return response;
    }
  }
  
  public HashMap<String, String> getResponse() {
	  return response;
  }
  
  public static Map<String, String> getQueryMap(String query)  {  
	  query = query.split("\\?")[1];
    String[] params = query.split("&"); 
    Map<String, String> map = new HashMap<String, String>();  
    for (String param : params) {  
  	  try {
  		  String name = param.split("=")[0];  
  		  String value = param.split("=")[1];  
  		  map.put(name, value);
  	  }
  	  catch (ArrayIndexOutOfBoundsException e) {
  		  String name = param.split("=")[0];  
  		  String value = "";
  		  map.put(name, value);
  	  }
    }  
    return map;  
  } 

  /** Returns a single character indicating the result.
   *
   * @return '1' = Approved, '0' = Declined, 'E' = Error
   *
   */
  public String getStatus() {
    if(response.containsKey("Result")) {
    	return response.get("Result");
    } else {
      return null;
    }
  }

  /**
   * Returns the transaction status.
   *
   * @return true if approved; false otherwise.
   *
   */
  public boolean isApproved()
  {
    if(response.containsKey("Result")) {
      if(response.get("Result").equals("APPROVED")) {
        return true;
      }
    } 
    return false;
  }

  /** 
   * Returns the transaction status.
   *
   * @return true if Declined; false otherwise.
   *
   */
  public boolean isDeclined()
  {
    if(response.containsKey("Result")) {
      if(response.get("Result").equals("DECLINED")) {
        return true;
      }
    } 
    return false;
  }
  
  /**
   * Returns the transaction status.
   *
   * @return true if error; false otherwise.
   *
   */
  public boolean isError() {
    if (response.containsKey("Result")) {
    	if (response.get("Result").equals("E") || response.get("Result").equals("MISSING")) {
          return true;
      }
    } 
    return false;
  }
  
  /**
   * Returns true if the transaction was successful (i.e. was approved and not a duplicate) 
   *
   * @return true if error; false otherwise.
   *
   */
  public boolean isSuccessful()
  {
    if (response.get("Result").equals("APPROVED") && !response.get("MESSAGE").equals("DUPLICATE")) {
      return true;
    } else {
      return false;
    }
  }


  /** 
   * Returns a human-readable transaction status.
   *
   * @return A string containing the status; e.g. "Approved" or "Declined: Hold Card"
   *
   */
  public String getMessage()
  {
    if(response.containsKey("MESSAGE")) {
      return response.get("MESSAGE");
    } else {
      return null;
    }
  }

  /** 
   * Returns the Transaction ID.
   *
   * @return String containing 12-digit ID or null if none.
   *
   */
  public String getTransID()
  {
    if(response.containsKey("RRNO")) {
      return response.get("RRNO");
    } else {
      return null;
    }
  }
  
  /**
   * Returns the rebilling ID.
   *
   * @return String containing 12-digit ID or null if none.
   *
   */
  public String getRebillingID()
  {
    if(response.containsKey("REBID")) {
      return response.get("REBID");
    } else if(response.containsKey("rebill_id")) {
        return response.get("rebill_id");
    } else {
      return null;
    }
  }

  /**
   * Returns the AVS response.
   *
   * @return String containing the AVS result or null if none.
   *
   */
  public String getAVS()
  {
    if(response.containsKey("AVS")) {
      return response.get("AVS");
    } else {
      return null;
    }
  }

  /**
   * Returns the CVV2 response.
   *
   * @return String containing the CVV2 response or null if none.
   *
   */
  public String getCVV2()
  {
    if(response.containsKey("CVV2")) {
      return response.get("CVV2");
    } else {
      return null;
    }
  }
  
  /**
   * Returns the masked payment account of the customer
   *
   * @return String containing the masked payment account or null if none.
   *
   */
  public String getMaskedPaymentAccount()
  {
  	if(response.containsKey("PAYMENT_ACCOUNT")) {
  	  return response.get("PAYMENT_ACCOUNT");
    } else {
      return null;
    }
  }
  
  /**
   * Returns the card type used for the transaction
   *
   * @return String containing the card type or null if none.
   *
   */
  public String getCardType()
  {
  	if(response.containsKey("CARD_TYPE")) {
  	  return response.get("CARD_TYPE");
    } else {
      return null;
    }
  }
  
  /**
   * Returns the customer's bank used for the transaction
   *
   * @return String containing the bank name or null if none.
   *
   */
  public String getBankName()
  {
  	if(response.containsKey("BANK_NAME")) {
  	  return response.get("BANK_NAME");
    } else {
      return null;
    }
  }
  
  /**
   * Returns the authorization code for the transaction
   *
   * @return String containing the auth code or null if none.
   *
   */
  public String getAuthCode()
  {
  	if(response.containsKey("AUTH_CODE")) {
  	  return response.get("AUTH_CODE");
    } else {
      return null;
    }
  }
  
  /**
   * Returns the Rebill Status response.
   *
   * @return String containing the Rebill Status response or null if none.
   *
   */
  public String getRebillStatus()
  {
  	if(response.containsKey("status")) {
  	  return response.get("status");
    } else {
      return null;
    }
  }
  
  /**
   * Returns the Rebill Creation Date response.
   *
   * @return String containing the Rebill Creation Date response or null if none.
   *
   */
  public String getRebillCreationDate()
  {
  	if(response.containsKey("creation_date")) {
  	  return response.get("creation_date");
    } else {
      return null;
    }
  }
  
  /**
   * Returns the Rebill Next Date response.
   *
   * @return String containing the Rebill Next Date response or null if none.
   *
   */
  public String getRebillNextDate()
  {
  	if(response.containsKey("next_date")) {
  	  return response.get("next_date");
    } else {
      return null;
    }
  }
  
  /**
   * Returns the Rebill Status response.
   *
   * @return String containing the Rebill Status response or null if none.
   *
   */
  public String getRebillLastDate()
  {
  	if(response.containsKey("last_date")) {
  	  return response.get("last_date");
    } else {
      return null;
    }
  }
  
  /**
   * Returns the Rebill Schedule Expression response.
   *
   * @return String containing the Rebill Schedule Expression response or null if none.
   *
   */
  public String getRebillSchedExpr()
  {
  	if(response.containsKey("sched_expr")) {
  	  return response.get("sched_expr");
    } else {
      return null;
    }
  }
  
  /**
   * Returns the Rebill Cycles Remaining response.
   *
   * @return String containing the Rebill Cycles Remaining response or null if none.
   *
   */
  public String getRebillCyclesRemain()
  {
  	if(response.containsKey("cycles_remain")) {
  	  return response.get("cycles_remain");
    } else {
      return null;
    }
  }
  
  /**
   * Returns the Rebill Amount response.
   *
   * @return String containing the Rebill Amount response or null if none.
   *
   */
  public String getRebillAmount()
  {
  	if(response.containsKey("reb_amount")) {
  	  return response.get("reb_amount");
    } else {
      return null;
    }
  }
  
  /**
   * Returns the Rebill Next Amount response.
   *
   * @return String containing the Rebill Next Amount response or null if none.
   *
   */
  public String getRebillNextAmount()
  {
  	if(response.containsKey("next_amount")) {
  	  return response.get("next_amount");
    } else {
    return null;
    }
  }

};
