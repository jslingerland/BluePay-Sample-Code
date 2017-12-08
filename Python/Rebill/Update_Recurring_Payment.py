##
# BluePay Python Sample code.
#
# This code sample runs a $0.00 Credit Card Auth transaction
# against a customer using test payment information.
# Once the rebilling cycle is created, this sample shows how to
# update the rebilling cycle. See comments below
# on the details of the initial setup of the rebilling cycle as well as the
# updated rebilling cycle.
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
    addr1 = "1234 Test St.",
    addr2 = "Apt #500",
    city = "Testville",
    state = "IL",
    zipcode = "54321",
    country = "USA"
)

rebill.set_cc_information(
    card_number = "4111111111111111",
    card_expire = "1225",
    cvv2 = "123"
)

# Set Recurring Payment Information
rebill.set_rebilling_information(
  reb_first_date = "2015-01-05", # Rebill Start Date: Jan. 5, 2015
  reb_expr = "1 MONTH", # Rebill Frequency: 1 MONTH
  reb_cycles = "5", # Rebill # of Cycles: 5
  reb_amount = "3.50" # Rebill Amount: $3.50
)

# Auth Amount: $0.00
rebill.auth(amount = '0.00')

# Makes the API Request to authorize a recurring payment
rebill.process()

# If transaction was approved..
if rebill.is_successful_response():

    payment_information_update = BluePay(
        account_id = account_id, # Merchant's Account ID
        secret_key = secret_key, # Merchant's Secret Key
        mode = mode # Transaction Mode: TEST (can also be LIVE)
    ) 

    # Sets an updated credit card expiration date
    payment_information_update.set_cc_information(
        cc_expiration = '1229'
    )
    
    # Stores new card expiration date
    payment_information_update.auth(
        amount =  '0.00', 
        trans_id = rebill.trans_id_response # the id of the initial payment to update
    )

    # Makes the API Request to update the payment information
    payment_information_update.process()

    # Creates a request to update the rebill
    rebill_update = BluePay(
        account_id = account_id, # Merchant's Account ID
        secret_key = secret_key, # Merchant's Secret Key
        mode = mode # Transaction Mode: TEST (can also be LIVE)
    )

    # Updates the rebill
    rebill_update.update_rebill(
      rebill_id =  rebill.reb_id_response, # The ID of the rebill to be updated.
      template_id = payment_information_update.reb_id_response, # Updates the payment information portion of the rebilling cycle with the new card expiration date entered above
      reb_next_date = "2015-03-01", # Rebill Start Date: March 1, 2015
      reb_expr = "1 MONTH", # Rebill Frequency: 1 MONTH
      reb_cycles = "8", # Rebill # of Cycles: 8
      reb_amount = "5.15", # Rebill Amount: $5.15
      reb_next_amount = "1.50" # Rebill Next Amount: $1.50
    )
    
    # Makes the API Request to updated the rebill
    rebill_update.process()
    
    # Read response from BluePay
    print('Rebill Status: ' + rebill_update.rebill_status_response)
    print('Rebill ID: ' + rebill_update.rebill_id_response)
    print('Rebill Creation Date: ' + rebill_update.creation_date_response)
    print('Rebill Next Date: ' + rebill_update.next_date_response)
    print('Rebill Schedule Expression: ' + rebill_update.sched_expression_response)
    print('Rebill Cycles Remaining: ' + rebill_update.cycles_remaining_response)
    print('Rebill Amount: ' + rebill_update.rebill_amount_response)
    print('Rebill Next Amount: ' + rebill_update.next_date_response)
else:
    print(rebill.message_response)