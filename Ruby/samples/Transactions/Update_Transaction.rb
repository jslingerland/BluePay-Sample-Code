##
# BluePay Ruby Sample code.
#
# This code sample runs a $3.00 Credit Card Sale transaction
# against a customer using test payment information. If
# approved, a 2nd transaction is run to update the first transaction 
# to $5.75, $2.75 more than the original $3.00.
# If using TEST mode, odd dollar amounts will return
# an approval and even dollar amounts will return a decline.
##

require_relative "../../lib/bluepay.rb"

ACCOUNT_ID = "Merchant's Account ID"
SECRET_KEY = "Merchant's Secret Key"
MODE = "TEST"  

payment = BluePay.new(
  account_id: ACCOUNT_ID,  
  secret_key: SECRET_KEY,  
  mode: MODE
)

payment.set_customer_information(
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

payment.set_cc_information(
  cc_number: "4111111111111111", # Customer Credit Card Number
  cc_expiration: "1225", # Card Expiration Date: MMYY
  cvv2: "123" # Card CVV2
)

payment.sale(amount: "3.00") # Sale Amount: $3.00

# Makes the API Request for processing the sale
payment.process

# If transaction was approved..
if payment.successful_transaction?  

  payment_update = BluePay.new(
    account_id: ACCOUNT_ID,  
    secret_key: SECRET_KEY,  
    mode: MODE
  )

  # Creates a update transaction against previous sale
  payment_update.update(
    trans_id: payment.get_trans_id, # id of previous transaction to update
    amount: "5.75" # add $2.75 to previous amount
  )

  # Makes the API Request to process update
  payment_update.process

  # Reads the response from BluePay
  puts "TRANSACTION STATUS: " + payment_update.get_status
  puts "TRANSACTION MESSAGE: " + payment_update.get_message
  puts "TRANSACTION ID: " + payment_update.get_trans_id
  puts "AVS RESPONSE: " + payment_update.get_avs_code
  puts "CVV2 RESPONSE: " + payment_update.get_cvv2_code
  puts "MASKED PAYMENT ACCOUNT: " + payment_update.get_masked_account
  puts "CARD TYPE: " + payment_update.get_card_type
  puts "AUTH CODE: " + payment_update.get_auth_code
else
  puts payment_update.get_message
end