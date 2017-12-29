##
# BluePay Ruby Sample code.
#
# This code sample runs a Credit Card sales transaction, 
# including sample Level 2 and 3 processing information,
# against a customer using test payment information.
# If using TEST mode, odd dollar amounts will return
# an approval and even dollar amounts will return a decline.
##

require_relative "../../lib/bluepay.rb"

ACCOUNT_ID = "Merchant's Account ID here"
SECRET_KEY = "Merchant's Secret Key here"
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

payment.set_cc_information(
  cc_number: "4111111111111111", # Customer Credit Card Number
  cc_expiration: "1225", # Card Expiration Date: MMYY
  cvv2: "123" # Card CVV2
)

# Set Level 2 Information
payment.invoice_id = "123456789"
payment.amount_tax = "0.91"

# Set Level 3 line item information. Repeat for each item up to 99.
payment.add_line_item(
  quantity: "1", # The number of units of item. Max: 5 digits
  unit_cost: "3.00", # The cost per unit of item. Max: 9 digits decimal
  descriptor: "test1", # Description of the item purchased. Max: 26 character
  commodity_code: "123412341234", # Commodity Codes can be found at http://www.census.gov/svsd/www/cfsdat/2002data/cfs021200.pdf. Max: 12 characters
  product_code: "432143214321", # Merchant-defined code for the product or service being purchased. Max: 12 characters 
  measure_units: "EA", # The unit of measure of the item purchase. Normally EA. Max: 3 characters
  tax_rate: "7%", # Tax rate for the item. Max: 4 digits
  tax_amount: "0.21", # Tax amount for the item. unit_cost * quantity * tax_rate = tax_amount. Max: 9 digits.
  item_discount: "0.00", # The amount of any discounts on the item. Max: 12 digits.
  line_item_total: "3.21" # The total amount for the item including taxes and discounts.
)

payment.add_line_item(
  quantity: "2", 
  unit_cost: "5.00", 
  descriptor: "test2", 
  commodity_code: "123412341234", 
  product_code: "098709870987", 
  measure_units: "EA", 
  tax_rate: "7%",
  tax_amount: "0.70", 
  item_discount: "0.00", 
  line_item_total: "10.70"
)

payment.sale(amount: "13.91") # Sale Amount: $13.91

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

