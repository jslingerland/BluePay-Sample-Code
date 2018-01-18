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
    batch_id = vars["batch_id"]
    batch_status = vars["batch_status"]
    total_count = vars["total_count"]
    total_amount = vars["total_amount"]
    batch_upload_id = vars["batch_upload_id"]
    rebill_id = vars["rebill_id"]
    rebill_amount = vars["reb_amount"]
    rebill_status = vars["status"]
    tps_hash_type = vars["TPS_HASH_TYPE"]

    # Calculate expected bp_stamp
    bp_stamp = tps.create_tps_hash(
        trans_id +
        trans_status +
        trans_type +
        amount +
        batch_id +
        batch_status +
        total_count +
        total_amount +
        batch_upload_id +
        rebill_id +
        rebill_amount +
        rebill_status,
        tps_hash_type)

    # check if expected bp_stamp = actual bp_stamp
    if bp_stamp == vars["BP_STAMP"]:

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