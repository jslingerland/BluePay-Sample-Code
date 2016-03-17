##
# BluePay Perl Sample code.
#
# This code sample creates a recurring payment charging $15.00 per month for one year.
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
	address1 => '123 Test St.',
	address2 => 'Apt', 
	city => 'Testville', 
	state => 'IL', 
	zip_code => '54321',
	country => 'USA',
	phone => '123-123-12345',
	email => 'test@bluepay.com'
});

$rebill->set_ach_information({
	ach_routing => "123123123", # Routing Number: 123123123
  ach_account => "123456789", # Account Number: 123456789
  ach_account_type => 'C', # Account Type: Checking
  doc_type => "WEB" # ACH Document Type: WEB
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

# Reads the response from BluePay
if ($rebill->is_successful_response()){
	print "TRANSACTION ID: " . $rebill->{RRNO} . "\n";
	print "REBILL ID: " . $rebill->{REBID} . "\n";
	print "TRANSACTION STATUS: " . $rebill->{Result} . "\n";
	print "TRANSACTION MESSAGE: " . $rebill->{MESSAGE} . "\n";
	print "MASKED PAYMENT ACCOUNT: " . $rebill->{PAYMENT_ACCOUNT} . "\n";
	print "CUSTOMER BANK: " . $rebill->{BANK_NAME} . "\n";
} else {
	print $rebill->{MESSAGE} . "\n";
}
