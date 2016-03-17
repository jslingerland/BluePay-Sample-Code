##
# BluePay Ruby Sample code.
#
# This code sample runs a $0.00 Credit Card Auth transaction
# against a customer using test payment information, sets up
# a rebilling cycle, and also shows how to cancel that rebilling cycle.
# See comments below on the details of the initial setup of the
# rebilling cycle.
##

require_relative "../../lib/bluepay.rb"

ACCOUNT_ID = "Merchant's Account ID Here"
SECRET_KEY = "Merchant's Secret Key Here"
MODE = "TEST" # Transaction Mode (can also be "LIVE")

rebill = BluePay.new(
  account_id: ACCOUNT_ID,  
  secret_key: SECRET_KEY,  
  mode: MODE
)

# Set Customer Information
rebill.set_customer_information(
  first_name: "Bob", 
  last_name: "Tester",
  address1: "123 Test St.", 
  address2: "Apt #500", 
  city: "Testville", 
  state: "IL", 
  zip_code: "54321", 
  country: "USA",
  phone: "123-123-1234",  
  email: "test@bluepay.com"  
)

# Set Credit Card Information
rebill.set_cc_information(
  cc_number: "4111111111111111", # Customer Credit Card Number
  cc_expiration: "1215", # Card Expiration Date: MMYY
  cvv2: "123" # Card CVV2
)

rebill.set_recurring_payment(
  reb_first_date: "2015-01-01", # Rebill Start Date: Jan. 1, 2015
  reb_expr: "1 MONTH", # Rebill Frequency: 1 MONTH
  reb_cycles: "12", # Rebill # of Cycles: 12
  reb_amount: "15.00" # Rebill Amount: $15.00
)

rebill.auth(amount: "0.00") 

# Makes the API Request to create a rebill
rebill.process

# If transaction was approved..
if rebill.successful_response?
  
  rebill_cancel = BluePay.new(
    account_id: ACCOUNT_ID,  
    secret_key: SECRET_KEY,  
    mode: MODE
  )

  # Find rebill by id and cancel rebilling cycle 
  rebill_cancel.cancel_rebilling_cycle(rebill.get_rebill_id)

  # Makes the API request to cancel the rebill
  rebill_cancel.process

  # Reads the response from BluePay
  puts "REBILL STATUS: " + rebill_cancel.get_rebill_status
  puts "REBILL ID: " + rebill_cancel.get_reb_id
  puts "REBILL CREATION DATE: " + rebill_cancel.get_creation_date
  puts "REBILL NEXT DATE: " + rebill_cancel.get_next_date
  puts "REBILL SCHEDULE EXPRESSION: " + rebill_cancel.get_sched_expression
  puts "REBILL CYCLES REMAINING: " + rebill_cancel.get_cycles_remaining
  puts "REBILL AMOUNT: " + rebill_cancel.get_rebill_amount
  puts "REBILL NEXT AMOUNT: " + rebill_cancel.get_next_amount
else
  puts rebill.get_message
end
