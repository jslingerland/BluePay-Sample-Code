##
# BluePay Python Sample code.
#
# This code sample runs a $0.00 AUTH transaction
# and creates a customer token using test payment information,
# which is then used to run a separate $3.99 sale.
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
    #stored_indicator = 'F',
    #stored_type = 'C',
    #stored_id = 'TESTID526957756',
    country = "USA"
)

store_payment.set_cc_information(
    card_number = "4111111111111111",
    card_expire = "1225",
    cvv2 = "123"
)

store_payment.auth(
    amount = "0.00",
    new_customer_token = True # True generates random string. Other values will be used literally
) 

# Makes the API Request
store_payment.process()

# Try again if we accidentally create a non-unique token
if "Customer Tokens must be unique" in store_payment.message_response:
    store_payment.auth(
        amount = "0.00",
        new_customer_token = True
    )

    store_payment.process()

# Read response from BluePay
if store_payment.is_successful_response():
    payment = BluePay(
        account_id = account_id, 
        secret_key = secret_key, 
        mode = mode
    )

    payment.sale(
        amount = "3.99",
        customer_token = store_payment.cust_token_response
    ) 

    payment.process()

    if payment.is_successful_response():
        print('Transaction Status: ' + payment.status_response)
        print('Transaction Message: ' + payment.message_response)
        print('Transaction ID: ' + payment.trans_id_response)
        print('AVS Response: ' + payment.avs_code_response)
        print('CVV2 Response: ' + payment.cvv2_code_response)
        print('Masked Payment Account: ' + payment.masked_account_response)
        print('Card Type: ' + payment.card_type_response)
        print('Auth Code: ' + payment.auth_code_response)
        #print('Stored ID: ' + payment.stored_id_response)
    else:
        print(payment.message_response)
else:
    print(store_payment.message_response)
