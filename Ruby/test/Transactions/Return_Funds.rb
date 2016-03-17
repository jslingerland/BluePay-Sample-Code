##
# BluePay Ruby Sample code.
#
# This code sample runs a $3.00 Credit Card Sale transaction
# against a customer using test payment information. If
# approved, a 2nd transaction is run to partially refund the 
# customer for $1.75 of the $3.00.
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
  cc_expiration: "1216", # Card Expiration Date: MMYY
  cvv2: "123" # Card CVV2
)

payment.sale(amount: "3.00") # Sale Amount: $3.00

# Makes the API Request for processing the sale
payment.process

# If transaction was approved..
if payment.successful_response?  

  payment_return = BluePay.new(
    account_id: acct_id,  
    secret_key: sec_key,  
    mode: trans_mode
  )

  # Creates a refund transaction against previous sale
  payment_return.refund(
    trans_id: payment.get_trans_id, # id of previous transaction to refund
    amount: "1.75" # partial refund of $1.75
  )

  # Makes the API Request to process refund
  payment_return.process

  # Reads the response from BluePay
  puts "TRANSACTION STATUS: " + payment_return.get_status
  puts "TRANSACTION MESSAGE: " + payment_return.get_message
  puts "TRANSACTION ID: " + payment_return.get_trans_id
  puts "AVS RESPONSE: " + payment_return.get_avs_code
  puts "CVV2 RESPONSE: " + payment_return.get_cvv2_code
  puts "MASKED PAYMENT ACCOUNT: " + payment_return.get_masked_account
  puts "CARD TYPE: " + payment_return.get_card_type
  puts "AUTH CODE: " + payment_return.get_auth_code
else
  puts payment.get_message
end
