##
# BluePay Python Sample code.
#
# This code sample runs a $3.00 Credit Card Sale transaction
# against a customer using test payment information.
# If approved, a 2nd transaction is run to cancel this transaction.
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

# Makes the API Request for processing the sale
payment.process()


# If transaction was approved..
if payment.is_successful_response():

    payment_cancel = BluePay(
        account_id = account_id, # Merchant's Account ID
        secret_key = secret_key, # Merchant's Secret Key
        mode = mode # Transaction Mode: TEST (can also be LIVE)
    )
    
    # Finds the previous payment by ID and attempts to void it
    payment_cancel.void(payment.trans_id_response) 
    
    # Makes the API Request to void the payment
    payment_cancel.process()

    # Read response from BluePay
    print('Transaction Status: ' + payment_cancel.status_response)
    print('Transaction Message: ' + payment_cancel.message_response)
    print('Transaction ID: ' + payment_cancel.trans_id_response)
    print('Transaction AVS Result: ' + payment_cancel.avs_code_response)
    print('Transaction CVV2 Result: ' + payment_cancel.cvv2_code_response)
    print('Masked Payment Account: ' + payment_cancel.masked_account_response)
    print('Card Type: ' + payment_cancel.card_type_response)
    print('Transaction Auth Code: ' + payment_cancel.auth_code_response)
else:
    print(payment.message_response)