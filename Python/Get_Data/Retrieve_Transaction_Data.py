##
# BluePay Python Sample code.
#
# This code sample runs a report that grabs data from the
# BluePay gateway based on certain criteria.
# If using TEST mode, only TEST transactions will be returned.
##

import os.path, sys
sys.path.append(os.path.join(os.path.dirname(os.path.realpath(__file__)), os.pardir))
from BluePay import BluePay

account_id = "100228390579"
secret_key = "AKGIF9X9WT9CLQCWDFONC8N3HXRL9Y5K"
mode = "TEST"

report = BluePay(
    account_id = account_id, # Merchant's Account ID
    secret_key = secret_key, # Merchant's Secret Key
    mode = mode # Transaction Mode: TEST (can also be LIVE)
)

report.get_transaction_report(
    report_start = '2015-01-01', # Report Start Date: Jan. 1, 2015
    report_end = '2015-04-30', # Report End Date: Aprl. 30, 2015
    subaccounts_searched = '1', # Also search subaccounts? Yes
    do_not_escape = '1', # Output response without commas? Yes
    exclude_errors = '1' # Do not include errored transactions? Yes
)

# Makes the API request to BluePay
report.process()

# Reads the response from BluePay
print report.response

