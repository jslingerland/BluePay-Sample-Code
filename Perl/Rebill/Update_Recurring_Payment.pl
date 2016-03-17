##
# BluePay Perl Sample code.
#
# This code sample runs a $0.00 Credit Card Auth transaction
# against a customer using test payment information.
# Once the rebilling cycle is created, this sample shows how to
# update the rebilling cycle. See comments below
# on the details of the initial setup of the rebilling cycle as well as the
# updated rebilling cycle.
##

use strict;
use lib '..';
use bluepay;

my $account_id = "100228390579";
my $secret_key = "AKGIF9X9WT9CLQCWDFONC8N3HXRL9Y5K";
my $mode = "TEST";

my $rebill = BluePay->new(
	$account_id, 
	$secret_key, 
	$mode
);

$rebill->set_customer_information({
	first_name => 'Bob',
	last_name => 'Tester', 
	address1 => '1235 Test St.',
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
	cc_expiration => '0815', # Card Expiration Date: MMYY
	cvv2 =>'123' # Card CVV2
});


$rebill->set_recurring_payment({
	reb_first_date => "2015-01-01", # Rebill Start Date: Jan. 1, 2015
	reb_expr => "1 MONTH", # Rebill Frequency: 1 MONTH
	reb_cycles => "12", # Rebill # of Cycles: 12
	reb_amount => "15.00" # Rebill Amount: $15.00
});

$rebill->auth({amount => '0.00'}); # Card authorization amount: $0.00

# Makes the API Request with BluePay
$rebill->process();

my $payment_information_update = BluePay->new(
	$account_id, 
	$secret_key, 
	$mode
);

# Sets an updated credit card expiration date
$payment_information_update->set_cc_information({
	cc_expiration => '0819' # Card Expiration Date: MMYY
});

# Stores new card expiration date
$payment_information_update->auth({
	amount => '0.00',
	trans_id => $rebill->{RRNO} # the id of the rebill to update
}); 
  
# Makes the API Request to update the payment information
$payment_information_update->process();

# Creates a request to update the rebill
my $rebill_update = BluePay->new(
$account_id, 
$secret_key, 
$mode
);

# Updates the rebill
$rebill_update->update_rebill({
	rebill_id => $rebill->{REBID}, # The ID of the rebill to be updated.  
	template_id => $rebill->{RRNO}, # Updates the payment information portion of the rebilling cycle with the new card expiration date entered above 
	next_date => "2015-03-01", # Rebill Start Date: March 1, 2015
	reb_expr => "1 MONTH", # Rebill Frequency: 1 MONTH
	reb_cycles => "8", # Rebill # of Cycles: 8
	reb_amount => "5.15", # Rebill Amount: $5.15
	next_amount =>"1.50" # Rebill Next Amount: $1.50
});
  
 # Makes the API Request to update the rebill
 $rebill_update->process();

# Reads the response from BluePay
if ($rebill->is_successful_response()){
	print "REBILL STATUS: " . $rebill_update->{status} . "\n";
	print "REBILL ID: " . $rebill_update->{rebill_id} . "\n";
	print "REBILL CREATION DATE: " .$rebill_update->{creation_date} . "\n";
	print "REBILL NEXT DATE: " .$rebill_update->{next_date} . "\n";
	print "REBILL LAST DATE: " .$rebill_update->{last_date} . "\n";
	print "REBILL SCHEDULE EXPRESSION: " .$rebill_update->{sched_expr} . "\n";
	print "REBILL CYCLES REMAINING: " .$rebill_update->{cycles_remain} . "\n";
	print "REBILL AMOUNT: " .$rebill_update->{reb_amount} . "\n";
	print "REBILL NEXT AMOUNT: " .$rebill_update->{next_amount} . "\n";
} else {
	print $rebill->{MESSAGE} . "\n";
}