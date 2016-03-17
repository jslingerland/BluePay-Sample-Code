##
# BluePay Ruby Sample code.
#
# This code sample runs a report that grabs data from the
# BluePay gateway based on certain criteria. This will ONLY return
# transactions that have already settled. See comments below
# on the details of the report.
# If using TEST mode, only TEST transactions will be returned.

require_relative "../../lib/bluepay.rb"

acct_id = "Merchant's Account ID Here"
sec_key = "Merchant's Secret Key Here"
trans_mode = "TEST" # Transaction Mode (can also be "LIVE")

report = BluePay.new(
  account_id: acct_id, 
  secret_key: sec_key, 
  mode: trans_mode 
)

report.get_settled_transaction_report(
	report_start_date: '2015-01-01', #YYYY-MM-DD
	report_end_date: '2015-04-30', #YYYY-MM-DD
	query_by_hierarchy: '1', # Also search subaccounts? Yes
	do_not_escape: '1', # Output response without commas? Yes
	exclude_errors: '1' # Do not include errored transactions? Yes
)

# Makes the API request with BluePay
response = report.process

# Reads the response from BluePay
puts response
