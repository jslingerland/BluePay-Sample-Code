##
# BluePay Ruby Sample code.
#
# This code sample shows a very based approach
# on handling data that is posted to a script running
# a merchant's server after a transaction is processed
# through their BluePay gateway account.
##

require_relative "../../lib/bluepay.rb"
require "cgi"

vars = CGI.new

secret_key = ""

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

# Calculate expected bp_stamp
bp_stamp = BluePay.calc_trans_notify_tps(
  secret_key,
  trans_id,
  trans_status,
  trans_type,
  amount,
  batch_id,
  batch_status,
  total_count,
  total_amount,
  batch_upload_id,
  rebill_id,
  rebill_amount,
  rebill_status
)

# check if expected bp_stamp = actual bp_stamp
if bp_stamp == vars["bp_stamp"]

  # Reads the response from BluePay
  puts 'Transaction ID: ' + trans_id
  puts 'Transaction Status: ' + trans_status
  puts  'Transaction Type: ' + trans_type
  puts  'Transaction Amount: ' + amount
  puts  'Rebill ID: ' + rebill_id
  puts  'Rebill Amount: ' + rebill_amount
  puts  'Rebill Status: ' + rebill_status
else
  puts 'ERROR IN RECEIVING DATA FROM BLUEPAY'
end