##
# BluePay Python Sample code.
#
# This code sample runs a $0.00 Credit Card Auth transaction
# against a customer using test payment information.
# Once the rebilling cycle is created, this sample shows how to
# get information back on this rebilling cycle.
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

rebill = BluePay(
    account_id = account_id, 
    secret_key = secret_key, 
    mode = mode
)

rebill.set_customer_information(
    name1 = "Bob",
    name2 = "Tester",
    addr1 = "123 Test St.",
    addr2 = "Apt #501",
    city = "Testville",
    state = "IL",
    zipcode = "54321",
    country = "USA"
)

rebill.set_cc_information(
    card_number = "4111111111111111",
    card_expire = "1215",
    cvv2 = "123"
)

# Set Recurring Payment Information
rebill.set_rebilling_information(
  reb_first_date = "2015-01-01", # Rebill Start Date: Jan. 1, 2015
  reb_expr = "1 MONTH", # Rebill Frequency: 1 MONTH
  reb_cycles = "12", # Rebill # of Cycles: 12
  reb_amount = "15.00" # Rebill Amount: $15.00
)

# Auth Amount: $0.00
rebill.auth(amount = '0.00')

# Makes the API Request for a recurring payment authorization
rebill.process()

# If transaction was approved..
if rebill.is_successful_response():

    rebill_status = BluePay(
        account_id = account_id, # Merchant's Account ID
        secret_key = secret_key, # Merchant's Secret Key
        mode = mode # Transaction Mode: TEST (can also be LIVE)
    )

    # Find the rebill by ID and get rebill status 
    rebill_status.get_rebilling_cycle_status(rebill.reb_id_response)

    # Makes the API Request to get the rebill status
    rebill_status.process()

    # Reads response from BluePay
    print('Rebill Status: ' + rebill_status.rebill_status_response)
    print('Rebill ID: ' + rebill_status.rebill_id_response)
    print('Rebill Creation Date: ' + rebill_status.creation_date_response)
    print('Rebill Next Date: ' + rebill_status.next_date_response)
    print('Rebill Last Date: ' + rebill.last_date_response)
    print('Rebill Schedule Expression: ' + rebill_status.sched_expression_response)
    print('Rebill Cycles Remaining: ' + rebill_status.cycles_remaining_response)
    print('Rebill Amount: ' + rebill_status.rebill_amount_response)
    print('Rebill Next Amount: ' + rebill_status.next_amount_response)
else:
    print(payment.message_response)