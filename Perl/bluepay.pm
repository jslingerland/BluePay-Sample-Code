package BluePay;

$VERSION = '1.10';
use strict;
use warnings;
# Required modules
use Digest::MD5 qw(md5_hex);
use LWP::UserAgent;
use URI::Escape;
use CGI;
use Mozilla::CA;

my $URL  = '';
my $MODE = '';
my $TAMPER_PROOF_SEAL;

# New
sub new {
    my $class = shift;
    my $self  = {};      # allocate new hash for object
    bless( $self, $class );

    # Sets account variable
    $self->{ACCOUNT_ID} = shift;
    $self->{SECRET_KEY} = shift;
    $self->{MODE} = shift;

    # return object
    return $self;
}

# calculates tamper proof seal based on api declared
sub calc_tps {
    my $self = shift;
    if ($self->{API} eq 'bpdailyreport2' ) {
        $self->{URL} = 'https://secure.bluepay.com/interfaces/bpdailyreport2';
        my $TAMPER_PROOF_DATA =
            ( $self->{SECRET_KEY}        || '' )
          . ( $self->{ACCOUNT_ID}        || '' )
          . ( $self->{REPORT_START_DATE} || '' )
          . ( $self->{REPORT_END_DATE}   || '' );
        $TAMPER_PROOF_SEAL = md5_hex $TAMPER_PROOF_DATA;
    }
    elsif ($self->{API} eq "stq") {
        $self->{URL} = 'https://secure.bluepay.com/interfaces/stq';
        my $TAMPER_PROOF_DATA =
            ( $self->{SECRET_KEY}        || '' )
          . ( $self->{ACCOUNT_ID}        || '' )
          . ( $self->{REPORT_START_DATE} || '' )
          . ( $self->{REPORT_END_DATE}   || '' );
        $TAMPER_PROOF_SEAL = md5_hex $TAMPER_PROOF_DATA;
    }    
    elsif ($self->{API} eq 'bp10emu' ) {
        #returns the remote host IP address
        my $q = CGI->new;
        $self->{REMOTE_IP} = $q->remote_addr(); 
        $self->{URL} = 'https://secure.bluepay.com/interfaces/bp10emu';
        $self->{MERCHANT} = $self->{ACCOUNT_ID};
        my $TAMPER_PROOF_DATA =
            ( $self->{SECRET_KEY}       || '' )
          . ( $self->{MERCHANT}         || '' )
          . ( $self->{TRANSACTION_TYPE} || '' )
          . ( $self->{AMOUNT}           || '' )
          . ( $self->{REBILLING}        || '' )
          . ( $self->{REB_FIRST_DATE}   || '' )
          . ( $self->{REB_EXPR}         || '' )
          . ( $self->{REB_CYCLES}       || '' )
          . ( $self->{REB_AMOUNT}       || '' )
          . ( $self->{RRNO}             || '' )
          . ( $self->{MODE}             || '' );
        $TAMPER_PROOF_SEAL = md5_hex $TAMPER_PROOF_DATA;
    }
    elsif ($self->{API} eq 'bp20rebadmin' ) {
        $self->{URL} = 'https://secure.bluepay.com/interfaces/bp20rebadmin';
        my $TAMPER_PROOF_DATA =
            ( $self->{SECRET_KEY} || '' )
          . ( $self->{ACCOUNT_ID} || '' )
          . ( $self->{TRANS_TYPE} || '' )
          . ( $self->{REBILL_ID}  || '' );
        $TAMPER_PROOF_SEAL = md5_hex $TAMPER_PROOF_DATA;
    }
    return $TAMPER_PROOF_SEAL;
}

# Makes the API request and gets response
sub process {
    my $self = shift;
    # calculates tamper proof seal
    $TAMPER_PROOF_SEAL = $self->calc_tps();
    # Create request (encode)
    my $request =
        $self->{URL} 
        . "\?FIELDS="
        . "&TAMPER_PROOF_SEAL="
        . uri_escape( $TAMPER_PROOF_SEAL || '' );
    
    # converts the object's attributes into a query string for the api request
    while ( my ( $key, $value ) = each(%$self) ) {
        if ( $key eq 'SECRET_KEY' ) { next; }
        if ( $key eq 'URL' )        { next; }
        $request .= "&$key=" . uri_escape( $value || '' );
    }

    # prints the full api request url 
    # print "\n" . "request: "  . "\n" . $request . "\n \n";

    # Create Agent
    my $ua = new LWP::UserAgent;
    my $content;
    
    if ($self->{API} eq 'bp10emu' ) {
        my $req = new HTTP::Request 'POST', $self->{URL};
        $req->content($request);
        my $raw_response = $ua->request($req);
        my $response_string = $raw_response->header("Location");
        my @content_string  = split /[?]/, $response_string;
        my $content_string = $content_string[1];
        # use the parse response method to parse the raw response and assign response values
        $self->parse_response($content_string);
    }
    elsif ($self->{API} eq 'bpdailyreport2' ) {
        my $req = new HTTP::Request 'POST', $self->{URL};
        $req->content($request);
        my $raw_response = $ua->request($req);
        $content = $raw_response->content;
        $self->{response} = $content
    }
    elsif ($self->{API} eq 'stq' ) {
        my $req = new HTTP::Request 'POST', $self->{URL};
        $req->content($request);
        my $raw_response = $ua->request($req);
        $content = $raw_response->content;
        $self->parse_response($content);
        $self->{response} = $content;
    }
    elsif ($self->{API} eq 'bp20rebadmin' ){
        my $req = new HTTP::Request 'POST', $self->{URL};
        $req->content($request);
        my $raw_response = $ua->request($req);
        $content = $raw_response->content;
        $self->parse_response($content);
    }
}

# Parse Response string and assign response values to self.  (Used in process function)
sub parse_response {
    my $self = shift;
    my $response_string = shift;
     # create an array of values; split at the '&' symbol
     my @response_array = split( /&/, $response_string);
     # prints the array
     # print join(", ", @response_array); print "\n";

     # Iterates through array.  Assigns attributes to self for each of the key-values in response.  
    for my $el (@response_array) {
         # print $el; print "\n";
         my @array = split ( /=/, $el); 
         # print $key; print "\n";
         my $key = uri_unescape($array[0]);
         my $value = uri_unescape($array[1]);
        $self->{$key} = $value;
    }
}

# returns true if the transaction is successful
sub is_successful_response{
    my $self = shift;
    $self->{Result} eq "APPROVED" && $self->{MESSAGE} ne "DUPLICATE";
    # return 1
}

# Set CC information
sub set_cc_information {
    my $self = shift;
    my $params =  shift;
    $self->{PAYMENT_TYPE} = 'CREDIT'; 
    $self->{CC_NUM} = $params->{cc_number};
    $self->{CC_EXPIRES} = $params->{cc_expiration};
    $self->{CVCVV2} = $params->{cvv2}; 
}

# Sets payment information for a swiped credit card transaction
sub swipe {
    my $self = shift;
    my $track_data = shift;
    $self->{SWIPE} = $track_data;
}

# Set Customer Information
sub set_customer_information{
    my $self = shift;
    my $params =  shift;
    $self->{NAME1} = $params->{first_name};
    $self->{NAME2} = $params->{last_name};
    $self->{ADDR1} = $params->{address1};
    $self->{ADDR2} = $params->{address2};
    $self->{CITY} = $params->{city};
    $self->{STATE} = $params->{state};
    $self->{ZIPCODE} = $params->{zip_code};
    $self->{COUNTRY} = $params->{country};
    $self->{PHONE} = $params->{phone};
    $self->{EMAIL} = $params->{email};
}

# Set ACH Payment information 
sub set_ach_information{
    my $self = shift;
    my $params =  shift;
    $self->{PAYMENT_TYPE} = 'ACH';
    $self->{ACH_ROUTING} = $params->{ach_routing};
    $self->{ACH_ACCOUNT} = $params->{ach_account};
    $self->{ACH_ACCOUNT_TYPE} = $params->{ach_account_type};
    $self->{DOC_TYPE} = $params->{doc_type}; #optional
}

# Set Sale Transaction
sub sale {
    my $self = shift;
    my $params =  shift;
    $self->{TRANSACTION_TYPE} = 'SALE';
    $self->{API} = 'bp10emu'; 
    $self->{AMOUNT} = $params->{amount};
    $self->{RRNO} = $params->{trans_id}; #optional
}

# Set up an Auth
sub auth{
    my $self = shift;
    my $params =  shift;
    $self->{TRANSACTION_TYPE} = 'AUTH';
    $self->{AMOUNT} = $params->{amount};
    $self->{RRNO} = $params->{trans_id}; # optional
    $self->{API} = 'bp10emu'; 
 } 
# Capture an Auth
sub capture{
    my $self = shift;
    my $params =  shift;
    $self->{TRANSACTION_TYPE} = 'CAPTURE';
    $self->{AMOUNT} = $params->{amount};
    $self->{RRNO} = $params->{trans_id}; # optional
    $self->{API} = 'bp10emu'; 
}

# Refund
sub refund{
    my $self = shift;
    my $params =  shift;
    $self->{TRANSACTION_TYPE} = 'REFUND';
    $self->{AMOUNT} = $params->{amount};
    $self->{RRNO} = $params->{trans_id}; # optional
    $self->{API} = 'bp10emu'; 
}
# Void
sub void{
    my $self = shift;
    my $params =  shift;
    $self->{TRANSACTION_TYPE}  = 'VOID';
    $self->{AMOUNT} = $params->{amount};
    $self->{RRNO} = $params->{trans_id}; # optional
    $self->{API} = 'bp10emu'; 
}

# Set fields for a recurring payment
sub set_recurring_payment{
    my $self = shift;
    my $params =  shift;
    $self->{REBILLING} = '1';
    $self->{REB_FIRST_DATE} = $params->{reb_first_date};
    $self->{REB_EXPR} = $params->{reb_expr};
    $self->{REB_CYCLES} = $params->{reb_cycles};
    $self->{REB_AMOUNT} = $params->{reb_amount};
}
 
# Set fields to do an update on an existing rebilling cycle
sub update_rebill{
    my $self = shift;
    my $params =  shift;
    $self->{TRANS_TYPE} = "SET";
    $self->{REBILL_ID} = $params->{rebill_id};
    $self->{NEXT_DATE} = $params->{next_date};
    $self->{REB_EXPR} = $params->{reb_expr};
    $self->{REB_CYCLES} = $params->{reb_cycles};
    $self->{REB_AMOUNT} = $params->{reb_amount};
    $self->{NEXT_AMOUNT} = $params->{next_amount};
    $self->{TEMPLATE_ID} = $params->{template_id};
    $self->{API} = "bp20rebadmin";
}
# Set fields to cancel an existing rebilling cycle
sub cancel_rebilling_cycle{
    my $self = shift;
    my $rebill_id = shift;
    $self->{TRANS_TYPE} = "SET";
    $self->{STATUS} = "stopped";
    $self->{REBILL_ID} = $rebill_id;
    $self->{API} = "bp20rebadmin";
}
# Set fields to get the status of an existing rebilling cycle
sub get_rebilling_cycle_status{
    my $self = shift;
    my $rebill_id = shift;
    $self->{TRANS_TYPE} = "GET";
    $self->{REBILL_ID} = $rebill_id;
    $self->{API} = "bp20rebadmin";
}
# Gets a report on all transactions within a specified date range
sub get_transaction_report{
    my $self = shift;
    my $params =  shift;
    $self->{QUERY_BY_SETTLEMENT} = '0';
    $self->{REPORT_START_DATE} = $params->{report_start_date};
    $self->{REPORT_END_DATE} = $params->{report_end_date};
    $self->{QUERY_BY_HIERARCHY} = $params->{query_by_hierarchy};
    $self->{DO_NOT_ESCAPE} = $params->{do_not_escape}; # optional
    $self->{EXCLUDE_ERRORS} = $params->{exclude_errors}; # optional
    $self->{API} = "bpdailyreport2";
}

# Gets a report on all settled transactions within a specified date range
sub get_settled_transaction_report{
    my $self = shift;
    my $params =  shift;
    $self->{QUERY_BY_SETTLEMENT} = '1';
    $self->{REPORT_START_DATE} = $params->{report_start_date};
    $self->{REPORT_END_DATE} = $params->{report_end_date};
    $self->{QUERY_BY_HIERARCHY} = $params->{query_by_hierarchy};
    $self->{DO_NOT_ESCAPE} = $params->{do_not_escape}; # optional
    $self->{EXCLUDE_ERRORS} = $params->{exclude_errors}; # optional
    $self->{API} = "bpdailyreport2";
}

# Gets data on a specific transaction
sub get_single_transaction_query{
    my $self = shift;
    my $params =  shift;
    $self->{API} = "stq";    
    $self->{REPORT_START_DATE} = $params->{report_start_date};
    $self->{REPORT_END_DATE} = $params->{report_end_date};
    $self->{id} = $params->{transaction_id};
    $self->{EXCLUDE_ERRORS} = $params->{exclude_errors}; # optional
}






# COMMENTS
=head1 MODULES

This script has some dependencies that need to be installed before it
can run.  You can use cpan to install the modules.  They are:
 - Digest::MD5
 - LWP::UserAgent
 - URI::Escape

=head1 AUTHOR

The BluePay::BluePayPayment perl module was written by Christopher Kois <ckois@bluepay.com> and modified
by Justin Slingerland <jslingerland@bluepay.com>.

=head1 COPYRIGHTS

    The BluePay::BluePay package is Copyright (c) January, 2013 by BluePay, Inc. 
    http://www.bluepay.com All rights reserved.  You may distribute this module under the terms 
    of GNU General Public License (GPL). 
    
Module Copyrights:
 - The Digest::MD5 module is Copyright (c) 1998-2003 Gisle Aas.
    Available at: http://search.cpan.org/~gaas/Digest-MD5-2.36/MD5.pm
 - The LWP::UserAgent module is Copyright (c) 1995-2008 Gisle Aas.
    Available at: http://search.cpan.org/~gaas/libwww-perl-5.812/lib/LWP/UserAgent.pm
 - The Crypt::SSLeay module is Copyright (c) 2006-2007 David Landgren.
    Available at: http://search.cpan.org/~dland/Crypt-SSLeay-0.57/SSLeay.pm
 - The URI::Escape module is Copyright (c) 1995-2004 Gisle Aas.
    Available at: http://search.cpan.org/~gaas/URI-1.36/URI/Escape.pm
                
NOTE: Each of these modules may have other dependencies.  The modules listed here are
the modules that BluePay::BluePayPayment specifically references.

=head1 SUPPORT/WARRANTY

BluePay::BluePayPayment is free Open Source software.  This code is Free.  You may use it, modify it, 
redistribute it, Post it on the bathroom wall, or whatever.  If you do make modifications that are 
useful, BluePay would love it if you donated them back to us!

=head1 KNOWN BUGS:

This is version 1.10 of BluePay::BluePayPayment.  There are currently no known bugs.

=head2 Post

Posts the data to the BluePay::BluePayPayment interface

=cut







1;
