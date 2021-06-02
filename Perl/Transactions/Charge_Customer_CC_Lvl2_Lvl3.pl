##
# BluePay Perl Sample code.
#
# This code sample runs a Credit Card sales transaction, 
# including sample Level 2 and 3 processing information,
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
	#stored_type => 'C', # Optional
	#stored_indicator => 'F', # Optional
	#stored_id => 'TESTID526957756', #Optional	
	email => 'test@bluepay.com'
});

$payment->set_cc_information({
	cc_number =>'4111111111111111', # Customer Credit Card Number
	cc_expiration => '1225', # Card Expiration Date: MMYY
	cvv2 =>'123' # Card CVV2
});

# Set Level 2 Information
$payment->set_invoice_id("123456789");
$payment->set_amount_tax("0.91");

# Set Level 3 line item information. Repeat for each item up to 99.
$payment->add_line_item({
  quantity => '1', # The number of units of item. Max: 5 digits
  unit_cost => '3.00', # The cost per unit of item. Max: 9 digits decimal
  descriptor => 'test1', # Description of the item purchased. Max: 26 character
  commodity_code => '123412341234', # Commodity Codes can be found at http://www.census.gov/svsd/www/cfsdat/2002data/cfs021200.pdf. Max: 12 characters
  product_code => '432143214321', # Merchant-defined code for the product or service being purchased. Max: 12 characters 
  measure_units => 'EA', # The unit of measure of the item purchase. Normally EA. Max: 3 characters
  tax_rate => '7%', # Tax rate for the item. Max: 4 digits
  tax_amount => '0.21', # Tax amount for the item. unit_cost * quantity * tax_rate = tax_amount. Max: 9 digits.
  item_discount => '0.00', # The amount of any discounts on the item. Max: 12 digits.
  line_item_total => '3.21' # The total amount for the item including taxes and discounts.
});

$payment->add_line_item({
  quantity => '2',
  unit_cost => '5.00',
  descriptor => 'test2',
  commodity_code => '123412341234',
  product_code => '098709870987',
  measure_units => 'EA',
  tax_rate => '7%',
  tax_amount => '0.70',
  item_discount => '0.00',
  line_item_total => '10.70'
});

$payment->sale({amount => '13.91'}); # Sale Amount: $13.91

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
	#print "STORED ID: " . $payment->{RESPONSE_STORED_ID} . "\n";
} else {
	print $payment->{MESSAGE} . "\n";
}
