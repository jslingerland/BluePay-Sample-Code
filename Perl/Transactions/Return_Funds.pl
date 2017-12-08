##
# BluePay Perl Sample code.
#
# This code sample runs a $3.00 Credit Card Sale transaction
# against a customer using test payment information. If
# approved, a 2nd transaction is run to refund the customer
# for $1.75.
# If using TEST mode, odd dollar amounts will return
# an approval and even dollar amounts will return a decline.
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
	cc_expiration => '1225', # Card Expiration Date: MMYY
	cvv2 =>'123' # Card CVV2
});

$payment->sale({amount => '3.00'}); # Sale Amount: $3.00

# Makes the API Request with BluePay
$payment->process();

my $payment_return = BluePay->new(
	$account_id, 
	$secret_key, 
	$mode
);

$payment_return->refund({
	trans_id => $payment->{RRNO},# id of previous transaction to refund
	amount => '1.75' # partial refund of $1.75
});

# Makes the API Request to process refund
$payment_return->process();

# Reads the response from BluePay
if ($payment->is_successful_response()){
	print "TRANSACTION STATUS: " . $payment_return->{Result} . "\n";
	print "TRANSACTION MESSAGE: " . $payment_return->{MESSAGE} . "\n";
	print "TRANSACTION ID: " . $payment_return->{RRNO} . "\n";
	print "AVS RESULT: " .$payment_return->{AVS} . "\n";
	print "CVV2 RESULT: " . $payment_return->{CVV2} . "\n";
	print "MASKED PAYMENT ACCOUNT: " . $payment_return->{PAYMENT_ACCOUNT} . "\n";
	print "CARD TYPE: " . $payment_return->{CARD_TYPE} . "\n";
	print "AUTH CODE: " . $payment_return->{AUTH_CODE} . "\n";
} else {
	print $payment->{MESSAGE} . "\n";
}