##
     # BluePay Python Sample code.
     #
     # This code sample shows a very based approach
     # on handling data that is posted to a script running
     # a merchant's server after a transaction is processed
     # through their BluePay gateway account.
    ##
from __future__ import print_function
import os.path, sys
sys.path.append(os.path.join(os.path.dirname(os.path.realpath(__file__)), os.pardir))
from BluePay import BluePay
import cgi

vars = cgi.FieldStorage()

account_id = "Merchant's Account ID Here"
secret_key = "Merchant's Secret Key Here"
mode = "TEST"  

tps = BluePay(
    account_id = account_id, 
    secret_key = secret_key, 
    mode = mode 
)

try:
    # Assign values
    trans_id = vars["trans_id"]
    trans_status = vars["trans_status"]
    trans_type = vars["trans_type"]
    amount = vars["amount"]
    rebill_id = vars["rebill_id"]
    rebill_amount = vars["reb_amount"]
    rebill_status = vars["status"]
    tps_hash_type = vars["TPS_HASH_TYPE"]
    bp_stamp = vars["BP_STAMP"]
    bp_stamp_def = vars["BP_STAMP_DEF"]

    # Calculate expected bp_stamp
    bp_stamp_string = ''
    for field in bp_stamp_def.value.split(' '): 
        bp_stamp_string += vars[field].value 

    expected_stamp = tps.create_tps_hash(bp_stamp_string, tps_hash_type.value).upper()

    # check if expected bp_stamp = actual bp_stamp
    if exepcted_stamp == bp_stamp:

        # Get response from BluePay
        print('Transaction ID: ' + trans_id)
        print('Transaction Status: ' + trans_status)
        print('Transaction Type: ' + trans_type)
        print('Transaction Amount: ' + amount)
        print('Rebill ID: ' + rebill_id)
        print('Rebill Amount: ' + rebill_amount)
        print('Rebill Status: ' + rebill_status)
    else:
        print('ERROR IN RECEIVING DATA FROM BLUEPAY')
except KeyError:
    print("ERROR")