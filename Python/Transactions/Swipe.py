##
# BluePay Python Sample code.
#
# This code sample runs a $3.00  sales transaction using the payment information obtained from a credit card swipe.
# If using TEST mode, odd dollar amounts will return an approval and even dollar amounts will return a decline.
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

# Set payment information for a swiped credit card transaction
payment.swipe("%B4111111111111111^TEST/BLUEPAY^1911101100001100000000667000000?;4111111111111111=191110110000667?")

# Sale Amount: $3.00
payment.sale(amount = '3.00') 

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