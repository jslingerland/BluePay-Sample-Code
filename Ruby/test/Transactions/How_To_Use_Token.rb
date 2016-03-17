##
# BluePay Ruby Sample code.
#
# Charges a customer $3.00 using the payment and customer information from a previous transaction. 
# If using TEST mode, odd dollar amounts will return
# an approval and even dollar amounts will return a decline.
##

require_relative "../../lib/bluepay.rb"

acct_id = "Merchant's Account ID Here"
sec_key = "Merchant's Secret Key Here"
trans_mode = "TEST" # Transaction Mode (can also be "LIVE")
token = "100012341234" # Transaction ID of previous auth or sale here

payment = BluePay.new(
  account_id: acct_id,
  secret_key: sec_key,
  mode: trans_mode
)

payment.sale(
  amount: "3.00", 
  trans_id: token # The transaction ID of a previous sale
) 

# Makes the API Request
payment.process

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
