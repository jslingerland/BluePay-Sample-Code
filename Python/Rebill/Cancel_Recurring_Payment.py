##
# BluePay Python Sample code.
#
# This code sample runs a $0.00 Credit Card Auth transaction
# against a customer using test payment information, sets up
# a rebilling cycle, and also shows how to cancel that rebilling cycle.
# See comments below on the details of the initial setup of the
# rebilling cycle.
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
    card_expire = "1215",
    cvv2 = "123"
)

# Set Recurring Payment Information
payment.set_rebilling_information(
  reb_first_date = "2015-01-05", # Rebill Start Date: Jan. 5, 2015
  reb_expr = "1 MONTH", # Rebill Frequency: 1 MONTH
  reb_cycles = "5", # Rebill # of Cycles: 5
  reb_amount = "3.50" # Rebill Amount: $3.50
)

# Auth Amount: $0.00
payment.auth(amount = '0.00')

# Makes the API Request for the recurring payment
payment.process()

# If transaction was approved..
if payment.is_successful_response():

    rebill_cancel = BluePay(
        account_id = account_id, # Merchant's Account ID
        secret_key = secret_key, # Merchant's Secret Key
        mode = mode # Transaction Mode: TEST (can also be LIVE)
    )

    # Find rebill by id and cancel rebilling cycle
    rebill_cancel.cancel_rebill(payment.reb_id_response)

    # Makes the API request to cancel the rebill
    rebill_cancel.process()

    # Read response from BluePay
    print('Rebill Status: ' + rebill_cancel.rebill_status_response)
    print('Rebill ID: ' + rebill_cancel.rebill_id_response)
    print('Rebill Creation Date: ' + rebill_cancel.creation_date_response)
    print('Rebill Next Date: ' + rebill_cancel.next_date_response)
    print('Rebill Schedule Expression: ' + rebill_cancel.sched_expression_response)
    print('Rebill Cycles Remaining: ' + rebill_cancel.cycles_remaining_response)
    print('Rebill Amount: ' + rebill_cancel.rebill_amount_response)
else:
    print(rebill_cancel.message_response)