##
# BluePay Perl Sample code.
#
# This code sample runs a $15.00 Credit Card Sale transaction
# against a customer using test payment information.
# Optional transaction data is also sent.
# If using TEST mode, odd dollar amounts will return
# an approval and even dollar amounts will return a decline.
##

use strict;
use lib '..';
use bluepay;

my $account_id = "100228390579";
my $secret_key = "AKGIF9X9WT9CLQCWDFONC8N3HXRL9Y5K";
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


# Optional fields users can set
$payment->{CUSTOM_ID} = '098765'; 
$payment->{CUSTOM_ID2} = '12345678'; 
$payment->{ORDER_ID} = '500000'; 
$payment->{INVOICE_ID} = '000045612'; 
$payment->{AMOUNT_TIP} = '1.12';
$payment->{AMOUNT_TAX} = '0.80';
$payment->{AMOUNT_FOOD} = '1.50';
$payment->{AMOUNT_MISC} = '1.33';
$payment->{COMMENT} = "Enter any comments about the transaction here.";

$payment->sale({amount => '3.00'}); # Sale Amount: $3.00

# Makes the API Request with BluePay
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