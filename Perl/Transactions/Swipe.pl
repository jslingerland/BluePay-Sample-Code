##
# BluePay Perl Sample code.
#
# This code sample runs a $3.00 Credit Card Sale transaction
# against a customer using test payment information.
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
	address1 => '1234 Test St.',
	address2 => 'Apt', 
	city => 'Testville', 
	state => 'IL', 
	zip_code => '54321',
	country => 'USA',
	phone => '123-123-12345',
	email => 'test@bluepay.com'
});

# Set payment information for a swiped credit card transaction
$payment->swipe("%B4111111111111111^TEST/BLUEPAY^1911101100001100000000667000000?;4111111111111111=191110110000667?");

# Sale Amount: $3.00
$payment->sale({amount => '3.00'}); 

# Makes the API Request with BluePay
$payment->process();


# Reads the response from BluePay
if ($payment->is_successful_response()){
	print "TRANSACTION STATUS: " . $payment->{Result} . "\n";
	print "TRANSACTION MESSAGE: " . $payment->{MESSAGE} . "\n";
	print "TRANSACTION ID: " . $payment->{RRNO} . "\n";
	print "AVS RESPONSE: " .$payment->{AVS} . "\n";
	print "CVV2 RESPONSE: " . $payment->{CVV2} . "\n";
	print "MASKED PAYMENT ACCOUNT: " . $payment->{PAYMENT_ACCOUNT} . "\n";
	print "CARD TYPE: " . $payment->{CARD_TYPE} . "\n";
	print "AUTH CODE: " . $payment->{AUTH_CODE} . "\n";
} else {
	print $payment->{MESSAGE} . "\n";
}