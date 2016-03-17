##
# BluePay Ruby Sample code.
#
# This code sample creates a recurring payment charging $15.00 per month for one year.

require_relative "../../lib/bluepay.rb"

acct_id = "Merchant's Account ID Here"
sec_key = "Merchant's Secret Key Here"
trans_mode = "TEST" # Transaction Mode (can also be "LIVE")

rebill = BluePay.new(
  account_id: acct_id,   
  secret_key: sec_key,   
  mode: trans_mode
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
  reb_first_date: "2015-01-01", # Rebill Start Date: Jan. 1, 2015
  reb_expr: "1 MONTH", # Rebill Frequency: 1 MONTH
  reb_cycles: "12", # Rebill # of Cycles: 12
  reb_amount: "15.00" # Rebill Amount: $15.00
)

rebill.auth(amount: "0.00") 

# Makes the API Request
rebill.process

# if transaction was successful
if rebill.successful_response?
  # Reads the response from BluePay
  puts "TRANSACTION ID: " + rebill.get_trans_id
  puts "REBILL ID: " + rebill.get_rebill_id
  puts "TRANSACTION STATUS: " + rebill.get_status
  puts "AVS RESPONSE: " + rebill.get_avs_code
  puts "CVV2 RESPONSE: " + rebill.get_cvv2_code
  puts "MASKED PAYMENT ACCOUNT: " + rebill.get_masked_account
  puts "CARD TYPE: " + rebill.get_card_type
  puts "AUTH CODE: " + rebill.get_auth_code
else
  puts rebill.get_message
end
