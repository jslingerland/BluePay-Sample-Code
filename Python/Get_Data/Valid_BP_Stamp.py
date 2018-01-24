##
# BluePay Python Sample code.
#
# This code sample runs a $3.00 Credit Card sales transaction
# against a customer using test payment information.
# It then validates the BP_STAMP returned by BluePay
# to authenticate the transaction.
# If using TEST mode, odd dollar amounts will return
# an approval and even dollar amounts will return a decline.
##
from __future__ import print_function
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
    card_expire = "1225",
    cvv2 = "123"    
)

payment.sale(amount = '3.00') # Sale Amount: $3.00

# Makes the API Request
payment.process()

# Read response from BluePay
if payment.is_successful_response():
    print('Transaction Status: ' + payment.status_response)
    print('Transaction Message: ' + payment.message_response)
    print('Transaction ID: ' + payment.trans_id_response)
    print('AVS Result: ' + payment.avs_code_response)
    print('CVV2 Result: ' + payment.cvv2_code_response)
    print('Masked Payment Account: ' + payment.masked_account_response)
    print('Card Type: ' + payment.card_type_response)
    print('Auth Code: ' + payment.auth_code_response)
    # Validate the BP_STAMP. Returns "TRUE" for a valid stamp, "FALSE" for invalid.
    print('Valid BP_STAMP: ' + payment.valid_bp_stamp())
else:
    print(payment.message_response)
