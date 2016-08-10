using System;

namespace BluePayLibrary.Interfaces.BluePay20Post.Fluent
{
    public interface IBluePay20PostRequestBuilderFields
    {
        /// <summary>
        /// The FULL amount of the transaction, including tax and tip. (XXXXXXXX.XX format)
        /// </summary>
        IBluePay20PostRequestBuilderFields Amount(decimal value);

        /// <summary>
        /// Your 12-digit Bluepay 2.0 Account ID
        /// </summary>
        IBluePay20PostRequestBuilderFields AccountId(string value);

        /// <summary>
        /// Optional.  Your 12-digit Bluepay 2.0 User ID
        /// </summary>
        IBluePay20PostRequestBuilderFields UserId(string value);

        /// <summary>
        /// Optional(defaults to SALE). AUTH, SALE, REFUND, CAPTURE, VOID, UPDATE, CREDIT, AGG
        /// </summary>
        IBluePay20PostRequestBuilderFields TransType(TransactionType value);

        /// <summary>
        /// Rebilling flag. Only used for non-BluePay generated rebillings to identify the
        /// transaction as a rebilling. Set value to 1 for rebill transaction.
        /// </summary>
        IBluePay20PostRequestBuilderFields FRebilling(bool value);

        /// <summary>
        /// Optional. CREDIT or ACH or DEBIT (Defaults to CREDIT)
        /// </summary>
        IBluePay20PostRequestBuilderFields PaymentType(PaymentType value);

        /// <summary>
        /// Optional. TEST or LIVE (Defaults to TEST)
        /// </summary>
        IBluePay20PostRequestBuilderFields Mode(Mode value);

        /// <summary>
        /// Optional.  The TRANS_ID of a previous transaction; any parameters not sent will be
        /// filled out from the previous transaction.  This allows you to run a "manual" rebilling.The results of a test transaction are determined by the dollar amount of the transaction without cents. If the dollar amount is odd a approval is returned. If the dollar amount is even a decline is returned.
        /// REQUIRED for CAPTURE or REFUND; contains the TRANS_ID of the transaction to CAPTURE or
        /// REFUND.
        /// </summary>
        IBluePay20PostRequestBuilderFields MasterId(string value);

        /// <summary>
        /// Authorization code for a force transaction (voice auth).
        /// 
        /// </summary>
        IBluePay20PostRequestBuilderFields AuthCode(string value);

        /// <summary>
        /// The customer's credit card number.  (eg: '4111111111111111')
        /// or for an ACH
        /// 
        /// This is set to three colon-seperated fields.First, the account
        ///     type must be 'C' for checking, or 'S' for savings.Second, the
        ///     bank's routing number, and finally the customer's account number.
        ///     (eg: 'C:123456789:1234123412341234')
        /// 
        /// </summary>
        IBluePay20PostRequestBuilderFields PaymentAccount(string value);

        /// <summary>
        /// The documentation for the ACH transaction.  May be 'PPD', 'CCD', 'TEL', 'WEB', or 'ARC'.  Defaults to 'WEB' if not set.
        /// </summary>
        IBluePay20PostRequestBuilderFields DocType(DocType value);

        /// <summary>
        /// The customer's Card Validation Code.  This is the three-digit code printed on the
        /// back of most credit cards.
        /// </summary>
        IBluePay20PostRequestBuilderFields CardCvv2(string value);

        /// <summary>
        /// The customer's Credit Card Expiration Date, in MMYY format.
        /// </summary>
        IBluePay20PostRequestBuilderFields CardExpire(string value);

        /// <summary>
        /// The customer's Credit Card Expiration Date, in MMYY format.
        /// </summary>
        IBluePay20PostRequestBuilderFields CardExpire(DateTime value);

        /// <summary>
        /// Unencrypted Swipe data
        /// </summary>
        IBluePay20PostRequestBuilderFields UnencryptedSwipe(UnencryptedSwipe value);

        /// <summary>
        /// Encrypted Swipe data
        /// </summary>
        IBluePay20PostRequestBuilderFields EncryptedSwipe(EncryptedSwipe value);

        /// <summary>
        /// Set to '1' if this is a corporate transaction, rather than personal.
        /// </summary>
        IBluePay20PostRequestBuilderFields IsCorporate(bool value);

        /// <summary>
        /// The name of the company on the check or credit card.  (64 characters)
        /// </summary>
        IBluePay20PostRequestBuilderFields CompanyName(string value);

        /// <summary>
        /// The customer's first name (32 characters)
        /// </summary>
        IBluePay20PostRequestBuilderFields Name1(string value);

        /// <summary>
        /// The customer's last name (32 characters)
        /// </summary>
        IBluePay20PostRequestBuilderFields Name2(string value);

        /// <summary>
        /// The customer's street address, necessary for AVS. (64 Characters)
        /// </summary>
        IBluePay20PostRequestBuilderFields Addr1(string value);

        /// <summary>
        /// The customer's second address line, if any (64 Characters)
        /// </summary>
        IBluePay20PostRequestBuilderFields Addr2(string value);

        /// <summary>
        /// The customer's city (32 Characters)
        /// </summary>
        IBluePay20PostRequestBuilderFields City(string value);

        /// <summary>
        /// The customers' state, province, or equivalent. (16 Characters max; 2-letter abbrev preferred)
        /// </summary>
        IBluePay20PostRequestBuilderFields State(string value);

        /// <summary>
        /// The customer's zipcode or equivalent. (16 Characters)
        /// </summary>
        IBluePay20PostRequestBuilderFields Zip(string value);

        /// <summary>
        /// The customer's country (64 Characters)
        /// </summary>
        IBluePay20PostRequestBuilderFields Country(string value);

        /// <summary>
        /// The customer's email address.  (64 Characters)
        /// </summary>
        IBluePay20PostRequestBuilderFields Email(string value);

        /// <summary>
        /// The cusotmer's phone number.  (16 Characters)
        /// </summary>
        IBluePay20PostRequestBuilderFields Phone(string value);

        /// <summary>
        /// 128-character descriptor field.
        /// </summary>
        IBluePay20PostRequestBuilderFields Memo(string value);

        /// <summary>
        /// Merchant-specified data, searchable and reportable. (16 Characters)
        /// </summary>
        IBluePay20PostRequestBuilderFields CustomId(string value);

        /// <summary>
        /// Merchant-specified data, searchable and reportable. (64 Characters)
        /// </summary>
        IBluePay20PostRequestBuilderFields CustomId2(string value);

        /// <summary>
        /// Set to 1 to turn off duplicate scrubbing for a transaction. Set to 0 or leave blank to process with duplicate srubbing.
        /// </summary>
        IBluePay20PostRequestBuilderFields DuplicateOverride(bool value);

        /// <summary>
        /// IP address of the customer. Used for velocity filtering. It is recommended that this value be sent.
        /// </summary>
        IBluePay20PostRequestBuilderFields CustomerIp(string value);

        /// <summary>
        /// Determines fields included in output. See output section below.
        /// </summary>
        IBluePay20PostRequestBuilderFields Version(string value);

        /// <summary>
        /// Optional Purchase Order ID (128 Chracters)
        /// </summary>
        IBluePay20PostRequestBuilderFields OrderId(string value);

        /// <summary>
        /// Optional Invoice ID (64 Characters)
        /// </summary>
        IBluePay20PostRequestBuilderFields InvoiceId(string value);

        /// <summary>
        /// The tip amount, if any.
        /// </summary>
        IBluePay20PostRequestBuilderFields AmountTip(decimal value);

        /// <summary>
        /// The tax amount, if any.
        /// </summary>
        IBluePay20PostRequestBuilderFields AmountTax(decimal value);

        /// <summary>
        /// The amount of the total that was spent on food, for restaurants
        /// </summary>
        IBluePay20PostRequestBuilderFields AmountFood(decimal value);

        /// <summary>
        /// The amount of the total that was spent on beverages and other.
        /// </summary>
        IBluePay20PostRequestBuilderFields AmountMisc(decimal value);

        /// <summary>
        /// 1 for rebilling, not set or 0 for no rebilling.
        /// </summary>
        IBluePay20PostRequestBuilderFields DoRebill(bool value);

        /// <summary>
        /// 1 for rebill transaction to be a Credit. Not set or 0 for Sale.
        /// </summary>
        IBluePay20PostRequestBuilderFields RebIsCredit(bool value);

        /// <summary>
        /// Date and time of first rebilling to run in ISO format (YYYY-MM-DD HH:MM:SS) or expression
        /// stating the amount of time from the current date. ("3 DAY" to run three days from
        /// transaction date, "1 MONTH" to run 1 month from transaction date, "1 YEAR" to run 1 year
        /// from transaction date, "2 WEEK" to 2 weeks from transaction date, etc.)
        /// Required for rebilling  Time portion is optional; will default to midnight on the
        /// date specified.
        /// </summary>
        IBluePay20PostRequestBuilderFields RebFirstDate(string value);

        /// <summary>
        /// How often to run rebilling, in date expression format ("3 DAY" to run every three days,
        /// "1 MONTH" to run monthly, "1 YEAR" to run yearly, "2 WEEK" to run bi-weekly, etc.) Required
        /// for rebilling.
        /// </summary>
        IBluePay20PostRequestBuilderFields RebExpr(string value);

        /// <summary>
        /// Optional.  How many times to rebill.  Not set to rebill until cancelled.
        /// </summary>
        IBluePay20PostRequestBuilderFields RebCycles(string value);

        /// <summary>
        /// Optional.  How much to charge when rebillings are run.  Defaults to amount of transaction being run.
        /// </summary>
        IBluePay20PostRequestBuilderFields RebAmount(decimal value);

        /// <summary>
        /// The customer's SSN or TAXID, all digits (no dashes) (9 Characters).
        /// </summary>
        IBluePay20PostRequestBuilderFields Ssn(string value);

        /// <summary>
        /// The customer's birthdate in ISO format (YYYY-MM-DD)
        /// </summary>
        IBluePay20PostRequestBuilderFields Birthdate(DateTime value);

        /// <summary>
        /// The customer's state ID or driver's license.  (25 Characters)
        /// </summary>
        IBluePay20PostRequestBuilderFields CustId(string value);

        /// <summary>
        /// The state of issuance of the customer's ID. (16 Characters)
        /// </summary>
        IBluePay20PostRequestBuilderFields CustIdState(string value);

        /// <summary>
        /// Required, this is the encryption key information
        /// </summary>
        IBluePay20PostRequestBuilderFields SmidId(string value);

        /// <summary>
        /// Required, this is the encrypted PIN
        /// </summary>
        IBluePay20PostRequestBuilderFields PinBlock(string value);

        /// <summary>
        /// Optional, amount of cash given to customer
        /// </summary>
        IBluePay20PostRequestBuilderFields AmountCashback(decimal value);

        /// <summary>
        /// Optional, surcharge for performing cash back
        /// </summary>
        IBluePay20PostRequestBuilderFields AmountSurcharge(decimal value);
    }
}