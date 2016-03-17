package BluePay::BluePayPayment_BP10Emu;

$VERSION   = '1.10';

use strict;
use warnings;

# Required modules
use Digest::MD5  qw(md5_hex);
use LWP::UserAgent;
use URI::Escape;

my $URL = '';
my $MODE = '';
my $TAMPER_PROOF_SEAL;

=head1 NAME

BluePay::BluePayPayment

=head1 VERSION

Version: 1.10
January 2013

=head1 SYNOPSIS

BluePay::BluePayPayment - The BluePay 2.0 Post interface

=head1 DESCRIPTION

BluePay::BluePayPayment is a Perl based implementation for interaction with the 
Bluepay 2.0 Post interface.  BluePayPayment accepts the parameters needed for the 
Bluepay20Post and sends the Post request to Bluepay via HTTPS. 

=head1 RUNNING BluePay::BluePayPayment_BP10Emu

	use BluePay::BluePayPayment;

	# Create object
	my $payment = BluePay::BluePayPayment_BP10Emu->new();

	# Assign values
	$payment->{MERCHANT} = "myaccountid";
	$payment->{SECRET_KEY} = 'mysecretkey';
	$payment->{TRANSACTION_TYPE} = 'SALE';
	$payment->{MODE} = 'TEST';		# Default is TEST --> Set to LIVE for live tx
	$payment->{AMOUNT} = '3.01';	# ODD returns Approved, EVEN returns Declined in TEST mode
	$payment->{CC_NUM} = '4111111111111111';	# VISA Test Card
	$payment->{CC_EXPIRES} = '0808';
	## PLEASE REVIEW THE BP20 POST DOCUMENTATION TO SEE ALL REQUIRED/POSSIBLE VALUES
	## REFERENCE THEM BY NAME DIRECTLY
	
	# Post --> Results contains the name value pair string of the response
	#  In this format: TRANS_ID=&STATUS=&AVS=&CVV2=&MESSAGE=&REBID=
	my $results = $payment->Post();

	# Can also retrieve the results directly from the object
	print $payment->{RRNO} . "\n";
	print $payment->{Result} . "\n";
	print $payment->{AVS} . "\n";
	print $payment->{CVV2} . "\n";
	print $payment->{AUTH_CODE} . "\n";
	print $payment->{MESSAGE} . "\n";
	print $payment->{REBID} . "\n";
	

=head1 METHODS

=head2 new

Creates a new instance of a BluePay::BluePayPayment object

=cut

# New
sub new  { 
    my $class = shift;
    my $self  = {};         # allocate new hash for object
    bless($self, $class);
    # Set defaults
    $self->{URL} = $URL;
    $self->{MODE} = $MODE;
    # return object
    return $self;
}

# functions for calculating the TAMPER_PROOF_SEAL

sub calc_tps {
    my $self = shift;
    if ( exists $self->{REPORT_START_DATE}) {
	my $TAMPER_PROOF_DATA = ($self->{SECRET_KEY} || '') . ($self->{ACCOUNT_ID} || '') . ($self->{REPORT_START_DATE} || '') .
                ($self->{REPORT_END_DATE} || '');
        $TAMPER_PROOF_SEAL = md5_hex $TAMPER_PROOF_DATA;
	if ( exists $self->{QUERY_BY_HIERARCHY}) {
	$self->{URL} = 'https://secure.bluepay.com/interfaces/bpdailyreport2';
        } else {
	$self->{URL} = 'https://secure.bluepay.com/interfaces/stq';
	}
    } elsif (defined $self->{TRANSACTION_TYPE}) {
    #} elsif ( $self->{TRANS_TYPE} ne "SET" and $self->{TRANS_TYPE} ne "GET") {
	$self->{URL} = 'https://secure.bluepay.com/interfaces/bp10emu';
	my $TAMPER_PROOF_DATA = ($self->{SECRET_KEY} || '') . ($self->{MERCHANT} || '') . ($self->{TRANSACTION_TYPE} || '') . ($self->{AMOUNT} || '')
                . ($self->{REBILLING} || '') . ($self->{REB_FIRST_DATE} || '') . ($self->{REB_EXPR} || '') . ($self->{REB_CYCLES} || '')
		. ($self->{REB_AMOUNT} || '') . ($self->{RRNO} || '') . ($self->{MODE} || '');
        $TAMPER_PROOF_SEAL = md5_hex $TAMPER_PROOF_DATA;
    } else {
        $self->{URL} = 'https://secure.bluepay.com/interfaces/bp20rebadmin';
        my $TAMPER_PROOF_DATA = ($self->{SECRET_KEY} || '') . ($self->{ACCOUNT_ID} || '') . ($self->{TRANS_TYPE} || '') . ($self->{REBILL_ID} || '');
        $TAMPER_PROOF_SEAL = md5_hex $TAMPER_PROOF_DATA;
    }
    return $TAMPER_PROOF_SEAL;
}

=head2 Post

Posts the data to the BluePay::BluePayPayment interface

=cut

sub Post {
    my $self = shift; 
    $TAMPER_PROOF_SEAL =  $self->calc_tps();
    $self->{REMOTE_IP} = $ENV{'REMOTE_ADDR'};
    # Create request (encode)
    my $request = $self->{URL} . "\?FIELDS=" . "&TAMPER_PROOF_SEAL=" . uri_escape($TAMPER_PROOF_SEAL || '');
	while ( my ($key, $value) = each(%$self) ) { 
		if ($key eq 'SECRET_KEY') { next; }
		if ($key eq 'URL') { next; }  
		$request .= "&$key=" . uri_escape($value || ''); 
	}
    # Create Agent
    my $ua = new LWP::UserAgent;
    my $content;
    if ( $self->{URL} ne 'https://secure.bluepay.com/interfaces/bp10emu') { 
        my $response = $ua->get("$request");
        $content = $response->content;
        chomp $content;
	if ( $self->{URL} ne 'https://secure.bluepay.com/interfaces/bpdailyreport2') { 
	    return $content;
	}
        # Parse Response
	# Split the name-value pairs
	my @pairs = split(/&/, $content);
	foreach my $pair (@pairs) {
            my ($name, $value) = split(/=/, $pair);
	    if ($value) {
              $value =~ tr/+/ /;  
	      $value =~ s/%([a-fA-F0-9][a-fA-F0-9])/pack("C", hex($1))/eg;
              $self->{$name} = $value;
	    }
	}
        return $content;

    } else {
        my $req = new HTTP::Request 'POST', $self->{URL}; 
        $req->content($request); 
        my $response = $ua->request($req);
        my $content_string = $response->header("Location");
        my @content_split = split /[?]/, $content_string;
        my $content = $content_split[1];
        # Parse Response
        # Split the name-value pairs
        my @pairs = split(/&/, $content);
        foreach my $pair (@pairs) {
            my ($name, $value) = split(/=/, $pair);
	    if ($value) {
                $value =~ tr/+/ /;  
	        $value =~ s/%([a-fA-F0-9][a-fA-F0-9])/pack("C", hex($1))/eg;
                $self->{$name} = $value;
	    }
        }        
    }
}

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

=cut

1;
