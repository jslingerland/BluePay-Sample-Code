##
# BluePay Python Sample code.
#
# This code sample runs a $3.00 Credit Card Sale transaction
# using a token from a previous transaction.
# If using TEST mode, odd dollar amounts will return
# an approval and even dollar amounts will return a decline.
##

import os.path, sys
sys.path.append(os.path.join(os.path.dirname(os.path.realpath(__file__)), os.pardir))
from BluePay import BluePay

account_id = "100228390579"
secret_key = "AKGIF9X9WT9CLQCWDFONC8N3HXRL9Y5K"
mode = "TEST"
token = "100228422096" # the transaction id of a previous sale

payment = BluePay(
    account_id = account_id, # Merchant's Account ID
    secret_key = secret_key, # Merchant's Secret Key
    mode = mode # Transaction Mode: TEST (can also be LIVE)
)

# Charges a customer $3.00 using the payment information from a previous transaction.
payment.sale(
    amount = '3.00', 
    transaction_id = token 
)

# Makes the API Request 
payment.process()

# Read response from BluePay
if payment.is_successful_response():
    print 'Transaction Status: ' + payment.status_response
    print 'Transaction Message: ' + payment.message_response
    print 'Transaction ID: ' + payment.trans_id_response
    print 'AVS Result: ' + payment.avs_code_response
    print 'CVV2 Result: ' + payment.cvv2_code_response
    print 'Masked Payment Account: ' + payment.masked_account_response
    print 'Card Type: ' + payment.card_type_response
    print 'Auth Code: ' + payment.auth_code_response
else:
    print payment.message_response