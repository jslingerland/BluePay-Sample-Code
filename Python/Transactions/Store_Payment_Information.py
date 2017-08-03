##
# BluePay Python Sample code.
#
# This code sample runs a $0.00 Credit Card Auth transaction
# against a customer using test payment information.
# This stores the customer's payment information securely in
# BluePay to be used for further transactions.
# Note: THIS DOES NOT ENSURE THAT THE CREDIT CARD OR ACH
# ACCOUNT IS VALID.
##
from __future__ import print_function
import os.path, sys
sys.path.append(os.path.join(os.path.dirname(os.path.realpath(__file__)), os.pardir))
from BluePay import BluePay

account_id = "Merchant's Account ID Here"
secret_key = "Merchant's Secret Key Here"
mode = "TEST"  

store_payment = BluePay(
    account_id = account_id, 
    secret_key = secret_key, 
    mode = mode
)

store_payment.set_customer_information(
    name1 = "Bob",
    name2 = "Tester",
    addr1 = "123 Test St.",
    addr2 = "Apt #500",
    city = "Testville",
    state = "IL",
    zipcode = "54321",
    country = "USA"
)

store_payment.set_cc_information(
    card_number = "4111111111111111",
    card_expire = "1215",
    cvv2 = "123"
)

store_payment.auth(amount = "0.00") 

# Makes the API Request
store_payment.process()

# Read response from BluePay
if store_payment.is_successful_response():
    print('Transaction Status: ' + store_payment.status_response)
    print('Transaction Message: ' + store_payment.message_response)
    print('Transaction ID: ' + store_payment.trans_id_response)
    print('AVS Response: ' + store_payment.avs_code_response)
    print('CVV2 Response: ' + store_payment.cvv2_code_response)
    print('Masked Payment Account: ' + store_payment.masked_account_response)
    print('Card Type: ' + store_payment.card_type_response)
    print('Auth Code: ' + store_payment.auth_code_response)
else:
    print(store_payment.message_response)