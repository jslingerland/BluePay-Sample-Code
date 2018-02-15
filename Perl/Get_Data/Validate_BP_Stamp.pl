#!/usr/bin/perl

##
# BluePay Perl Sample code.
#
# This code sample reads the values from a BP10emu redirect
# and authenticates the message using the the BP_STAMP
# provided in the response. Point the REDIRECT_URL of your 
# BP10emu request to the location of this script on your server.
##

print "Content-type:text/html\r\n\r\n";
print "<html><head></head><body>";

use strict;
use CGI;
use lib '..';
use bluepay;

my $account_id = "Merchant's Account ID Here";
my $secret_key = "Merchant's Secret Key Here";
my $mode = "TEST";

my $response = new CGI;
my $response_params = $response->Vars;

if (defined $response_params->{BP_STAMP}){ # Check whether BP_STAMP is provided

	my $bp = BluePay->new(
	    $account_id, 
	    $secret_key, 
	    $mode
	);

	my $bp_stamp_string = '';
	foreach my $field (split(' ', $response_params->{BP_STAMP_DEF})){ # Split BP_STAMP_DEF on whitespace
		$bp_stamp_string .= $response_params->{$field}; # Concatenate values used to calculate expected BP_STAMP
	}

    my $expected_stamp = uc($bp->generate_tps($bp_stamp_string, $response_params->{TPS_HASH_TYPE})); # Calculate expected BP_STAMP using hash function specified in response

    if ($expected_stamp eq  $response_params->{BP_STAMP}){ # Compare expected BP_STAMP with received BP_STAMP
    	# Validate BP_STAMP and reads the response results
        print "VALID BP_STAMP: TRUE<br/>"; 
    	foreach my $key (keys $response_params){
    		print $key . ': ' . $response_params->{$key} . "<br/>";
    	}
    }
    else{
        print "ERROR: BP_STAMP VALUES DO NOT MATCH<br/>";
    }
} else{
    print "ERROR: BP_STAMP NOT FOUND. CHECK MESSAGE & RESPONSEVERSION<br/>";
}

print "</body></html>";
}