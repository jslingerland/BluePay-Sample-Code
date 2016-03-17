##
# BluePay Ruby Sample code.
#
# This code sample runs a report that grabs data from the
# BluePay gateway based on certain criteria.
# If using TEST mode, only TEST transactions will be returned.
##

require_relative "../../lib/bluepay.rb"

ACCOUNT_ID = "Merchant's Account ID Here"
SECRET_KEY = "Merchant's Secret Key Here"
MODE = "TEST" 

report = BluePay.new(
  account_id: ACCOUNT_ID, 
  secret_key: SECRET_KEY, 
  mode: MODE 
)

report.get_transaction_report(
  report_start_date: '2015-01-01', #YYYY-MM-DD
  report_end_date: '2015-04-30', #YYYY-MM-DD
  query_by_hierarchy: '1', # Also search subaccounts? Yes
  do_not_escape: '1', # Output response without commas? Yes
  exclude_errors: '1' # Do not include errored transactions? Yes
)

# Makes the API request with BluePay
report.process

# Reads the response from BluePay
puts report.get_response