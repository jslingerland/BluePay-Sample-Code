import urllib
from urllib2 import Request, urlopen, URLError, HTTPError
import urlparse
import hashlib
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
    
    # Response fields
    reb_status = ''
    rebill_id = ''
    rrno = ''
    data = ''

    url = ''

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
        
    # Passes ACH information into the transaction
    def set_ach_information(self, **params):
        self.payment_type = 'ACH'
        self.routing_number = params['routing_number']
        self.account_number = params['account_number']
        self.account_type = params['account_type']
        self.doc_type = params['doc_type']
         
       
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
    def calc_TPS(self):
        tps_string = (self.secret_key + self.account_id + self.trans_type + self.amount +
                      self.do_rebill + self.reb_first_date + self.reb_expr + self.reb_cycles + 
                      self.reb_amount + self.rrno + self.mode)
        m = hashlib.md5()
        m.update(tps_string)
        return m.hexdigest()

    def calc_rebill_TPS(self):
        tps_string = (self.secret_key + self.account_id + self.trans_type + self.rebill_id)
        m = hashlib.md5()
        m.update(tps_string)
        return m.hexdigest()   
        
    def calc_report_TPS(self):
        tps_string = (self.secret_key + self.account_id + self.report_start_date + self.report_end_date)
        m = hashlib.md5()
        m.update(tps_string)
        return m.hexdigest()
        
    def calc_trans_notify_TPS(self):
        tps_string = (secret_key, trans_id, trans_status, trans_type, amount, batch_id, batch_status,
                      total_count, total_amount, batch_upload_id, rebill_id, rebill_amount, rebill_status)
        m = hashlib.md5()
        m.update(tps_string)
        return m.hexdigest()

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
                'EXCLUDE_ERRORS' : self.excludeErrors
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
                'TAMPER_PROOF_SEAL': self.calc_TPS()
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
                'TAMPER_PROOF_SEAL': self.calc_rebill_TPS()
            })
        response = self.request(self.url, self.create_post_string(fields))
        parsed_response = self.parse_response(response)
        return parsed_response

    def create_post_string(self, fields):
        fields = dict([k,str(v).replace(',', '')] for (k,v) in fields.iteritems())       
        return urllib.urlencode(fields)
    
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
            response = r.read()
            return response
        except HTTPError, e:
            if re.match("https://secure.bluepay.com/interfaces/wlcatch", e.geturl()):
                response = e.geturl()
                return response
                return e.read()
            return "ERROR"
    
    def parse_response(self, response):
        if self.api == 'bpdailyreport2':
            self.response = response
        elif self.api == 'bp10emu':        
            query_string = urlparse.urlparse(response)
            response = urlparse.parse_qs(query_string.query)
            self.response = response
            self.assign_response_values()
        elif self.api == 'stq' or self.api == 'bp20rebadmin':
            response = cgi.parse_qs(response)
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

    


 
    
