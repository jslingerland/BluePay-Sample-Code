##
# BluePay Ruby Sample code.
#
# This code sample creates a recurring payment charging $15.00 per month for one year.
##

require_relative "../../lib/bluepay.rb"

ACCOUNT_ID = "Merchant's Account ID Here"
SECRET_KEY = "Merchant's Secret Key Here"
MODE = "TEST"  

rebill = BluePay.new(
  account_id: ACCOUNT_ID ,  
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

rebill.set_ach_information(
  ach_routing: "123123123", # Routing Number: 123123123
  ach_account: "123456789", # Account Number: 123456789
  ach_account_type: 'C', # Account Type: Checking
  doc_type: "WEB" # ACH Document Type: WEB
)

rebill.set_recurring_payment(
  reb_first_date: "2015-01-01", # Rebill Start Date: Jan. 1, 2015
  reb_expr: "1 MONTH", # Rebill Frequency: 1 MONTH
  reb_cycles: "12", # Rebill # of Cycles: 12
  reb_amount: "15.00" # Rebill Amount: $15.00
)

rebill.auth(amount: "0.00") 

# Makes the API Request with BluePay
rebill.process

# If transaction was successful reads the responses from BluePay
if rebill.successful_transaction?
  puts "TRANSACTION ID: " + rebill.get_trans_id
  puts "REBILL ID: " + rebill.get_rebill_id
  puts "TRANSACTION STATUS: " + rebill.get_status
  puts "TRANSACTION MESSAGE: " + rebill.get_message
  puts "MASKED PAYMENT ACCOUNT: " + rebill.get_masked_account
  puts "CUSTOMER BANK NAME: " + rebill.get_bank_name
else
  puts rebill.get_message
end