require "net/http"
require "net/https"
require "uri"
require "digest/md5"

# Files
require_relative "api_request"
require_relative "api_response"

class BluePay
  SERVER = "secure.bluepay.com"
  # Make sure this is the correct path to your CA certificates directory
  # For testing purposes, this gem comes with a CA bundle.
  RootCA = "."

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
    # @api = "bp10emu"
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
end
