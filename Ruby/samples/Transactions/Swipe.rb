##
# BluePay Ruby Sample code.
# 
# This code sample runs a $3.00  sales transaction using the payment information obtained from a credit card swipe.
# If using TEST mode, odd dollar amounts will return an approval and even dollar amounts will return a decline.

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
  email: "test@bluepay.com"  
)

# Set payment information for a swiped credit card transaction
payment.swipe("%B4111111111111111^TEST/BLUEPAY^1911101100001100000000667000000?;4111111111111111=191110110000667?")

payment.sale(amount: "3.00") # Sale Amount: $3.00

# Makes the API Request with BluePay
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
else
  puts payment.get_message
end
