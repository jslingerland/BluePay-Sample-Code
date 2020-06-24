import sys
from BluePay import BluePay

account_id = "Merchant's Account ID Here"
secret_key = "Merchant's Secret Key Here"
mode = "TEST" # Transaction Mode: TEST (can also be LIVE)

test = BluePay(
 account_id = account_id, 
 secret_key = secret_key, 
 mode = mode 
)

generated_url = test.generate_url(
merchant_name = 'Test Merchant',
return_url = 'www.google.com',
transaction_type = 'SALE', 
accept_discover = 'Yes', 
accept_amex = 'Yes', 
amount = '99.99', 
protect_amount = 'Yes',
rebilling = 'Yes',
reb_protect = 'Yes', 
reb_amount = '50', 
reb_cycles = '12',
reb_start_date = '1 MONTH',
reb_frequency = '1 MONTH',
custom_id = 'MyCustomID1.1234',
protect_custom_id = 'Yes',
custom_id2 = 'MyCustomID2.12345678910',
protect_custom_id2 = 'Yes',
payment_template = 'mobileform01', 
receipt_template = 'defaultres2', 
receipt_temp_remote_url = ''
)

print "Hosted Payment Form URL: " + generated_url