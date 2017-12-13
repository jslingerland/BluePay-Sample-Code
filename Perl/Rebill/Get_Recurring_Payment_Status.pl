##
# BluePay Perl Sample code.
#
# This code sample runs a $0.00 Credit Card Auth transaction
# against a customer using test payment information.
# Once the rebilling cycle is created, this sample shows how to
# get information on this rebilling cycle.
# See comments below on the details of the initial setup of the
# rebilling cycle.
##

use strict;
use lib '..';
use bluepay;

my $account_id = "Merchant's Account ID Here";
my $secret_key = "Merchant's Secret Key Here";
my $mode = "TEST";

my $rebill = BluePay->new(
	$account_id, 
	$secret_key, 
	$mode
);

$rebill->set_customer_information({
	first_name => 'Bob',
	last_name => 'Tester', 
	address1 => '1234 Test St.',
	address2 => 'Apt', 
	city => 'Testville', 
	state => 'IL', 
	zip_code => '54321',
	country => 'USA',
	phone => '123-123-12345',
	email => 'test@bluepay.com'
});

$rebill->set_cc_information({
	cc_number =>'4111111111111111', # Customer Credit Card Number
	cc_expiration => '1225', # Card Expiration Date: MMYY
	cvv2 =>'123' # Card CVV2
});

$rebill->set_recurring_payment({
	reb_first_date => "2015-01-01", # Rebill Start Date: Jan. 1, 2015
	reb_expr => "1 MONTH", # Rebill Frequency: 1 MONTH
	reb_cycles => "12", # Rebill # of Cycles: 12
	reb_amount => "15.00" # Rebill Amount: $15.00
});

$rebill->auth({amount => '0.00'}); # Card authorization amount: $0.00

# Makes the API Request to create a recurring payment
$rebill->process();

my $rebill_status = BluePay->new(
	$account_id, 
	$secret_key, 
	$mode
);

 # Find the rebill by ID and get rebill status 
$rebill_status->get_rebilling_cycle_status($rebill->{REBID});

# Makes the API Request with BluePay
$rebill_status->process();

# Reads the response from BluePay
if ($rebill->is_successful_response()){
	print "REBILL STATUS: " . $rebill_status->{status} . "\n";
	print "REBILL ID: " . $rebill_status->{rebill_id} . "\n";
	print "REBILL CREATION DATE: " .$rebill_status->{creation_date} . "\n";
	print "REBILL NEXT DATE: " .$rebill_status->{next_date} . "\n";
	print "REBILL LAST DATE: " .$rebill_status->{last_date} . "\n";
	print "REBILL SCHEDULE EXPRESSION: " .$rebill_status->{sched_expr} . "\n";
	print "REBILL CYCLES REMAINING: " .$rebill_status->{cycles_remain} . "\n";
	print "REBILL AMOUNT: " .$rebill_status->{reb_amount} . "\n";
	print "REBILL NEXT AMOUNT: " .$rebill_status->{next_amount} . "\n";	
} else {
	print $rebill->{MESSAGE} . "\n";
}
