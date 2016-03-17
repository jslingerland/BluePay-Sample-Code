##
# BluePay Python Sample code.
#
# This code sample creates a recurring payment charging $15.00 per month for one year.
##

import os.path, sys
sys.path.append(os.path.join(os.path.dirname(os.path.realpath(__file__)), os.pardir))
from BluePay import BluePay

account_id = "Merchant's Account ID Here"
secret_key = "Merchant's Secret Key Here"
mode = "TEST"  

payment = BluePay(
    account_id = account_id, 
    secret_key = secret_key, 
    mode = mode
)

payment.set_customer_information(
    name1 = "Bob",
    name2 = "Tester",
    addr1 = "123 Test St.",
    addr2 = "Apt #500",
    city = "Testville",
    state = "IL",
    zipcode = "54321",
    country = "USA"
)

payment.set_cc_information(
    card_number = "4111111111111111",
    card_expire = "1215",
    cvv2 = "123"
)

payment.auth(amount = '0.00')

# Set Recurring Payment Information
payment.set_rebilling_information(
  reb_first_date = "2015-01-01", # Rebill Start Date: Jan. 1, 2015
  reb_expr = "1 MONTH", # Rebill Frequency: 1 MONTH
  reb_cycles = "12", # Rebill # of Cycles: 12
  reb_amount = "15.00" # Rebill Amount: $15.00
)

# Makes the API Request
payment.process()

# Read response from BluePay
if payment.is_successful_response():
    print 'Transaction ID: ' + payment.trans_id_response
    print 'Rebill ID: ' + payment.reb_id_response
    print 'Transaction Status: ' + payment.status_response
    print 'Transaction Message: ' + payment.message_response
    print 'Transaction AVS Result: ' + payment.avs_code_response
    print 'Transaction CVV2 Result: ' + payment.cvv2_code_response
    print 'Masked Payment Account: ' + payment.masked_account_response
    print 'Transaction Auth Code: ' + payment.auth_code_response
else:
    print payment.message_response