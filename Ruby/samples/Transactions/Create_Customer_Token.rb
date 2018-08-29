##
# BluePay Ruby Sample code.
#
# This code sample runs a $0.00 AUTH transaction
# and creates a customer token using test payment information,
# which is then used to run a separate $3.99 sale.
##

require_relative "../../lib/bluepay.rb"

ACCOUNT_ID = "Merchant's Account ID here"
SECRET_KEY = "Merchant's Secret Key here"
MODE = "TEST"  

auth = BluePay.new(
  account_id: ACCOUNT_ID,  
  secret_key: SECRET_KEY,  
  mode: MODE
)

auth.set_customer_information(
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

auth.set_cc_information(
  cc_number: "4111111111111111", # Customer Credit Card Number
  cc_expiration: "1225", # Card Expiration Date: MMYY
  cvv2: "123" # Card CVV2
)

auth.auth(
  amount: "0.00", # Card Authorization amount: $0.00
  new_customer_token: true # Token must be 6-16 alphanumeric characters. True generates random string, other values are used as token value
) 

# Makes the API request with BluePay
auth.process

# Try again if we accidentally create a non-unique token
if auth.get_message.include?("Customer Tokens must be unique")
  auth.auth(
    amount: "0.00",
    new_customer_token: true
  ) 
  auth.process
end

# If transaction was successful reads the responses from BluePay
if auth.successful_transaction?  
  payment = BluePay.new(
    account_id: ACCOUNT_ID,  
    secret_key: SECRET_KEY,  
    mode: MODE
  )

  payment.sale(
    amount: "3.00",
    customer_token: auth.get_cust_token
  )

  payment.process

  if payment.successful_transaction?
      puts "TRANSACTION STATUS: " + payment.get_status
      puts "TRANSACTION MESSAGE: " + payment.get_message
      puts "TRANSACTION ID: " + payment.get_trans_id
      puts "AVS RESPONSE: " + payment.get_avs_code
      puts "CVV2 RESPONSE: " + payment.get_cvv2_code
      puts "MASKED PAYMENT ACCOUNT: " + payment.get_masked_account
      puts "CARD TYPE: " + payment.get_card_type
      puts "AUTH CODE: " + payment.get_auth_code
      puts "CUSTOMER TOKEN: " + payment.get_cust_token
  else
    puts payment.get_message
  end
else
  puts auth.get_message
end