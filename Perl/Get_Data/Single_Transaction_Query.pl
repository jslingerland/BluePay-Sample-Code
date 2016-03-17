##
# BluePay Perl Sample code.
#
# This code sample runs a report that grabs a single transaction
# from the BluePay gateway based on certain criteria.
# See comments below on the details of the report.
# If using TEST mode, only TEST transactions will be returned.
##

use strict;
use lib '..';
use bluepay;

my $account_id = "Merchant's Account ID Here";
my $secret_key = "Merchant's Secret Key Here";
my $mode = "TEST";

my $report = BluePay->new(
	$account_id, 
	$secret_key, 
	$mode
);

$report->get_single_transaction_query({
	transaction_id => 'ID of previous transaction', # required
	report_start_date => '2013-01-01', #YYYY-MM-DD, required
	report_end_date => '2015-05-30', #YYYY-MM-DD, required
	exclude_errors => '1' # Do not include errored transactions? Yes
});

# Makes the API request with BluePay        
$report->process();

if ($report->{payment_type}) {
# Get response from BluePay
print "TRANSACTION ID: " . $report->{id} . "\n";
print "FIRST NAME: " . $report->{name1} . "\n";
print "LAST NAME: " . $report->{name2} . "\n";
print "PAYMENT TYPE: " . $report->{payment_type} . "\n";
print "TRANSACTION TYPE: " . $report->{trans_type} . "\n";
print "AMOUNT: " . $report->{amount} . "\n";
print "CARD TYPE: " . $report->{payment_account} . "\n";
} else {
	print $report->{response};
}

