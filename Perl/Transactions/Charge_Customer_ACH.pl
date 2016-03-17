##
# BluePay Perl Sample code.
#
# This code sample runs a $3.00 ACH Sale transaction
# against a customer using test payment information.
##

use strict;
use lib '..';
use bluepay;

my $account_id = "Merchant's Account ID Here";
my $secret_key = "Merchant's Secret Key Here";
my $mode = "TEST";

my $payment = BluePay->new(
	$account_id, 
	$secret_key, 
	$mode
);

$payment->set_customer_information({
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

$payment->set_ach_information({
	ach_routing => "123123123", # Routing Number: 123123123
  ach_account => "123456789", # Account Number: 123456789
  ach_account_type => 'C', # Account Type: Checking
  doc_type => "WEB" # ACH Document Type: WEB
});

$payment->sale({amount => '3.00'}); # Sale Amount: $3.00

# Makes the API Request with BluePay
$payment->process();

# Reads the response from BluePay
if ($payment->is_successful_response()){
	print "TRANSACTION ID: " . $payment->{RRNO} . "\n";
	print "TRANSACTION STATUS: " . $payment->{Result} . "\n";
	print "TRANSACTION MESSAGE: " . $payment->{MESSAGE} . "\n";
	print "MASKED PAYMENT ACCOUNT: " . $payment->{PAYMENT_ACCOUNT} . "\n";
	print "CUSTOMER BANK: " . $payment->{BANK_NAME} . "\n";
} else {
	print $payment->{MESSAGE} . "\n";
}