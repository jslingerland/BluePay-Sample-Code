require_relative "bluepay.rb"


ACCOUNT_ID = "Merchant's Account ID Here"
SECRET_KEY = "Merchant's Secret Key Here"
MODE = "TEST"

test_url = BluePay.new(
  account_id: ACCOUNT_ID,  
  secret_key: SECRET_KEY,  
  mode: MODE
)

generated_url = test_url.generate_url(
merchant_name: "Test Merchant",
return_url: "www.google.com",
transaction_type: "SALE", 
accept_discover: "Yes", 
accept_amex: "Yes", 
amount: "99.99", 
protect_amount: "Yes",
rebilling: "Yes",
reb_protect: "Yes", 
reb_amount: "50", 
reb_cycles: "12",
reb_start_date: "1 MONTH",
reb_frequency: "1 MONTH",
custom_id: "MyCustomID1.1234",
protect_custom_id: "Yes",
custom_id2: "MyCustomID2.12345678910",
protect_custom_id2: "Yes",
payment_template: "mobileform01", 
receipt_template: "defaultres2", 
receipt_temp_remote_url: ""
 )

puts "Hosted Payment Form URL: " + generated_url