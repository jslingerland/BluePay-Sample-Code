##
# BluePay Ruby Sample code.
#
# This code sample runs a $25.00 Credit Card Sale transaction
# against a customer using test payment information.
# Optional transaction data is also sent.
# If using TEST mode, odd dollar amounts will return
# an approval and even dollar amounts will return a decline.
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

payment.set_cc_information(
  cc_number: "4111111111111111", # Customer Credit Card Number
  cc_expiration: "1215", # Card Expiration Date: MMYY
  cvv2: "123" # Card CVV2
)

# Optional fields users can set
payment.custom_id1 = "12345" # Custom ID1: 12345
payment.custom_id2 = "09866" # Custom ID2: 09866
payment.invoice_id = "500000" # Invoice ID: 50000
payment.order_id = "10023145" # Order ID: 10023145
payment.amount_tip = "6.00" # Tip Amount: $6.00
payment.amount_tax = "3.50" # Tax Amount: $3.50
payment.amount_food = "3.11" # Food Amount: $3.11
payment.amount_misc = "5.00" # Miscellaneous Amount: $5.00
payment.memo = "Enter any comments about the transaction here." # Comments

payment.sale(amount: "25.00") # Sale Amount: $25.00

# Makes the API request with BluePay
payment.process

# If transaction was approved..
if payment.successful_response?
  # Reads the response from BluePay
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
