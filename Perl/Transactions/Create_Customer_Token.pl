##
# BluePay Perl Sample code.
#
# This code sample runs a $0.00 AUTH transaction
# and creates a customer token using test payment information,
# which is then used to run a separate $3.99 sale.
##

use strict;
use lib '..';
use bluepay;

my $account_id = "Merchant's Account ID Here";
my $secret_key = "Merchant's Secret Key Here";
my $mode = "TEST";

my $auth = BluePay->new(
	$account_id, 
	$secret_key, 
	$mode
);

$auth->set_customer_information({
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

$auth->set_cc_information({
	cc_number =>'4111111111111111', # Customer Credit Card Number
	cc_expiration => '1225', # Card Expiration Date: MMYY
	cvv2 =>'123' # Card CVV2
});

$auth->auth({
	amount => '0.00', # Card authorization at $0.00
	new_customer_token => 'true' # Create new customer token
}); 

# Makes the API request with BluePay
$auth->process();

# Reads the response from BluePay
if ($auth->is_successful_response()){
	my $payment = BluePay->new(
		$account_id, 
		$secret_key, 
		$mode
	);
	
	$payment->sale({
		amount => '3.99', 
		customer_token => $auth->{CUST_TOKEN}  # provide customer token
	});

	$payment->process();

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
} else {
	print $auth->{MESSAGE} . "\n";
}