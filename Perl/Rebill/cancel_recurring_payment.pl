##
# BluePay Perl Sample code.
#
# This code sample runs a $0.00 Credit Card Auth transaction
# against a customer using test payment information, sets up
# a rebilling cycle, and also shows how to cancel that rebilling cycle.
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

# If transaction was approved..
if ($rebill->is_successful_response()){
    
    my $rebill_cancel = BluePay->new(
        $account_id, 
        $secret_key, 
        $mode
    );

    # Find rebill by id and cancel rebilling cycle 
    $rebill_cancel->cancel_rebilling_cycle($rebill->{REBID});

    # Makes the API request to cancel the rebill
    $rebill_cancel->process();


    # Reads the response from BluePay
    print "REBILL STATUS: " . $rebill_cancel->{status} . "\n";
    print "REBILL ID: " . $rebill_cancel->{rebill_id} . "\n";
    print "REBILL CREATION DATE: " .$rebill_cancel->{creation_date} . "\n";
    print "REBILL NEXT DATE: " .$rebill_cancel->{next_date} . "\n";
    print "REBILL LAST DATE: " .$rebill_cancel->{last_date} . "\n";
    print "REBILL SCHEDULE EXPRESSION: " .$rebill_cancel->{sched_expr} . "\n";
    print "REBILL CYCLES REMAINING: " .$rebill_cancel->{cycles_remain} . "\n";
    print "REBILL AMOUNT: " .$rebill_cancel->{reb_amount} . "\n";
    print "REBILL NEXT AMOUNT: " .$rebill_cancel->{next_amount} . "\n";
} 
else {
    print $rebill->{MESSAGE};
}