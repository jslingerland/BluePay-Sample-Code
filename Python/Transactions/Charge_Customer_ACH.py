##
# BluePay Python Sample code.
#
# This code sample runs a $3.00 ACH Sale transaction
# against a customer using test payment information.
##

import os.path, sys
sys.path.append(os.path.join(os.path.dirname(os.path.realpath(__file__)), os.pardir))
from BluePay import BluePay

account_id = "100228390579"
secret_key = "AKGIF9X9WT9CLQCWDFONC8N3HXRL9Y5K"
mode = "TEST"

payment = BluePay(
    account_id = account_id, # Merchant's Account ID
    secret_key = secret_key, # Merchant's Secret Key
    mode = mode # Transaction Mode: TEST (can also be LIVE)
)

# Set Customer Information
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

# Set ACH Payment Information
payment.set_ach_information(
    routing_number = "123123123", # Routing Number: 123123123
    account_number = "05125121", # Account Number: 0523421
    account_type = "C", # Account Type: Checking
    doc_type = "WEB" # ACH Document Type: WEB
)

payment.sale(amount = '3.00') # Sale Amount: $3.00

# Makes the API Request
payment.process()

# Read response from BluePay
if payment.is_successful_response():
    print 'Transaction ID: ' + payment.trans_id_response
    print 'Transaction Status: ' + payment.status_response
    print 'Transaction Message: ' + payment.message_response
    print 'Masked Payment Account: ' + payment.masked_account_response
    print 'Customer Bank: ' + payment.bank_name_response
else:    
    print payment.message_response

