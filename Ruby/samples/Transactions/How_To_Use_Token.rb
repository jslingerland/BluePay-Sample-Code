##
# BluePay Ruby Sample code.
#
# Charges a customer $3.00 using the payment and customer information from a previous transaction. 
# If using TEST mode, odd dollar amounts will return
# an approval and even dollar amounts will return a decline.
##

require_relative "../../lib/bluepay.rb"

ACCOUNT_ID = "Merchant's Account ID Here"
SECRET_KEY = "Merchant's Secret Key Here"
MODE = "TEST"  
TOKEN = "Transaction ID here" 

payment = BluePay.new(
  account_id: ACCOUNT_ID,  
  secret_key: SECRET_KEY,  
  mode: MODE
)

#payment.set_stored_parameters(
#  stored_indicator: "F",
#  stored_type: "C",
#  stored_id: "TESTID526957756"
#)

payment.sale(
  amount: "3.00", 
  trans_id: TOKEN 
) 

# Makes the API Request
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
