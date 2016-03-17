##
# BluePay Perl Sample code.
#
# This code sample runs a report that grabs data from the
# BluePay gateway based on certain criteria. This will ONLY return
# transactions that have already settled. See comments below
# on the details of the report.
# If using TEST mode, only TEST transactions will be returned.
##

use strict;
use lib '..';
use bluepay;

my $account_id = "100228390579";
my $secret_key = "AKGIF9X9WT9CLQCWDFONC8N3HXRL9Y5K";
my $mode = "TEST";

my $report = BluePay->new(
	$account_id, 
	$secret_key, 
	$mode
);

$report->get_settled_transaction_report({
	report_start_date => '2015-01-01', #YYYY-MM-DD
	report_end_date => '2015-04-30', #YYYY-MM-DD
	query_by_hierarchy => '1', # Also search subaccounts? Yes
	do_not_escape => '1', # Output response without commas? Yes
	exclude_errors => '1' # Do not include errored transactions? Yes
});

# Makes the API Request with BluePay
$report->process();

# Reads the response from BluePay
print $report->{response};