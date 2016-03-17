##
# BluePay Perl Sample code.
#
# This code sample runs a $0.00 AUTH transaction
# against a customer using test payment information.
# This stores the customer's payment information securely in
# BluePay to be used for further transactions.
# Note: THIS DOES NOT ENSURE THAT THE CREDIT CARD OR ACH
# ACCOUNT IS VALID.
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

$payment->set_cc_information({
	cc_number =>'4111111111111111', # Customer Credit Card Number
	cc_expiration => '0815', # Card Expiration Date: MMYY
	cvv2 =>'123' # Card CVV2
});

$payment->auth({amount => '0.00'}); # Card authorization at $0.00

# Makes the API request with BluePay
$payment->process();

# Reads the response from BluePay
if ($payment->is_successful_response()){
	print "TRANSACTION STATUS: " . $payment->{Result} . "\n";
	print "TRANSACTION MESSAGE: " . $payment->{MESSAGE} . "\n";
	print "TRANSACTION ID: " . $payment->{RRNO} . "\n";
	print "AVS RESULT: " .$payment->{AVS} . "\n";
	print "CVV2 RESULT: " . $payment->{CVV2} . "\n";
	print "MASKED PAYMENT ACCOUNT: " . $payment->{PAYMENT_ACCOUNT} . "\n";
	print "CARD TYPE: " . $payment->{CARD_TYPE} . "\n";
	print "AUTH CODE: " . $payment->{AUTH_CODE} . "\n";
} else {
	print $payment->{MESSAGE} . "\n";
}