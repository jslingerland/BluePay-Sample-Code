##
# BluePay Ruby Sample code.
#
# This code sample runs a report that grabs a single transaction
# from the BluePay gateway based on certain criteria.
# See comments below on the details of the report.
# If using TEST mode, only TEST transactions will be returned.
##
from __future__ import print_function
import os.path, sys
sys.path.append(os.path.join(os.path.dirname(os.path.realpath(__file__)), os.pardir))
from BluePay import BluePay

account_id = "Merchant's Account ID Here"
secret_key = "Merchant's Secret Key Here"
mode = "TEST"  

report = BluePay(
    account_id = account_id, 
    secret_key = secret_key, 
    mode = mode 
)

report.get_single_trans_query(
    transaction_id = '100532208235', # Transaction ID
    report_start = "2015-01-01", # Query Start Date: Jan. 1, 2013
    report_end = "2018-12-31", # Query End Date: Jan. 15, 2015
    exclude_errors =  "1" # Do not include errored transactions? Yes
 ) 

# Makes the API Request with BluePay
report.process()

# Reads the response from BluePay
print(report.response)