##
# BluePay Ruby Sample code.
#
# This code sample runs a $3.00 Credit Card Sale transaction
# against a customer using test payment information.
# If approved, a 2nd transaction is run to cancel this transaction.
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
  address1: "1234 Test St.", 
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


payment.sale(amount: "3.00") # Sale Amount: $3.00

# Makes the API Request to create a sale 
payment.process

# If transaction was approved..
if payment.successful_response?

  payment_void = BluePay.new(
    account_id: acct_id,  
    secret_key: sec_key,  
    mode: trans_mode
  )

  # Finds the previous payment by ID and attempts to void it
  payment_void.void(payment.get_trans_id)
  
  # Makes the API Request to void the payment
  payment_void.process

  # Reads the response from BluePay
  puts "TRANSACTION STATUS: " + payment_void.get_status
  puts "TRANSACTION MESSAGE: " + payment_void.get_message
  puts "TRANSACTION ID: " + payment_void.get_trans_id
  puts "AVS RESPONSE: " + payment_void.get_avs_code
  puts "CVV2 RESPONSE: " + payment_void.get_cvv2_code
  puts "MASKED PAYMENT ACCOUNT: " + payment_void.get_masked_account
  puts "CARD TYPE: " + payment_void.get_card_type
  puts "AUTH CODE: " + payment_void.get_auth_code
else
  puts payment.get_message
end
