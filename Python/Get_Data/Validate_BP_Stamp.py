#!/usr/bin/python

##
 # BluePay Python Sample code.
 #
 # This code sample reads the values from a BP10emu redirect
 # and authenticates the message using the the BP_STAMP
 # provided in the response. Point the REDIRECT_URL of your 
 # BP10emu request to the location of this script on your server.
##

from __future__ import print_function
import os.path, sys
sys.path.append(os.path.join(os.path.dirname(os.path.realpath(__file__)), os.pardir))
from BluePay import BluePay
import cgi

print("Content-type:text/html\r\n\r\n")
print("<html><head></head><body>")

response = cgi.FieldStorage()

account_id = "Merchant's Account ID Here"
secret_key = "Merchant's Secret Key Here"
mode = "TEST"  


if "BP_STAMP" in response: # Check whether BP_STAMP is provided

    bp = BluePay(
        account_id = account_id, 
        secret_key = secret_key, 
        mode = mode 
    )

    bp_stamp_string = ''
    for field in response["BP_STAMP_DEF"].value.split(' '): # Split BP_STAMP_DEF on whitespace
        bp_stamp_string += response[field].value # Concatenate values used to calculate expected BP_STAMP

    expected_stamp = bp.create_tps_hash(bp_stamp_string, response["TPS_HASH_TYPE"].value).upper() # Calculate expected BP_STAMP using hash function specified in response
    if expected_stamp == response['BP_STAMP'].value: # Compare expected BP_STAMP with received BP_STAMP
        # Validate BP_STAMP and reads the response results
        print("VALID BP_STAMP: TRUE<br/>")
        for key in response.keys():
            print(key + ': ' + response[key].value + "<br/>")
    else:
        print("ERROR: BP_STAMP VALUES DO NOT MATCH<br/>")
    
else:
    print("ERROR: BP_STAMP NOT FOUND. CHECK MESSAGE & RESPONSEVERSION<br/>")

print("</body></html>")