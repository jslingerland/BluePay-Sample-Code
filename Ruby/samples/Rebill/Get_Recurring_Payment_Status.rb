##
# BluePay Ruby Sample code.
#
# This code sample runs a $0.00 Credit Card Auth transaction
# against a customer using test payment information.
# Once the rebilling cycle is created, this sample shows how to
# get information back on this rebilling cycle.
# See comments below on the details of the initial setup of the
# rebilling cycle.
##

require_relative "../../lib/bluepay.rb"

ACCOUNT_ID = "Merchant's Account ID Here"
SECRET_KEY = "Merchant's Secret Key Here"
MODE = "TEST"  

rebill = BluePay.new(
  account_id: ACCOUNT_ID,  
  secret_key: SECRET_KEY,  
  mode: MODE
)

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

rebill.set_cc_information(
  cc_number: "4111111111111111", # Customer Credit Card Number
  cc_expiration: "1215", # Card Expiration Date: MMYY
  cvv2: "123" # Card CVV2
)

rebill.set_recurring_payment(
  reb_first_date: "2015-01-05", # Rebill Start Date: Jan. 5, 2015
  reb_expr: "1 MONTH", # Rebill Frequency: 1 MONTH
  reb_cycles: "11", # Rebill # of Cycles: 11
  reb_amount: "15.00" # Rebill Amount: $15.00
)

# Sets a Card Authorization at $0.00
rebill.auth(amount: "0.00") 

# Makes the API Request to create a recurring payment
rebill.process

# If transaction was successful..
if rebill.successful_transaction?
  
  rebill_status = BluePay.new(
    account_id: ACCOUNT_ID,  
    secret_key: SECRET_KEY,  
    mode: MODE
  )

  # Find the rebill by ID and get rebill status 
  rebill_status.get_rebilling_cycle_status(rebill.get_rebill_id)

  # Makes the API Request to get the rebill status
  rebill_status.process

  # Reads the response from BluePay
  puts "REBILL STATUS: " + rebill_status.get_rebill_status
  puts "REBILL ID: " + rebill_status.get_reb_id
  puts "REBILL CREATION DATE: " + rebill_status.get_creation_date
  puts "REBILL NEXT DATE: " + rebill_status.get_next_date
  puts "REBILL LAST DATE: " + rebill_status.get_last_date
  puts "REBILL SCHEDULE EXPRESSION: " + rebill_status.get_sched_expression
  puts "REBILL CYCLES REMAINING: " + rebill_status.get_cycles_remaining
  puts "REBILL AMOUNT: " + rebill_status.get_rebill_amount
  puts "REBILL NEXT AMOUNT: " + rebill_status.get_next_amount
else
  puts rebill.get_message
end