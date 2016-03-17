##
# BluePay Ruby Sample code.
#
# This code sample runs a $0.00 Credit Card Auth transaction
# against a customer using test payment information.
# Once the rebilling cycle is created, this sample shows how to
# update the rebilling cycle. See comments below
# on the details of the initial setup of the rebilling cycle as well as the
# updated rebilling cycle.
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
  address1: "1234 Test St.", 
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
  reb_cycles: "5", # Rebill # of Cycles: 5
  reb_amount: "3.50" # Rebill Amount: $3.50
)

# Sets a Card Authorization at $0.00
rebill.auth(amount: "0.00") 

# Makes the API Request to create a rebill
rebill.process

# If transaction was approved..
if rebill.successful_transaction?

  payment_information_update = BluePay.new(
    account_id: ACCOUNT_ID,  
    secret_key: SECRET_KEY,  
    mode: MODE  
  )

  # Sets an updated credit card expiration date
  payment_information_update.set_cc_information(
    cc_expiration: "0121" # Card Expiration
  )

  # Stores new card expiration date
  payment_information_update.auth(
    amount: "0.00", 
    trans_id: rebill.get_trans_id # the id of the rebill to update
  )
  
  # Makes the API Request to update the payment information
  payment_information_update.process

  # Creates a request to update the rebill
  rebill_update = BluePay.new(
    account_id: ACCOUNT_ID,  
    secret_key: SECRET_KEY,  
    mode: MODE  
  )

  # Updates the rebill
  rebill_update.update_rebill(
    rebill_id: rebill.get_rebill_id, # The ID of the rebill to be updated.  
    template_id: payment_information_update.get_trans_id, # Updates the payment information portion of the rebilling cycle with the new card expiration date entered above 
    next_date: "2015-03-01", # Rebill Start Date: March 1, 2015
    reb_expr: "1 MONTH", # Rebill Frequency: 1 MONTH
    reb_cycles: "8", # Rebill # of Cycles: 8
    reb_amount: "5.15", # Rebill Amount: $5.15
    next_amount: "1.50" # Rebill Next Amount: $1.50
  )
  
  # Makes the API Request to update the rebill
  rebill_update.process

  # Reads the response from BluePay
  puts "REBILL STATUS: " + rebill_update.get_rebill_status
  puts "REBILL ID: " + rebill_update.get_reb_id
  puts "REBILL CREATION DATE: " + rebill_update.get_creation_date
  puts "REBILL NEXT DATE: " + rebill_update.get_next_date
  puts "REBILL LAST DATE: " + rebill_update.get_last_date
  puts "REBILL SCHEDULE EXPRESSION: " + rebill_update.get_sched_expression
  puts "REBILL CYCLES REMAINING: " + rebill_update.get_cycles_remaining
  puts "REBILL AMOUNT: " + rebill_update.get_rebill_amount
  puts "REBILL NEXT AMOUNT: " + rebill_update.get_next_amount
else
  puts rebill.get_message
end