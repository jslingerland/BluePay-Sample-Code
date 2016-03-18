require "net/http"
require "net/https"
require "uri"
require "digest/sha2"
require "digest/md5"

# Files
require_relative "api_request"
require_relative "api_response"

class BluePay
  SERVER = "secure.bluepay.com"
  # Make sure this is the correct path to your CA certificates directory
  RootCA = "/"

  def initialize(params = {})
    @ACCOUNT_ID = params[:account_id]
    @SECRET_KEY = params[:secret_key]
    @PARAM_HASH = {'MODE' => params[:mode]}
  end

  # Set up a credit card payment.
  def set_cc_information(params={})
    @PARAM_HASH['PAYMENT_TYPE'] = 'CREDIT'
    @PARAM_HASH['CC_NUM'] = params[:cc_number] || ''
    @PARAM_HASH['CC_EXPIRES'] = params[:cc_expiration] || ''
    @PARAM_HASH['CVCVV2'] = params[:cvv2] || ''
  end

  # Set up an ACH transaction.  Expects:
  # acc_type: C for Checking, S for Savings
  # routing: Bank routing number
  # account: Customer's checking or savings account number
  # doc_type: WEB, TEL, ARC, etc -- see docs.  Optional.
  # REMEMBER: Ach requires some other fields,
  # such as address and phone 
  def set_ach_information(params = {})
    @PARAM_HASH['PAYMENT_TYPE'] = 'ACH'
    @PARAM_HASH['ACH_ROUTING'] = params[:ach_routing]
    @PARAM_HASH['ACH_ACCOUNT'] = params[:ach_account]
    @PARAM_HASH['ACH_ACCOUNT_TYPE'] = params[:ach_account_type]
    @PARAM_HASH['DOC_TYPE'] = params[:doc_type] || ''
  end

  # Set up a sale
  def sale(params = {})
    @PARAM_HASH['TRANSACTION_TYPE'] = 'SALE'
    @PARAM_HASH['AMOUNT'] = params[:amount]
    @PARAM_HASH['RRNO'] = params[:trans_id] || ''
    @api = "bp10emu"
  end

  # Set up an Auth
  def auth(params ={})
    @PARAM_HASH['TRANSACTION_TYPE'] = 'AUTH'
    @PARAM_HASH['AMOUNT'] = params[:amount]
    @PARAM_HASH['RRNO'] = params[:trans_id] || ''
    @api = "bp10emu"
  end
  
  # Capture an Auth
  def capture(trans_id, amount='')
    @PARAM_HASH['TRANSACTION_TYPE'] = 'CAPTURE'
    @PARAM_HASH['AMOUNT'] = amount
    @PARAM_HASH['RRNO'] = trans_id
    @api = "bp10emu"
  end

  # Refund
  def refund(params = {})
    @PARAM_HASH['TRANSACTION_TYPE'] = 'REFUND'
    @PARAM_HASH['RRNO'] = params[:trans_id]
    @PARAM_HASH['AMOUNT'] = params[:amount] || ''
    @api = "bp10emu"
  end

  # Void
  def void(trans_id)
    @PARAM_HASH['TRANSACTION_TYPE'] = 'VOID'
    @PARAM_HASH['AMOUNT'] = ''
    @PARAM_HASH['RRNO'] = trans_id
    @api = "bp10emu"
  end

  # Sets payment information for a swiped credit card transaction
  def swipe(track_data)
    @PARAM_HASH['SWIPE'] = track_data
    #  Regex matchers 
      # track1_and_track2 = /(%B)\d{0,19}\^([\w\s]*)\/([\w\s]*)([\s]*)\^\d{7}\w*\?;\d{0,19}=\d{7}\w*\?/.match(track_data).to_s
      # track2 = /;\d{0,19}=\d{7}\w*\?/.match(track_data).to_s
  end

  # Sets customer information for the transaction
  def set_customer_information(params={})
    @PARAM_HASH['NAME1'] = params[:first_name]
    @PARAM_HASH['NAME2'] = params[:last_name]
    @PARAM_HASH['ADDR1'] = params[:address1]
    @PARAM_HASH['ADDR2'] = params[:address2]
    @PARAM_HASH['CITY'] = params[:city]
    @PARAM_HASH['STATE'] = params[:state]
    @PARAM_HASH['ZIPCODE'] = params[:zip_code]
    @PARAM_HASH['COUNTRY'] = params[:country]
    @PARAM_HASH['PHONE'] = params[:phone]
    @PARAM_HASH['EMAIL'] = params[:email]
  end

  # Set customer Phone
  def phone=(number)
    @PARAM_HASH['PHONE'] = number
  end

  # Set customer E-mail address
  def email=(email)
    @PARAM_HASH['EMAIL'] = email
  end

  # Set MEMO field
  def memo=(memo)
    @PARAM_HASH['COMMENT'] = memo
  end

  # Set CUSTOM_ID field
  def custom_id1=(custom_id1)
    @PARAM_HASH['CUSTOM_ID'] = custom_id1
  end

  # Set CUSTOM_ID2 field
  def custom_id2=(custom_id2)
    @PARAM_HASH['CUSTOM_ID2'] = custom_id2
  end

  # Set INVOICE_ID field
  def invoice_id=(invoice_id)
    @PARAM_HASH['INVOICE_ID'] = invoice_id
  end

  # Set ORDER_ID field
  def order_id=(order_id)
    @PARAM_HASH['ORDER_ID'] = order_id
  end

  # Set AMOUNT_TIP field
  def amount_tip=(amount_tip)
    @PARAM_HASH['AMOUNT_TIP'] = amount_tip
  end

  # Set AMOUNT_TAX field
  def amount_tax=(amount_tax)
    @PARAM_HASH['AMOUNT_TAX'] = amount_tax
  end

  # Set AMOUNT_FOOD field
  def amount_food=(amount_food)
    @PARAM_HASH['AMOUNT_FOOD'] = amount_food
  end

  # Set AMOUNT_MISC field
  def amount_misc=(amount_misc)
    @PARAM_HASH['AMOUNT_MISC'] = amount_misc
  end

  # Set fields for a recurring payment
  def set_recurring_payment(params = {})
    @PARAM_HASH['REBILLING'] = '1'
    @PARAM_HASH['REB_FIRST_DATE'] = params[:reb_first_date]
    @PARAM_HASH['REB_EXPR'] = params[:reb_expr]
    @PARAM_HASH['REB_CYCLES'] = params[:reb_cycles]
    @PARAM_HASH['REB_AMOUNT'] = params[:reb_amount]
  end

  # Set fields to do an update on an existing rebilling cycle
  def update_rebill(params = {})
    @PARAM_HASH['TRANS_TYPE'] = "SET"
    @PARAM_HASH['REBILL_ID'] = params[:rebill_id]
    @PARAM_HASH['NEXT_DATE'] = params[:next_date] || ''
    @PARAM_HASH['REB_EXPR'] = params[:reb_expr] || ''
    @PARAM_HASH['REB_CYCLES'] = params[:reb_cycles] || ''
    @PARAM_HASH['REB_AMOUNT'] = params[:reb_amount] || ''
    @PARAM_HASH['NEXT_AMOUNT'] = params[:next_amount] || ''
    @PARAM_HASH["TEMPLATE_ID"] = params[:template_id] || ''
    @api = "bp20rebadmin"
  end

  # Set fields to cancel an existing rebilling cycle
  def cancel_rebilling_cycle(rebill_id)
    @PARAM_HASH["TRANS_TYPE"] = "SET"
    @PARAM_HASH["STATUS"] = "stopped"
    @PARAM_HASH["REBILL_ID"] = rebill_id
    @api = "bp20rebadmin"
  end

  # Set fields to get the status of an existing rebilling cycle
  def get_rebilling_cycle_status(rebill_id)
    @PARAM_HASH["TRANS_TYPE"] = "GET"
    @PARAM_HASH["REBILL_ID"] = rebill_id
    @api = "bp20rebadmin"
  end

  # Updates an existing rebilling cycle's payment information.   
  def update_rebilling_payment_information(template_id)
    @PARAM_HASH["TEMPLATE_ID"] = template_id
  end

  # Gets a report on all transactions within a specified date range
  def get_transaction_report(params = {})
    @PARAM_HASH["QUERY_BY_SETTLEMENT"] = '0'
    @PARAM_HASH["REPORT_START_DATE"] = params[:report_start_date]
    @PARAM_HASH["REPORT_END_DATE"] = params[:report_end_date]
    @PARAM_HASH["QUERY_BY_HIERARCHY"] = params[:query_by_hierarchy]
    @PARAM_HASH["DO_NOT_ESCAPE"] = params[:do_not_escape] || ''
    @PARAM_HASH["EXCLUDE_ERRORS"] = params[:exclude_errors] || ''
    @api = "bpdailyreport2"
  end

  # Gets a report on all settled transactions within a specified date range
  def get_settled_transaction_report(params = {})
    @PARAM_HASH["QUERY_BY_SETTLEMENT"] = '1'
    @PARAM_HASH["REPORT_START_DATE"] = params[:report_start_date]
    @PARAM_HASH["REPORT_END_DATE"] = params[:report_end_date]
    @PARAM_HASH["QUERY_BY_HIERARCHY"] = params[:query_by_hierarchy]
    @PARAM_HASH["DO_NOT_ESCAPE"] = params[:do_not_escape] || ''
    @PARAM_HASH["EXCLUDE_ERRORS"] = params[:exclude_errors] || ''
    @api = "bpdailyreport2"
  end

  # Gets data on a specific transaction
  def get_single_transaction_query(params = {})
    @PARAM_HASH["REPORT_START_DATE"] = params[:report_start_date]
    @PARAM_HASH["REPORT_END_DATE"] = params[:report_end_date]
    @PARAM_HASH["id"] = params[:transaction_id]
    @PARAM_HASH["EXCLUDE_ERRORS"] = params[:exclude_errors] || ''
    @api = "stq"    
  end

  # Queries by a specific Transaction ID. To be used with get_single_trans_query
  def query_by_transaction_id(trans_id)
    @PARAM_HASH["id"] = trans_id
  end

  # Queries by a specific Payment Type. To be used with get_single_trans_query
  def query_by_payment_type(pay_type)
    @PARAM_HASH["payment_type"] = payment_type
  end

  # Queries by a specific Transaction Type. To be used with get_single_trans_query
  def query_by_trans_type(trans_type)
    @PARAM_HASH["trans_type"] = trans_type
  end

  # Queries by a specific Transaction Amount. To be used with get_single_trans_query
  def query_by_amount(amount)
    @PARAM_HASH["amount"] = amount
  end

  # Queries by a specific First Name. To be used with get_single_trans_query
  def query_by_name1(name1)
    @PARAM_HASH["name1"] = name1
  end

  # Queries by a specific Last Name. To be used with get_single_trans_query
  def query_by_name2(name2) 
    @PARAM_HASH["name2"] = name2
  end

  # Required arguments for generate_url:
  # merchant_name: Merchant name that will be displayed in the payment page.
  # return_url: Link to be displayed on the transacton results page. Usually the merchant's web site home page.
  # transaction_type: SALE/AUTH -- Whether the customer should be charged or only check for enough credit available.
  # accept_discover: Yes/No -- Yes for most US merchants. No for most Canadian merchants.
  # accept_amex: Yes/No -- Has an American Express merchant account been set up?
  # amount: The amount if the merchant is setting the initial amount.
  # protect_amount: Yes/No -- Should the amount be protected from changes by the tamperproof seal?
  # rebilling: Yes/No -- Should a recurring transaction be set up?
  # paymentTemplate: Select one of our payment form template IDs or your own customized template ID. If the customer should not be allowed to change the amount, add a 'D' to the end of the template ID. Example: 'mobileform01D'
      # mobileform01 -- Credit Card Only - White Vertical (mobile capable) 
      # default1v5 -- Credit Card Only - Gray Horizontal 
      # default7v5 -- Credit Card Only - Gray Horizontal Donation
      # default7v5R -- Credit Card Only - Gray Horizontal Donation with Recurring
      # default3v4 -- Credit Card Only - Blue Vertical with card swipe
      # mobileform02 -- Credit Card & ACH - White Vertical (mobile capable)
      # default8v5 -- Credit Card & ACH - Gray Horizontal Donation
      # default8v5R -- Credit Card & ACH - Gray Horizontal Donation with Recurring
      # mobileform03 -- ACH Only - White Vertical (mobile capable)
  # receiptTemplate: Select one of our receipt form template IDs, your own customized template ID, or "remote_url" if you have one.
      # mobileresult01 -- Default without signature line - White Responsive (mobile)
      # defaultres1 -- Default without signature line – Blue
      # V5results -- Default without signature line – Gray
      # V5Iresults -- Default without signature line – White
      # defaultres2 -- Default with signature line – Blue
      # remote_url - Use a remote URL
  # receipt_temp_remote_url: Your remote URL ** Only required if receipt_template = "remote_url".

  # Optional arguments for generate_url:
  # reb_protect: Yes/No -- Should the rebilling fields be protected by the tamperproof seal?
  # reb_amount: Amount that will be charged when a recurring transaction occurs.
  # reb_cycles: Number of times that the recurring transaction should occur. Not set if recurring transactions should continue until canceled.
  # reb_start_date: Date (yyyy-mm-dd) or period (x units) until the first recurring transaction should occur. Possible units are DAY, DAYS, WEEK, WEEKS, MONTH, MONTHS, YEAR or YEARS. (ex. 2016-04-01 or 1 MONTH)
  # reb_frequency: How often the recurring transaction should occur. Format is 'X UNITS'. Possible units are DAY, DAYS, WEEK, WEEKS, MONTH, MONTHS, YEAR or YEARS. (ex. 1 MONTH) 
  # custom_id: A merchant defined custom ID value.
  # protect_custom_id: Yes/No -- Should the Custom ID value be protected from change using the tamperproof seal?
  # custom_id2: A merchant defined custom ID 2 value.
  # protect_custom_id2: Yes/No -- Should the Custom ID 2 value be protected from change using the tamperproof seal?
   
  def generate_url(params={})
    @PARAM_HASH['DBA'] = params[:merchant_name] 
    @PARAM_HASH['RETURN_URL'] = params[:return_url]  
    @PARAM_HASH['TRANSACTION_TYPE'] = params[:transaction_type]  
    @PARAM_HASH['DISCOVER_IMAGE'] = params[:accept_discover].start_with?("y","Y") ? "discvr.gif" : "spacer.gif"
    @PARAM_HASH['AMEX_IMAGE'] = params[:accept_amex].start_with?("y","Y") ? "amex.gif" : "spacer.gif"
    @PARAM_HASH['AMOUNT'] = params[:amount] || '' 
    @PARAM_HASH['PROTECT_AMOUNT'] = params[:protect_amount] || "No" 
    @PARAM_HASH['REBILLING'] = params[:rebilling].start_with?("y","Y") ? "1" : "0"
    @PARAM_HASH['REB_PROTECT'] = params[:reb_protect] || 'Yes' 
    @PARAM_HASH['REB_AMOUNT'] = params[:reb_amount] || '' 
    @PARAM_HASH['REB_CYCLES'] = params[:reb_cycles] || '' 
    @PARAM_HASH['REB_FIRST_DATE'] = params[:reb_start_date] || ''  
    @PARAM_HASH['REB_EXPR'] = params[:reb_frequency] || '' 
    @PARAM_HASH['CUSTOM_ID'] = params[:custom_id] || ''  
    @PARAM_HASH['PROTECT_CUSTOM_ID'] = params[:protect_custom_id] || "No"
    @PARAM_HASH['CUSTOM_ID2'] = params[:custom_id2] || ''  
    @PARAM_HASH['PROTECT_CUSTOM_ID2'] = params[:protect_custom_id2] || "No" 
    @PARAM_HASH['SHPF_FORM_ID'] = params[:payment_template] || "mobileform01"
    @PARAM_HASH['RECEIPT_FORM_ID'] = params[:receipt_template] || "mobileresult01"
    @PARAM_HASH['REMOTE_URL'] = params[:receipt_temp_remote_url] || '' 
    @card_types = set_card_types
    @receipt_tps_def = 'SHPF_ACCOUNT_ID SHPF_FORM_ID RETURN_URL DBA AMEX_IMAGE DISCOVER_IMAGE SHPF_TPS_DEF'
    @receipt_tps_string = set_receipt_tps_string
    @receipt_tamper_proof_seal = calc_url_tps(@receipt_tps_string)
    @receipt_url = set_receipt_url
    @bp10emu_tps_def = add_def_protected_status('MERCHANT APPROVED_URL DECLINED_URL MISSING_URL MODE TRANSACTION_TYPE TPS_DEF')
    @bp10emu_tps_string = set_bp10emu_tps_string
    @bp10emu_tamper_proof_seal = calc_url_tps(@bp10emu_tps_string)
    @shpf_tps_def = add_def_protected_status('SHPF_FORM_ID SHPF_ACCOUNT_ID DBA TAMPER_PROOF_SEAL AMEX_IMAGE DISCOVER_IMAGE TPS_DEF SHPF_TPS_DEF')
    @shpf_tps_string = set_shpf_tps_string
    @shpf_tamper_proof_seal = calc_url_tps(@shpf_tps_string)
    return calc_url_response
  end

  # Sets the types of credit card images to use on the Simple Hosted Payment Form. Must be used with generate_url.
  def set_card_types
    credit_cards = 'vi-mc'
    credit_cards.concat('-di') if @PARAM_HASH['DISCOVER_IMAGE'] == 'discvr.gif'
    credit_cards.concat('-am') if @PARAM_HASH['AMEX_IMAGE'] == 'amex.gif'
    return credit_cards 
  end

  # Sets the receipt Tamperproof Seal string. Must be used with generate_url.
  def set_receipt_tps_string
    [@SECRET_KEY, 
    @ACCOUNT_ID, 
    @PARAM_HASH['RECEIPT_FORM_ID'], 
    @PARAM_HASH['RETURN_URL'], 
    @PARAM_HASH['DBA'], 
    @PARAM_HASH['AMEX_IMAGE'], 
    @PARAM_HASH['DISCOVER_IMAGE'], 
    @receipt_tps_def].join('')
  end

  # Sets the bp10emu string that will be used to create a Tamperproof Seal. Must be used with generate_url.
  def set_bp10emu_tps_string
    bp10emu = [
    @SECRET_KEY,
    @ACCOUNT_ID,
    @receipt_url,
    @receipt_url,
    @receipt_url,
    @PARAM_HASH['MODE'],
    @PARAM_HASH['TRANSACTION_TYPE'],
    @bp10emu_tps_def].join('')
    return add_string_protected_status(bp10emu)
  end

  # Sets the Simple Hosted Payment Form string that will be used to create a Tamperproof Seal. Must be used with generate_url.
  def set_shpf_tps_string 
    shpf = ([@SECRET_KEY,
    @PARAM_HASH['SHPF_FORM_ID'], 
    @ACCOUNT_ID, 
    @PARAM_HASH['DBA'], 
    @bp10emu_tamper_proof_seal, 
    @PARAM_HASH['AMEX_IMAGE'], 
    @PARAM_HASH['DISCOVER_IMAGE'], 
    @bp10emu_tps_def, 
    @shpf_tps_def].join(''))
    return add_string_protected_status(shpf)
  end

  # Sets the receipt url or uses the remote url provided. Must be used with generate_url.
  def set_receipt_url
    if @PARAM_HASH['RECEIPT_FORM_ID']== 'remote_url'
      return @PARAM_HASH['REMOTE_URL']
    else
      return 'https://secure.bluepay.com/interfaces/shpf?SHPF_FORM_ID=' + @PARAM_HASH['RECEIPT_FORM_ID'] + 
      '&SHPF_ACCOUNT_ID=' + ACCOUNT_ID + 
      '&SHPF_TPS_DEF='    + url_encode(@receipt_tps_def) + 
      '&SHPF_TPS='        + url_encode(@receipt_tamper_proof_seal) + 
      '&RETURN_URL='      + url_encode(@PARAM_HASH['RETURN_URL']) + 
      '&DBA='             + url_encode(@PARAM_HASH['DBA']) + 
      '&AMEX_IMAGE='      + url_encode(@PARAM_HASH['AMEX_IMAGE']) + 
      '&DISCOVER_IMAGE='  + url_encode(@PARAM_HASH['DISCOVER_IMAGE'])
    end
  end

  # Adds optional protected keys to a string. Must be used with generate_url.
  def add_def_protected_status(string)
    string.concat(' AMOUNT') if @PARAM_HASH['PROTECT_AMOUNT'] == 'Yes'
    string.concat(' REBILLING REB_CYCLES REB_AMOUNT REB_EXPR REB_FIRST_DATE') if @PARAM_HASH['REB_PROTECT'] == 'Yes'
    string.concat(' CUSTOM_ID') if @PARAM_HASH['PROTECT_CUSTOM_ID'] == 'Yes'
    string.concat(' CUSTOM_ID2') if @PARAM_HASH['PROTECT_CUSTOM_ID2'] == 'Yes'
    return string 
  end
  
  # Adds optional protected values to a string. Must be used with generate_url.
  def add_string_protected_status(string)
    string.concat(@PARAM_HASH['AMOUNT']) if @PARAM_HASH['PROTECT_AMOUNT'] == 'Yes'
    string.concat([@PARAM_HASH['REBILLING'], @PARAM_HASH['REB_CYCLES'], @PARAM_HASH['REB_AMOUNT'], @PARAM_HASH['REB_EXPR'], @PARAM_HASH['REB_FIRST_DATE']].join('')) if @PARAM_HASH['REB_PROTECT'] == 'Yes'
    string.concat(@PARAM_HASH['CUSTOM_ID']) if @PARAM_HASH['PROTECT_CUSTOM_ID'] == 'Yes'
    string.concat(@PARAM_HASH['CUSTOM_ID2']) if @PARAM_HASH['PROTECT_CUSTOM_ID2'] == 'Yes'
    return string 
  end

  # Encodes a string into a URL. Must be used with generate_url.
  def url_encode(string) 
    encoded_string = ''
    string.each_char do |char|
      char = ("%%%02X" % char.ord) if char.match(/[A-Za-z0-9]/) == nil
      encoded_string << char
    end
   return encoded_string
  end

  # Generates a Tamperproof Seal for a url. Must be used with generate_url.
  def calc_url_tps(tps_type)
    Digest::MD5.hexdigest(tps_type)
  end

  # Generates the final url for the Simple Hosted Payment Form. Must be used with generate_url.
  def calc_url_response
    'https://secure.bluepay.com/interfaces/shpf?'                                     +
    'SHPF_FORM_ID='       .concat(url_encode    (@PARAM_HASH['SHPF_FORM_ID'])       ) +
    '&SHPF_ACCOUNT_ID='   .concat(url_encode    (@ACCOUNT_ID)                       ) +
    '&SHPF_TPS_DEF='      .concat(url_encode    (@shpf_tps_def)                     ) +
    '&SHPF_TPS='          .concat(url_encode    (@shpf_tamper_proof_seal)           ) +
    '&MODE='              .concat(url_encode    (@PARAM_HASH['MODE'])               ) +
    '&TRANSACTION_TYPE='  .concat(url_encode    (@PARAM_HASH['TRANSACTION_TYPE'])   ) +
    '&DBA='               .concat(url_encode    (@PARAM_HASH['DBA'])                ) +
    '&AMOUNT='            .concat(url_encode    (@PARAM_HASH['AMOUNT'])             ) +
    '&TAMPER_PROOF_SEAL=' .concat(url_encode    (@bp10emu_tamper_proof_seal)        ) +
    '&CUSTOM_ID='         .concat(url_encode    (@PARAM_HASH['CUSTOM_ID'])          ) +
    '&CUSTOM_ID2='        .concat(url_encode    (@PARAM_HASH['CUSTOM_ID2'])         ) +
    '&REBILLING='         .concat(url_encode    (@PARAM_HASH['REBILLING'])          ) +
    '&REB_CYCLES='        .concat(url_encode    (@PARAM_HASH['REB_CYCLES'])         ) +
    '&REB_AMOUNT='        .concat(url_encode    (@PARAM_HASH['REB_AMOUNT'])         ) +
    '&REB_EXPR='          .concat(url_encode    (@PARAM_HASH['REB_EXPR'])           ) +
    '&REB_FIRST_DATE='    .concat(url_encode    (@PARAM_HASH['REB_FIRST_DATE'])     ) +
    '&AMEX_IMAGE='        .concat(url_encode    (@PARAM_HASH['AMEX_IMAGE'])         ) +
    '&DISCOVER_IMAGE='    .concat(url_encode    (@PARAM_HASH['DISCOVER_IMAGE'])     ) +
    '&REDIRECT_URL='      .concat(url_encode    (@receipt_url)                      ) +
    '&TPS_DEF='           .concat(url_encode    (@bp10emu_tps_def)                  ) +
    '&CARD_TYPES='        .concat(url_encode    (@card_types)                       )
  end
end
