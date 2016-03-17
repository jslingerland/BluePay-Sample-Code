#!/usr/bin/env perl

use strict;
use warnings;
use BluePayPayment_BP10Emu;
#use FindBin '$Bin';
#use lib "$FindBin::Bin/../../lib";
#use Data::Dump 'dump';
#use Cetec::Reboot::Config;


my $payment = BluePay::BluePayPayment_BP10Emu->new();

my $account = "100229179297";
my $sekrit  = "NGCDVZ8PIJWJ4UO97RZ4MEE1RTQNTGTK";
my $mode    = "TEST";
my $name1   = "Test";
my $name2   = "Card";
my $address_name = "2525 S Lamar";
my $street1      = "Suite 10";
my $city         = "Austin";
my $state        = "TX";
my $zip          = "78704";
my $country      = "USA";
my $payment_type = "CREDIT";
my $amount       = "15";
my $decrypt_ccnum = "4111111111111111";
my $cc_exp        = "0820";

    # Merchant values
    $payment->{MERCHANT}    = $account;
    $payment->{SECRET_KEY}  = $sekrit;
    $payment->{MODE}        = $mode;

    # Customer values
    $payment->{NAME1} = $name1 || '';
    $payment->{NAME2} = $name2 || '';
    $payment->{ADDR1} = $address_name;
    $payment->{ADDR2} = $street1;
    $payment->{CITY}  = $city;
    $payment->{STATE} = $state;
    $payment->{ZIPCODE} = $zip;
    $payment->{COUNTRY} = $country ;

    # Payment values
    $payment->{TRANSACTION_TYPE}= 'SALE';
    $payment->{PAYMENT_TYPE}    = $payment_type;
    $payment->{AMOUNT}          = $amount;
    $payment->{CC_NUM}          = $decrypt_ccnum;
    $payment->{CC_EXPIRES}      = $cc_exp;

    $payment->Post();

warn "Response MESSAGE: $payment->{MESSAGE}";
exit();
