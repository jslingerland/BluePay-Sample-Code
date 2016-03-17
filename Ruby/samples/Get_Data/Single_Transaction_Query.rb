##
# BluePay Ruby Sample code.
#
# This code sample runs a report that grabs a single transaction
# from the BluePay gateway based on certain criteria.
# See comments below on the details of the report.
# If using TEST mode, only TEST transactions will be returned.
##

require_relative "../../lib/bluepay.rb"

ACCOUNT_ID = "Merchant's Account ID Here"
SECRET_KEY = "Merchant's Secret Key Here"
MODE = "TEST" 

query = BluePay.new(
  account_id: ACCOUNT_ID,  
  secret_key: SECRET_KEY,  
  mode: MODE
)

query.get_single_transaction_query(
  transaction_id: "Transaction ID here", # required
  report_start_date: '2013-01-01', # YYYY-MM-DD; required
  report_end_date: '2015-05-30', # YYYY-MM-DD; required
  exclude_errors: '1' # Do not include errored transactions? Yes; optional
)

# Makes the API request with BluePay 
query.process

if query.get_id
  # Reads the response from BluePay
  puts 'Transaction ID: ' + query.get_id
  puts 'First Name: ' + query.get_name1
  puts 'Last Name: ' + query.get_name2
  puts 'Payment Type: ' + query.get_payment_type
  puts 'Transaction Type: ' + query.get_trans_type
  puts 'Amount: ' + query.get_amount
else
  puts response
end

