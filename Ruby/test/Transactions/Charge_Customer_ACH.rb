##
# BluePay Ruby Sample code.
#
# This code sample runs a $3.00 ACH sales transaction
# against a customer using test payment information.
##

require_relative "../../lib/bluepay.rb"

acct_id = "Merchant's Account ID Here"
sec_key = "Merchant's Secret Key Here"
trans_mode = "TEST" # Transaction Mode (can also be "LIVE")

payment = BluePay.new(
  account_id: acct_id,
  secret_key: sec_key,
  mode: trans_mode
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

payment.set_ach_information(
  ach_routing: "123123123", # Routing Number: 123123123
  ach_account: "123456789", # Account Number: 123456789
  ach_account_type: 'C', # Account Type: Checking
  doc_type: "WEB" # ACH Document Type: WEB
)

payment.sale(amount: "3.00") # Sale Amount: $3.00

# Makes the API Request with BluePay
payment.process

# Reads the response from BluePay
if payment.successful_response?
  puts "TRANSACTION ID: " + payment.get_trans_id
  puts "TRANSACTION STATUS: " + payment.get_status
  puts "TRANSACTION MESSAGE: " + payment.get_message
  puts "MASKED PAYMENT ACCOUNT: " + payment.get_masked_account
  puts "CUSTOMER BANK NAME: " + payment.get_bank_name
else
  puts payment.get_message
end
