#!/usr/bin/ruby

##
# BluePay Ruby Sample code.
#
# This code sample reads the values from a BP10emu redirect
# and authenticates the message using the the BP_STAMP
# provided in the response. Point the REDIRECT_URL of your 
# BP10emu request to the location of this script on your server.
##

print "Content-type:text/html\r\n\r\n"
print "<html><head></head><body>"

require_relative "bluepay.rb"
require "cgi"

ACCOUNT_ID = "Merchant's Account ID Here"
SECRET_KEY = "Merchant's Secret Key Here"
MODE = "TEST" 

response = CGI.new

if response["BP_STAMP"] # Check whether BP_STAMP is provided

  bp = BluePay.new(account_id: ACCOUNT_ID, secret_key: SECRET_KEY, mode: MODE)

  bp_stamp_string = ''
  response["BP_STAMP_DEF"].split(' ').each do |field| # Split BP_STAMP_DEF on whitespace
    bp_stamp_string += response[field] # Concatenate values used to calculate expected BP_STAMP
  end
  expected_stamp = bp.create_tps_hash(bp_stamp_string, response["TPS_HASH_TYPE"]).upcase # Calculate expected BP_STAMP using hash function specified in response

  if expected_stamp == response["BP_STAMP"] # Compare expected BP_STAMP with received BP_STAMP
    # Validate BP_STAMP and reads the response results
    print "VALID BP_STAMP: TRUE<br/>"
    response.params.each{|k,v| print "#{k}: #{v[0]}<br/>"}
  else
    print "ERROR: BP_STAMP VALUES DO NOT MATCH<br/>"
  end

else
  print "ERROR: BP_STAMP NOT FOUND. CHECK MESSAGE & RESPONSEVERSION<br/>"
end

print "</body></html>"