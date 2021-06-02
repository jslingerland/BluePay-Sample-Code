##
# BluePay Ruby Sample code.
#
# This code sample runs a $0.00 Credit Card Authorization.
# This stores the customer's payment information securely in
# BluePay to be used for further transactions.
##

require_relative "../../lib/bluepay.rb"

ACCOUNT_ID = "Merchant's Account ID Here"
SECRET_KEY = "Merchant's Secret Key Here"
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
  #stored_indicator: "F",
  #stored_type: "C",
  #stored_id: "TESTID526957756",
  email: "test@bluepay.com"
)

payment.set_cc_information(
  cc_number: "4111111111111111", # Customer Credit Card Number
  cc_expiration: "1225", # Card Expiration Date: MMYY
  cvv2: "123" # Card CVV2
)

payment.auth(amount: "0.00") # Card Authorization amount: $0.00

# Makes the API request with BluePay
payment.process

# If transaction was successful reads the responses from BluePay
if payment.successful_transaction?  
  puts "TRANSACTION STATUS: " + payment.get_status
  puts "TRANSACTION MESSAGE: " + payment.get_message
  puts "TRANSACTION ID: " + payment.get_trans_id
  puts "AVS RESPONSE: " + payment.get_avs_code
  puts "CVV2 RESPONSE: " + payment.get_cvv2_code
  puts "MASKED PAYMENT ACCOUNT: " + payment.get_masked_account
  puts "CARD TYPE: " + payment.get_card_type
  puts "AUTH CODE: " + payment.get_auth_code
  #puts "STORED ID: " + payment.get_stored_id.to_s

else
  puts payment.get_message
end
