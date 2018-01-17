# -*- coding: utf-8 -*-
import six
from six.moves import urllib
from six.moves.urllib.request import Request, urlopen
from six.moves.urllib.error import URLError, HTTPError
from six.moves.urllib.parse import urlparse, urlencode, parse_qs
import hashlib
import hmac
import cgi
import os
import re

import sys # PG: added this

class BluePay:

    # Sets all the attributes to default to empty strings if not defined
    
    # Merchant fields
    account_id = ''
    secret_key = '' 
    mode = ''

    # Transaction fields
    trans_type = ''
    payment_type = ''
    amount = ''
    card_number = ''
    cvv2 = ''
    card_expire = ''
    routing_number = ''
    account_number = ''
    account_type = ''
    doc_type = ''
    track_data = ''

    # Customer fields
    name1 = ''
    name2 = ''
    addr1 = ''
    addr2 = ''
    city = ''
    state = ''
    zipcode = ''
    country = ''
    phone = ''
    email = ''

    # Optional fields
    memo = ''
    custom_id1 = ''
    custom_id2 = ''
    invoice_id = ''
    order_id = ''
    amount_tax = ''
    amount_tip = ''
    amount_food = ''
    amount_misc = ''

    # Rebilling fields
    do_rebill = ''
    reb_first_date = ''
    reb_expr = ''
    reb_cycles = ''
    reb_amount = ''
    reb_next_date = ''
    reb_next_amount = ''
    template_id = ''

    # Reporting fields
    report_start_date = ''
    report_end_date = ''
    query_by_settlement = ''
    subaccounts_searched = ''
    do_not_escape = ''
    excludeErrors = ''

    # Generating Simple Hosted Payment Form URL fields
    dba = ''
    return_url = ''
    accept_discover = ''
    accept_amex = ''
    protect_amount = ''
    reb_protect = ''
    protect_custom_id = ''
    protect_custom_id2 = ''
    shpf_form_id = ''
    receipt_form_id = ''
    remote_url = ''
    
    # Response fields
    reb_status = ''
    rebill_id = ''
    rrno = ''
    data = ''

    # Processing fields
    url = ''
    api = ''
    tps_hash_type = 'HMAC_SHA512'

    # Level 2 Processing field
    level2_info = {}

    # Level 3 Processing field
    line_items = []

    # Class constructor. Accepts:
    # accID : Merchant's Account ID
    # secret_key : Merchant's Secret Key
    # mode : Transaction mode of either LIVE or TEST (default)
    def __init__(self, **params):
        self.account_id = params['account_id']
        self.secret_key = params['secret_key']
        self.mode = params['mode']



    # Performs a SALE
    def sale(self, **params):
        """ 
        Declares sales parameters for API Request 
        """
        # self, amount, rrno=None 
        self.trans_type = 'SALE'
        self.amount = params['amount']
        self.api = 'bp10emu'
        # self.rrno = params['rrno']
        if 'transaction_id' in params:
            self.rrno = params['transaction_id']
 
    # Performs an AUTH
    def auth(self, **params):
        """ 
        Send an Auth request to the BluePay gateway
        """
        self.trans_type = 'AUTH'
        self.amount = params['amount']
        self.api = 'bp10emu'
        if 'rrno' in params:
            self.rrno = params['rrno']
    
    # Performs a CAPTURE
    def capture(self, **params):
        """ 
        Send a Capture request to the BluePay gateway 
        """
        self.api = 'bp10emu'
        self.trans_type = 'CAPTURE'
        self.rrno = params['rrno']
        if 'amount' in params:
            self.amount = params['amount']

    # Performs a REFUND
    def refund(self, **params):
        """ 
        Send a Refund request to the BluePay gateway.
        """
        self.api = 'bp10emu'
        self.trans_type = 'REFUND'
        self.rrno = params['transaction_id']
        if 'amount' in params:
            self.amount = params['amount']

    # Performs a VOID
    def void(self, rrno):
        """ 
        Send a Void request to the BluePay gateway.
        """
        self.trans_type = 'VOID'
        self.rrno = rrno
        self.api = 'bp10emu'

    # Passes customer information into the transaction
    def set_customer_information(self, **params):
        self.name1 = params['name1']
        self.name2 = params['name2']
        self.addr1 = params['addr1']
        self.city = params['city']
        self.state = params['state']
        self.zipcode = params['zipcode']
        self.addr2 = params['addr2']
        self.country = params['country']
        
    # Sets payment type. Needed if using ACH tokens
    def set_payment_type(self, pay_type):
        self.payment_type = pay_type


        
    # Passes credit card information into the transaction
    def set_cc_information(self, **params):
        self.payment_type = 'CREDIT'
        if 'card_number' in params:    
            self.card_number = params['card_number']
        if 'card_expire' in params:
            self.card_expire = params['card_expire']
        if 'cvv2' in params:
            self.cvv2 = params['cvv2']
        
    # Sets payment information for a swiped credit card transaction
    def swipe(self, track_data):
        self.track_data = track_data
        
    # Passes ACH information into the transaction
    def set_ach_information(self, **params):
        self.payment_type = 'ACH'
        self.routing_number = params['routing_number']
        self.account_number = params['account_number']
        self.account_type = params['account_type']
        self.doc_type = params['doc_type']
         
    # Adds information required for level 2 processing.
    def add_level2_information(self, **params):
        self.level2_info.update({
            'LV2_ITEM_TAX_RATE' : params.get('tax_rate', ''),
            'LV2_ITEM_GOODS_TAX_RATE' : params.get('goods_tax_rate', ''),
            'LV2_ITEM_GOODS_TAX_AMOUNT' : params.get('goods_tax_amount', ''),
            'LV2_ITEM_SHIPPING_AMOUNT' : params.get('shipping_amount', ''),
            'LV2_ITEM_DISCOUNT_AMOUNT' : params.get('discount_amount', ''),
            'LV2_ITEM_CUST_PO' : params.get('cust_po', ''),
            'LV2_ITEM_GOODS_TAX_ID' : params.get('goods_tax_id', ''),
            'LV2_ITEM_TAX_ID' : params.get('tax_id', ''),
            'LV2_ITEM_CUSTOMER_TAX_ID' : params.get('customer_tax_id', ''),
            'LV2_ITEM_DUTY_AMOUNT' : params.get('duty_amount', ''),
            'LV2_ITEM_SUPPLEMENTAL_DATA' : params.get('supplemental_data', ''),
            'LV2_ITEM_CITY_TAX_RATE' : params.get('city_tax_rate', ''),
            'LV2_ITEM_CITY_TAX_AMOUNT' : params.get('city_tax_amount', ''),
            'LV2_ITEM_COUNTY_TAX_RATE' : params.get('county_tax_rate', ''),
            'LV2_ITEM_COUNTY_TAX_AMOUNT' : params.get('county_tax_amount', ''),
            'LV2_ITEM_STATE_TAX_RATE' : params.get('state_tax_rate', ''),
            'LV2_ITEM_STATE_TAX_AMOUNT' : params.get('state_tax_amount', ''),
            'LV2_ITEM_BUYER_NAME' : params.get('buyer_name', ''),
            'LV2_ITEM_CUSTOMER_REFERENCE' : params.get('customer_reference', ''),
            'LV2_ITEM_CUSTOMER_NUMBER' : params.get('customer_number', ''),
            'LV2_ITEM_SHIP_NAME' : params.get('ship_name', ''),
            'LV2_ITEM_SHIP_ADDR1' : params.get('ship_addr1', ''),
            'LV2_ITEM_SHIP_ADDR2' : params.get('ship_addr2', ''),
            'LV2_ITEM_SHIP_CITY' : params.get('ship_city', ''),
            'LV2_ITEM_SHIP_STATE' : params.get('ship_state', ''),
            'LV2_ITEM_SHIP_ZIP' : params.get('ship_zip', ''),
            'LV2_ITEM_SHIP_COUNTRY' : params.get('ship_country', '')
        })

    # Adds a line item for level 3 processing. Repeat method for each item up to 99 items.
    # For Canadian and AMEX processors, ensure required Level 2 information is present.
    def add_line_item(self, **params):
        i = len(self.line_items) + 1
        prefix = 'LV3_ITEM' + str(i) + '_'
        self.line_items.append(                                                         #  VALUE REQUIRED IN:
            {                                                                           #  USA | CANADA
                prefix + 'UNIT_COST' : params.get('unit_cost'),                         #   *      *
                prefix + 'QUANTITY' : params.get('quantity'),                           #   *      *
                prefix + 'ITEM_SKU' : params.get('item_sku', ''),                       #          *
                prefix + 'ITEM_DESCRIPTOR' : params.get('descriptor', ''),              #   *      *       
                prefix + 'COMMODITY_CODE' : params.get('commodity_code', ''),           #   *      *    
                prefix + 'PRODUCT_CODE' : params.get('product_code', ''),               #   * 
                prefix + 'MEASURE_UNITS' : params.get('measure_units', ''),             #   *      *
                prefix + 'ITEM_DISCOUNT' : params.get('item_discount', ''),             #          *
                prefix + 'TAX_RATE' : params.get('tax_rate', ''),                       #   * 
                prefix + 'GOODS_TAX_RATE' : params.get('goods_tax_rate', ''),           #          *
                prefix + 'TAX_AMOUNT' : params.get('tax_amount', ''),                   #   *
                prefix + 'GOODS_TAX_AMOUNT' : params.get('goods_tax_amount', ''),       #   *     
                prefix + 'CITY_TAX_RATE' : params.get('city_tax_rate', ''),             #
                prefix + 'CITY_TAX_AMOUNT' : params.get('city_tax_amount', ''),         #    
                prefix + 'COUNTY_TAX_RATE' : params.get('county_tax_rate', ''),         #
                prefix + 'COUNTY_TAX_AMOUNT' : params.get('county_tax_amount', ''),     #    
                prefix + 'STATE_TAX_RATE' : params.get('state_tax_rate', ''),           #
                prefix + 'STATE_TAX_AMOUNT' : params.get('state_tax_amount', ''),       #    
                prefix + 'CUST_SKU' : params.get('cust_sku', ''),                       #
                prefix + 'CUST_PO' : params.get('cust_po', ''),                         #
                prefix + 'SUPPLEMENTAL_DATA' : params.get('supplemental_data', ''),     #    
                prefix + 'GL_ACCOUNT_NUMBER' : params.get('gl_account_number', ''),     #    
                prefix + 'DIVISION_NUMBER' : params.get('division_number', ''),         #
                prefix + 'PO_LINE_NUMBER' : params.get('po_line_number', ''),           #
                prefix + 'LINE_ITEM_TOTAL' : params.get('line_item_total', '')          #   * 
            }
        )

    # Passes rebilling information into the transaction
    def set_rebilling_information(self, **params):
        self.do_rebill = '1'
        self.reb_first_date = params['reb_first_date']
        self.reb_expr = params['reb_expr']
        self.reb_cycles = params['reb_cycles']
        self.reb_amount = params['reb_amount']
         
    # Passes rebilling information for a rebill update
    def update_rebill(self, **params):
        self.api = "bp20rebadmin"
        self.trans_type = 'SET'
        self.rebill_id = params['rebill_id']
        if 'template_id' in params:
            self.template_id = params['template_id']
        if 'reb_next_date' in params:
            self.reb_next_date = params['reb_next_date']
        if 'reb_expr' in params:
            self.reb_expr = params['reb_expr']
        if 'reb_cycles' in params: 
            self.reb_cycles = params['reb_cycles']
        if 'reb_amount' in params: 
            self.reb_amount = params['reb_amount']
        if 'reb_next_amount' in params:
            self.reb_next_amount = params['reb_next_amount']

    # Passes rebilling information for a rebill cancel
    def cancel_rebill(self, rebill_id):
        self.trans_type = 'SET'
        self.reb_status = 'stopped'
        self.api = "bp20rebadmin"
        self.rebill_id = rebill_id

    # Set fields to get the status of an existing rebilling cycle
    def get_rebilling_cycle_status(self, rebill_id):
        self.trans_type = 'GET'
        self.api = "bp20rebadmin"
        self.rebill_id = rebill_id

    # Updates an existing rebilling cycle's payment information.   
    def update_rebilling_payment_information(self, template_id):
        self.template_id = template_id
        

    # Passes values for a call to the bpdailyreport2 API to get all transactions based on start/end dates
    def get_transaction_report(self, **params):
        self.query_by_settlement = '0'
        self.api = "bpdailyreport2"
        self.report_start_date = params['report_start']
        self.report_end_date = params['report_end']
        self.subaccounts_searched = params['subaccounts_searched']
        if ('do_not_escape' in params):
            self.do_not_escape = params['do_not_escape'] 
        if ('exclude_errors' in params):
            self.excludeErrors = params['exclude_errors']


    # Passes values for a call to the bpdailyreport2 API to get settled transactions based on start/end dates    
    def get_settled_transaction_report(self, **params):
        self.api = "bpdailyreport2"
        self.query_by_settlement = '1'
        self.report_start_date = params['report_start']
        self.report_end_date = params['report_end']
        if ('subaccounts_searched' in params):    
            self.subaccounts_searched = params['subaccounts_searched']
        if ('do_not_escape' in params):
            self.do_not_escape = params['do_not_escape'] 
        if ('exclude_errors' in params):
            self.excludeErrors = params['exclude_errors']

    # Passes values for a call to the stq API to get information on a single transaction
    def get_single_trans_query(self, **params):
        self.api = "stq"
        self.trans_id = params['transaction_id']
        self.report_start_date = params['report_start']
        self.report_end_date = params['report_end']
        if ('exclude_errors' in params):
            self.excludeErrors = params['exclude_errors']

    # Queries transactions by a specific Transaction ID. Must be used with getSingleTransQuery
    def query_by_transaction_id(self, trans_id):
        self.trans_id = trans_id
        

    # Queries transactions by a specific Payment Type. Must be used with getSingleTransQuery
    def query_by_payment_type(self, pay_type):
        self.payment_type = pay_type
        

    # Queries transactions by a specific Transaction Type. Must be used with getSingleTransQuery
    def query_by_trans_type(self, trans_type):
        self.trans_type = trans_type
        

    # Queries transactions by a specific Transaction Amount. Must be used with getSingleTransQuery
    def query_by_amount(self, amount):
        self.amount = amount
        

    # Queries transactions by a specific First Name. Must be used with getSingleTransQuery
    def query_by_name1(self, name1):
        self.name1 = name1
        

    # Queries transactions by a specific Last Name. Must be used with getSingleTransQuery
    def query_by_name2(self, name2):
        self.name2 = name2
        
    ### API REQUEST ###

    # Functions for calculating the TAMPER_PROOF_SEAL
    def create_tps_hash(self, string, hash_type):
        tps_hash = ""
        if hash_type == "HMAC_SHA256":
            tps_hash = hmac.new(self.secret_key, string, hashlib.sha256).hexdigest()
        elif hash_type == "SHA512":
            m = hashlib.sha512()
            m.update(self.secret_key + string)
            tps_hash = m.hexdigest()
        elif hash_type == "SHA256":
            m = hashlib.sha256()
            m.update(self.secret_key + string)
            tps_hash = m.hexdigest()
        elif hash_type == "MD5":
            m = hashlib.md5()
            m.update(self.secret_key + string)
            tps_hash = m.hexdigest()
        else:
            tps_hash = hmac.new(self.secret_key, string, hashlib.sha512).hexdigest()

        return tps_hash


    def calc_TPS(self):
        tps_string = (self.account_id + self.trans_type + self.amount + self.do_rebill + 
                        self.reb_first_date + self.reb_expr + self.reb_cycles + self.reb_amount + 
                        self.rrno + self.mode)
        tps = self.create_tps_hash(tps_string, self.tps_hash_type)
        return tps

    def calc_rebill_TPS(self):
        tps_string = (self.account_id + self.trans_type + self.rebill_id)
        tps = self.create_tps_hash(tps_string, self.tps_hash_type)
        return tps 
        
    def calc_report_TPS(self):
        tps_string = (self.account_id + self.report_start_date + self.report_end_date)
        tps = self.create_tps_hash(tps_string, self.tps_hash_type)
        return tps
        
    def calc_trans_notify_TPS(self):
        tps_string = (trans_id, trans_status, trans_type, amount, batch_id, batch_status,
                      total_count, total_amount, batch_upload_id, rebill_id, rebill_amount, rebill_status, hash_type)
        tps = self.create_tps_hash(tps_string, hash_type)
        return tps

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
    def generate_url(self, **params):
        self.dba = params['merchant_name'] 
        self.return_url = params['return_url']  
        self.trans_type = params['transaction_type']  
        self.accept_discover = 'discvr.gif' if re.match(r'(^[Yy])', params['accept_discover']) else 'spacer.gif'
        self.accept_amex = 'amex.gif' if re.match(r'(^[Yy])', params['accept_amex']) else 'spacer.gif'
        if 'amount' in params:
            self.amount = params['amount']
        if 'protect_amount' in params:
            self.protect_amount = params['protect_amount']
        else:
            self.protect_amount ="No" 
        self.do_rebill = '1' if re.match(r'(^[Yy])', params['rebilling']) else '0'
        if 'reb_protect' in params:
            self.reb_protect = params['reb_protect']
        else:
            self.reb_protect = 'Yes' 
        if 'reb_protect' in params:
            self.reb_amount = params['reb_amount']
        if 'reb_cycles' in params:
            self.reb_cycles = params['reb_cycles']
        if 'reb_start_date' in params:
            self.reb_first_date = params['reb_start_date']
        if 'reb_frequency' in params:
            self.reb_expr = params['reb_frequency']
        if 'custom_id' in params:
            self.custom_id1 = params['custom_id']
        if 'protect_custom_id' in params:
            self.protect_custom_id = params['protect_custom_id']
        else:
            self.protect_custom_id = "No"
        if 'custom_id2' in params:
            self.custom_id2 = params['custom_id2']
        if 'protect_custom_id2' in params:
            self.protect_custom_id2 = params['protect_custom_id2']
        else:
            self.protect_custom_id2 = "No" 
        if 'payment_template' in params:
            self.shpf_form_id = params['payment_template']
        else:
            self.shpf_form_id = "mobileform01"
        if 'receipt_template' in params:
            self.receipt_form_id = params['receipt_template']
        else:
            self.receipt_form_id = "mobileresult01"
        if 'receipt_temp_remote_url' in params:
            self.remote_url = params['receipt_temp_remote_url']
        self.shpf_tps_hash_type = 'HMAC_SHA512'
        self.receipt_tps_hash_type = self.shpf_tps_hash_type
        self.tps_hash_type = self.set_hash_type(params.get('tps_hash_type', ''))
        self.card_types = self.set_card_types()
        self.receipt_tps_def = 'SHPF_ACCOUNT_ID SHPF_FORM_ID RETURN_URL DBA AMEX_IMAGE DISCOVER_IMAGE SHPF_TPS_DEF SHPF_TPS_HASH_TYPE'
        self.receipt_tps_string = self.set_receipt_tps_string()
        self.receipt_tamper_proof_seal = self.create_tps_hash(self.receipt_tps_string, self.receipt_tps_hash_type)
        self.receipt_url = self.set_receipt_url()
        self.bp10emu_tps_def = self.add_def_protected_status('MERCHANT APPROVED_URL DECLINED_URL MISSING_URL MODE TRANSACTION_TYPE TPS_DEF TPS_HASH_TYPE')
        self.bp10emu_tps_string = self.set_bp10emu_tps_string()
        self.bp10emu_tamper_proof_seal = self.create_tps_hash(self.bp10emu_tps_string, self.tps_hash_type)
        self.shpf_tps_def = self.add_def_protected_status('SHPF_FORM_ID SHPF_ACCOUNT_ID DBA TAMPER_PROOF_SEAL AMEX_IMAGE DISCOVER_IMAGE TPS_DEF TPS_HASH_TYPE SHPF_TPS_DEF SHPF_TPS_HASH_TYPE')
        self.shpf_tps_string = self.set_shpf_tps_string()
        self.shpf_tamper_proof_seal = self.create_tps_hash(self.shpf_tps_string, self.shpf_tps_hash_type)
        return self.calc_url_response()

    # Validates hash type or uses default
    def set_hash_type(self, chosen_hash):
        default_hash = 'HMAC_SHA512'
        result = chosen_hash.upper() if chosen_hash.upper() in ['MD5', 'SHA256', 'SHA512', 'HMAC_SHA256'] else default_hash
        return result

    # Sets the types of credit card images to use on the Simple Hosted Payment Form. Must be used with generate_url.
    def set_card_types(self):
        credit_cards = 'vi-mc'
        if self.accept_discover == 'discvr.gif':
            credit_cards += '-di'
        if self.accept_amex == 'amex.gif':
            credit_cards += '-am'
        return credit_cards

    # Sets the receipt Tamperproof Seal string. Must be used with generate_url.
    def set_receipt_tps_string(self):
        return ''.join(self.account_id +
            self.receipt_form_id +
            self.return_url +
            self.dba +
            self.accept_amex +
            self.accept_discover +
            self.receipt_tps_def + 
            self.receipt_tps_hash_type)

    # Sets the bp10emu string that will be used to create a Tamperproof Seal. Must be used with generate_url.
    def set_bp10emu_tps_string(self):
        bp10emu = ''.join(self.account_id + 
            self.receipt_url + 
            self.receipt_url +
            self.receipt_url +
            self.mode +
            self.trans_type +
            self.bp10emu_tps_def + 
            self.tps_hash_type)
        return self.add_string_protected_status(bp10emu)

    # Sets the Simple Hosted Payment Form string that will be used to create a Tamperproof Seal. Must be used with generate_url.
    def set_shpf_tps_string(self):
        shpf = ''.join(self.shpf_form_id +
            self.account_id + 
            self.dba + 
            self.bp10emu_tamper_proof_seal +
            self.accept_amex +
            self.accept_discover +
            self.bp10emu_tps_def +
            self.tps_hash_type +
            self.shpf_tps_def + 
            self.shpf_tps_hash_type)
        return self.add_string_protected_status(shpf)

    # Sets the receipt url or uses the remote url provided. Must be used with generate_url.
    def set_receipt_url(self):
        if self.receipt_form_id == 'remote_url':
            return self.remote_url
        else:
            return ''.join('https://secure.bluepay.com/interfaces/shpf?' + 
            'SHPF_FORM_ID=' + self.receipt_form_id + 
            '&SHPF_ACCOUNT_ID=' + self.account_id + 
            '&SHPF_TPS_DEF=' + self.url_encode(self.receipt_tps_def) + 
            '&SHPF_TPS_HASH_TYPE=' + self.url_encode(self.receipt_tps_hash_type) +
            '&SHPF_TPS=' + self.url_encode(self.receipt_tamper_proof_seal) + 
            '&RETURN_URL=' + self.url_encode(self.return_url) + 
            '&DBA=' + self.url_encode(self.dba) + 
            '&AMEX_IMAGE=' + self.url_encode(self.accept_amex) + 
            '&DISCOVER_IMAGE=' + self.url_encode(self.accept_discover))

    # Adds optional protected keys to a string. Must be used with generate_url.
    def add_def_protected_status(self, string):
        if self.protect_amount == 'Yes':
            string +=' AMOUNT'
        if self.reb_protect == 'Yes':
            string += ' REBILLING REB_CYCLES REB_AMOUNT REB_EXPR REB_FIRST_DATE'
        if self.protect_custom_id == 'Yes':
            string += ' CUSTOM_ID'
        if self.protect_custom_id2 == 'Yes':
            string += ' CUSTOM_ID2'
        return string 

    # Adds optional protected values to a string. Must be used with generate_url.
    def add_string_protected_status(self, string):
        if self.protect_amount == 'Yes':
            string += self.amount
        if self.reb_protect == 'Yes':
            string += self.do_rebill + self.reb_cycles + self.reb_amount + self.reb_expr + self.reb_first_date
        if self.protect_custom_id == 'Yes':
            string += self.custom_id1
        if self.protect_custom_id2 == 'Yes':
            string += self.custom_id2
        return string

    # Encodes a string into a URL. Must be used with generate_url.
    def url_encode(self, string):
        encoded_string = ''
        for char in string:
            if re.match(r'[^A-Za-z0-9]', char):
                char = "%%%02X" % ord(char)
            encoded_string += char
        return encoded_string
    
    # Generates the final url for the Simple Hosted Payment Form. Must be used with generate_url.
    def calc_url_response(self):
        return ''.join('https://secure.bluepay.com/interfaces/shpf?' +
            'SHPF_FORM_ID='       + self.url_encode(self.shpf_form_id) +
            '&SHPF_ACCOUNT_ID='   + self.url_encode(self.account_id) +
            '&SHPF_TPS_DEF='      + self.url_encode(self.shpf_tps_def) +
            '&SHPF_TPS_HASH_TYPE='+ self.url_encode(self.shpf_tps_hash_type) +
            '&SHPF_TPS='          + self.url_encode(self.shpf_tamper_proof_seal) +
            '&MODE='              + self.url_encode(self.mode) +
            '&TRANSACTION_TYPE='  + self.url_encode(self.trans_type) +
            '&DBA='               + self.url_encode(self.dba) +
            '&AMOUNT='            + self.url_encode(self.amount) +
            '&TAMPER_PROOF_SEAL=' + self.url_encode(self.bp10emu_tamper_proof_seal) +
            '&CUSTOM_ID='         + self.url_encode(self.custom_id1) +
            '&CUSTOM_ID2='        + self.url_encode(self.custom_id2) +
            '&REBILLING='         + self.url_encode(self.do_rebill) +
            '&REB_CYCLES='        + self.url_encode(self.reb_cycles) +
            '&REB_AMOUNT='        + self.url_encode(self.reb_amount) +
            '&REB_EXPR='          + self.url_encode(self.reb_expr) +
            '&REB_FIRST_DATE='    + self.url_encode(self.reb_first_date) +
            '&AMEX_IMAGE='        + self.url_encode(self.accept_amex) +
            '&DISCOVER_IMAGE='    + self.url_encode(self.accept_discover) +
            '&REDIRECT_URL='      + self.url_encode(self.receipt_url) +
            '&TPS_DEF='           + self.url_encode(self.bp10emu_tps_def) +
            '&TPS_HASH_TYPE='     + self.url_encode(self.tps_hash_type) +
            '&CARD_TYPES='        + self.url_encode(self.card_types))

    ### PROCESSES THE API REQUEST ####
    def process(self, card=None, customer=None, order=None):
        fields = {
            'MODE': self.mode,
            'RRNO': self.rrno
        }
        if self.api == 'bpdailyreport2':
            self.url = 'https://secure.bluepay.com/interfaces/bpdailyreport2'
            fields.update({
                'ACCOUNT_ID': self.account_id,
                'REPORT_START_DATE' : self.report_start_date,
                'REPORT_END_DATE' : self.report_end_date,
                'TAMPER_PROOF_SEAL' : self.calc_report_TPS(),
                'DO_NOT_ESCAPE' : self.do_not_escape,
                'QUERY_BY_SETTLEMENT' : self.query_by_settlement,
                'QUERY_BY_HIERARCHY' : self.subaccounts_searched,
                'EXCLUDE_ERRORS' : self.excludeErrors,
                'TPS_HASH_TYPE': self.tps_hash_type
            })
        elif self.api == 'stq':
            self.url = 'https://secure.bluepay.com/interfaces/stq'
            fields.update({
                'ACCOUNT_ID': self.account_id,
                'REPORT_START_DATE' : self.report_start_date,
                'REPORT_END_DATE' : self.report_end_date,
                'TAMPER_PROOF_SEAL' : self.calc_report_TPS(),
                'EXCLUDE_ERRORS' : self.excludeErrors,
                'IGNORE_NULL_STR' : '1',
                'TPS_HASH_TYPE': self.tps_hash_type,
                'id' : self.trans_id,
                'payment_type' : self.payment_type,
                'trans_type' : self.trans_type,
                'amount' : self.amount,
                'name1' : self.name1,
                'name2' : self.name2
            })
        elif self.api == 'bp10emu':
            self.url = 'https://secure.bluepay.com/interfaces/bp10emu'
            fields.update({
                'MERCHANT': self.account_id,
                'TRANSACTION_TYPE': self.trans_type,
                'PAYMENT_TYPE': self.payment_type,
                'TAMPER_PROOF_SEAL': self.calc_TPS(),
                'AMOUNT': self.amount,
                'NAME1': self.name1,
                'NAME2': self.name2,
                'ADDR1': self.addr1,
                'ADDR2': self.addr2,
                'CITY': self.city,
                'STATE': self.state,
                'ZIPCODE': self.zipcode,
                'COUNTRY': self.country,
                'EMAIL': self.email,
                'PHONE': self.phone,
                'CUSTOM_ID': self.custom_id1,
                'CUSTOM_ID2': self.custom_id2,
                'INVOICE_ID': self.invoice_id,
                'ORDER_ID': self.order_id,
                'COMMENT': self.memo,
                'AMOUNT_TAX': self.amount_tax,
                'AMOUNT_TIP': self.amount_tip,
                'AMOUNT_FOOD': self.amount_food,
                'AMOUNT_MISC': self.amount_misc,
                'REBILLING': self.do_rebill,
                'REB_FIRST_DATE': self.reb_first_date,
                'REB_EXPR': self.reb_expr,
                'REB_CYCLES': self.reb_cycles,
                'REB_AMOUNT': self.reb_amount,
                'SWIPE': self.track_data,
                'TPS_HASH_TYPE': self.tps_hash_type
            })
            try:
                fields.update({
                    'REMOTE_IP' : cgi.escape(os.environ["REMOTE_ADDR"])
                })
            except KeyError:
                pass
                if self.payment_type == 'CREDIT':
                    fields.update({
                        'CC_NUM': self.card_number, 
                        'CC_EXPIRES': self.card_expire,
                        'CVCVV2': self.cvv2
                    })
                else:
                    fields.update({
                        'ACH_ROUTING': self.routing_number,
                        'ACH_ACCOUNT': self.account_number,
                        'ACH_ACCOUNT_TYPE': self.account_type,
                        'DOC_TYPE': self.doc_type
                    })
        elif self.api == 'bp20rebadmin':
            self.url = 'https://secure.bluepay.com/interfaces/bp20rebadmin'
            fields.update({
                'ACCOUNT_ID': self.account_id,
                'TRANS_TYPE': self.trans_type,
                'REBILL_ID': self.rebill_id,
                'TEMPLATE_ID' : self.template_id,
                'NEXT_DATE': self.reb_next_date,
                'REB_EXPR': self.reb_expr,
                'REB_CYCLES': self.reb_cycles,
                'REB_AMOUNT': self.reb_amount,
                'NEXT_AMOUNT': self.reb_next_amount,
                'STATUS': self.reb_status,
                'TAMPER_PROOF_SEAL': self.calc_rebill_TPS(),
                'TPS_HASH_TYPE': self.tps_hash_type
            })

        fields.update(self.level2_info) # Update fields dictionary with Level 2 processing information, if available.
            
        for item in self.line_items: # Update fields dictionary with line item information, if available.
            fields.update(item)
        
        response = self.request(self.url, self.create_post_string(fields).encode())
        parsed_response = self.parse_response(response)
        return parsed_response

    def create_post_string(self, fields):
        fields = dict([k,str(v).replace(',', '')] for (k,v) in six.iteritems(fields))
        return urlencode(fields)
    
    def request(self, url, data):
        """
        Submits an https request to BluePay.
        """
        response = self.send(data)
        return response

    def send(self, data):
        """
        Send an https request.
        """
        try:
            r = urlopen(self.url, data)
            #response = r.read()
            response = r.geturl()
            return response
        except HTTPError as e:
            if re.match("https://secure.bluepay.com/interfaces/wlcatch", e.geturl()):
                response = e.geturl()
                return response
                #return e.read()
            return "ERROR"
    
    def parse_response(self, response):
        if self.api == 'bpdailyreport2':
            self.response = response
        elif self.api == 'bp10emu':        
            query_string = urlparse(response)
            response = parse_qs(query_string.query)
            self.response = response
            self.assign_response_values()
        elif self.api == 'stq' or self.api == 'bp20rebadmin':
            response = parse_qs(response)
            self.response = response
            self.assign_response_values()

    # Verifies whether transaction was approved.
    # Returns true if the response is successful, else returns false
    def is_successful_response(self):
        # return True  # Remove this line for production
        return self.status_response == 'APPROVED' and self.message_response != "DUPLICATE"

    def __str__(self):
        return 'BluePay Python sample code for the BP10Emu API'
    

        #######  RESPONSE VALUES ####
        # assigns values to an empty string if value does not exist
    def assign_response_values(self):
        # print self.response
        # print ''
        self.status_response = self.response['Result'][0] if 'Result' in self.response else ''
        # Returns the human-readable response from Bluepay.
        self.message_response = self.response['MESSAGE'][0] if 'MESSAGE' in self.response else ''
        # The all-important transaction ID.
        self.trans_id_response = self.response['RRNO'][0] if 'RRNO' in self.response else ''  
        # Returns the single-character AVS response from the 
        # Card Issuing Bank
        self.avs_code_response = self.response['AVS'][0] if 'AVS' in self.response else ''
        # Same as avs_code, but for CVV2
        self.cvv2_code_response = self.response['CVV2'][0] if 'CVV2' in self.response else ''
        # In the case of an approved transaction, contains the
        # 6-character authorization code from the processing network.
        # In the case of a decline or error, the contents may be junk.        
        self.auth_code_response = self.response['AUTH_CODE'][0] if 'AUTH_CODE' in self.response else ''
        # If you set up a rebilling, this'll get its ID.
        self.reb_id_response = self.response['REBID'][0] if 'REBID' in self.response else ''
        # Masked credit card or ACH account
        self.masked_account_response = self.response['PAYMENT_ACCOUNT'][0] if 'PAYMENT_ACCOUNT' in self.response else ''
        # Card type used in transaction
        self.card_type_response = self.response['CARD_TYPE'][0] if 'CARD_TYPE' in self.response else ''
        # Bank account used in transaction
        self.bank_name_response = self.response['BANK_NAME'][0] if 'BANK_NAME' in self.response else ''
        # Rebill ID from bprebadmin API
        self.rebill_id_response = self.response['rebill_id'][0] if 'rebill_id' in self.response else ''
        # Template ID of rebilling
        self.get_template_id = self.response['template_id'][0] if 'template_id' in self.response else ''
        # Status of rebilling
        self.rebill_status_response = self.response['status'][0] if 'status' in self.response else ''
        # Creation date of rebilling
        self.creation_date_response = self.response['creation_date'][0] if 'creation_date' in self.response else ''
        # Next date that the rebilling is set to fire off on
        self.next_date_response = self.response['next_date'][0] if 'next_date' in self.response else ''
        # Last date that the rebilling fired off on
        self.last_date_response = self.response['last_date'][0] if 'last_date' in self.response else ''
        # Rebilling expression
        self.sched_expression_response = self.response['sched_expr'][0] if 'sched_expr' in self.response else ''
        # Number of cycles remaining on rebilling
        self.cycles_remaining_response = self.response['cycles_remain'][0] if 'cycles_remain' in self.response else ''
        # Amount to charge when rebilling fires off
        self.rebill_amount_response = self.response['reb_amount'][0] if 'reb_amount' in self.response else ''
        # Next amount to charge when rebilling fires off
        self.next_amount_response = self.response['next_amount'][0] if 'next_amount' in self.response else ''
        # Transaction ID used with stq API
        self.id_response = self.response['id'][0] if 'id' in self.response else ''
        # First name associated with the transaction
        self.name1_response = self.response['name1'][0] if 'name1' in self.response else ''
        # Last name associated with the transaction
        self.name2_response = self.response['name2'][0] if 'name2' in self.response else ''
        # Payment type associated with the transaction
        self.payment_type_response = self.response['payment_type'][0] if 'payment_type' in self.response else ''
        # Transaction type associated with the transaction
        self.trans_type_response = self.response['trans_type'][0] if 'trans_type' in self.response else ''
        # Amount associated with the transaction
        self.amount_response = self.response['amount'][0] if 'amount' in self.response else ''

    


 
    
