##
# BluePay Python Sample code.
#
# This code sample runs a $15.00 Credit Card Sale transaction
# against a customer using test payment information.
# Optional transaction data is also sent.
# If using TEST mode, odd dollar amounts will return
# an approval and even dollar amounts will return a decline.
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

# Optional fields users can set
payment.phone = "1231231234" # Phone #
payment.email = "test@bluepay.com" # Email Address
payment.custom_id1 = "12345" # Custom ID1
payment.custom_id2 = "09866" # Custom ID2
payment.invoice_id = "500000" # Invoice ID
payment.order_id = "10023145" # Order ID
payment.amount_tip = "6.00" # Tip Amount
payment.amount_tax = "3.50" # Tax Amount
payment.amount_food = "3.11" # Food Amount
payment.amount_misc = "5.00" # Miscellaneous Amount
payment.memo = "Enter any comments about the transaction here." # Comments

# Sale Amount: $25.00
payment.sale(amount = "25.00") 

# Makes the API Request to process sale
payment.process() 

if payment.is_successful_response(): # Read response from BluePay
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