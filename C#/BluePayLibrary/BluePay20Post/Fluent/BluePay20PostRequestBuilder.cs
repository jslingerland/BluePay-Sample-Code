using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace BluePayLibrary.Interfaces.BluePay20Post.Fluent
{
    public class BluePay20PostRequestBuilder : IBluePay20PostRequestBuilder, IBluePay20PostRequestBuilderFields
    {
        private readonly Dictionary<string, string> _fields = new Dictionary<string, string>();

        public BluePay20PostRequestBuilder()
        {
            Version("3");
            TamperProofSealFields = new List<string>
            {
                "ACCOUNT_ID",
                "TRANS_TYPE",
                "AMOUNT",
                "MASTER_ID",
                "NAME1",
                "PAYMENT_ACCOUNT"
            };
        }

        public string this[string index]
        {
            get
            {
                string ret;
                _fields.TryGetValue(index, out ret);
                return ret;
            }
            set { _fields[index] = value; }
        }

        /// <summary>
        /// The FULL amount of the transaction, including tax and tip. (XXXXXXXX.XX format)
        /// </summary>
        public IBluePay20PostRequestBuilderFields Amount(decimal value)
        {
            this["AMOUNT"] = value.ToString("0.00", NumberFormatInfo.InvariantInfo);
            return this;
        }

        /// <summary>
        /// Your 12-digit Bluepay 2.0 Account ID
        /// </summary>
        public IBluePay20PostRequestBuilderFields AccountId(string value)
        {
            this["ACCOUNT_ID"] = value;
            return this;
        }

        /// <summary>
        /// Optional.  Your 12-digit Bluepay 2.0 User ID
        /// </summary>
        public IBluePay20PostRequestBuilderFields UserId(string value)
        {
            this["USER_ID"] = value;
            return this;
        }

        /// <summary>
        /// Optional(defaults to SALE). AUTH, SALE, REFUND, CAPTURE, VOID, UPDATE, CREDIT, AGG
        /// </summary>
        public IBluePay20PostRequestBuilderFields TransType(TransactionType value)
        {
            this["TRANS_TYPE"] = value.ToString();
            return this;
        }

        /// <summary>
        /// Rebilling flag. Only used for non-BluePay generated rebillings to identify the
        /// transaction as a rebilling. Set value to 1 for rebill transaction.
        /// </summary>
        public IBluePay20PostRequestBuilderFields FRebilling(bool value)
        {
            this["F_REBILLING"] = value ? "1" : "0";
            return this;
        }

        /// <summary>
        /// Optional. CREDIT or ACH or DEBIT (Defaults to CREDIT)
        /// </summary>
        public IBluePay20PostRequestBuilderFields PaymentType(PaymentType value)
        {
            this["PAYMENT_TYPE"] = value.ToString();
            return this;
        }
        /// <summary>
        /// Optional. TEST or LIVE (Defaults to TEST)
        /// </summary>
        public IBluePay20PostRequestBuilderFields Mode(Mode value)
        {
            this["MODE"] = value.ToString();
            return this;
        }

        /// <summary>
        /// Optional.  The TRANS_ID of a previous transaction; any parameters not sent will be
        /// filled out from the previous transaction.  This allows you to run a "manual" rebilling.The results of a test transaction are determined by the dollar amount of the transaction without cents. If the dollar amount is odd a approval is returned. If the dollar amount is even a decline is returned.
        /// REQUIRED for CAPTURE or REFUND; contains the TRANS_ID of the transaction to CAPTURE or
        /// REFUND.
        /// </summary>
        public IBluePay20PostRequestBuilderFields MasterId(string value)
        {
            this["MASTER_ID"] = value;
            return this;
        }

        /// <summary>
        /// Authorization code for a force transaction (voice auth).
        /// 
        /// </summary>
        public IBluePay20PostRequestBuilderFields AuthCode(string value)
        {
            this["AUTH_CODE"] = value;
            return this;
        }

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
        public IBluePay20PostRequestBuilderFields PaymentAccount(string value)
        {
            this["PAYMENT_ACCOUNT"] = value;
            return this;
        }
        
        /// <summary>
        /// The documentation for the ACH transaction.  May be 'PPD', 'CCD', 'TEL', 'WEB', or 'ARC'.  Defaults to 'WEB' if not set.
        /// </summary>
        public IBluePay20PostRequestBuilderFields DocType(DocType value)
        {
            this["DOC_TYPE"] = value.ToString();
            return this;
        }

        #region creditcard
        
        /// <summary>
        /// The customer's Card Validation Code.  This is the three-digit code printed on the
        /// back of most credit cards.
        /// </summary>
        public IBluePay20PostRequestBuilderFields CardCvv2(string value)
        {
            this["CARD_CVV2"] = value;
            return this;
        }

        /// <summary>
        /// The customer's Credit Card Expiration Date, in MMYY format.
        /// </summary>
        public IBluePay20PostRequestBuilderFields CardExpire(string value)
        {
            this["CARD_EXPIRE"] = value;
            return this;
        }

        /// <summary>
        /// The customer's Credit Card Expiration Date, in MMYY format.
        /// </summary>
        public IBluePay20PostRequestBuilderFields CardExpire(DateTime value)
        {
            this["CARD_EXPIRE"] = value.ToString("MMyy");
            return this;
        }

        #endregion



        /// <summary>
        /// Unencrypted Swipe data
        /// </summary>
        public IBluePay20PostRequestBuilderFields UnencryptedSwipe(UnencryptedSwipe value)
        {
            if (!string.IsNullOrEmpty(value.Swipe))
                this["SWIPE"] = value.Swipe;

            if(!string.IsNullOrEmpty(value.Track2))
                this["TRACK2"] = value.Track2;

            return this;
        }

        /// <summary>
        /// Encrypted Swipe data
        /// </summary>
        public IBluePay20PostRequestBuilderFields EncryptedSwipe(EncryptedSwipe value)
        {
            this["KSN"] = value.KeySerialNumber;
            this["TRACK1_ENC"] = value.Track1Enc;
            this["TRACK1_EDL"] = value.Track1Edl;
            this["TRACK2_ENC"] = value.Track2Enc;
            this["TRACK2_EDL"] = value.Track2Edl;

            return this;
        }

        #region OptionalFields
        /// <summary>
        /// Set to '1' if this is a corporate transaction, rather than personal.
        /// </summary>
        public IBluePay20PostRequestBuilderFields IsCorporate(bool value)
        {
            this["IS_CORPORATE"] = value ? "1" : "0";
            return this;
        }

        /// <summary>
        /// The name of the company on the check or credit card.  (64 characters)
        /// </summary>
        public IBluePay20PostRequestBuilderFields CompanyName(string value)
        {
            this["COMPANY_NAME"] = value;
            return this;
        }

        #region CustomerInfo
        /// <summary>
        /// The customer's first name (32 characters)
        /// </summary>
        public IBluePay20PostRequestBuilderFields Name1(string value)
        {
            this["NAME1"] = value;
            return this;
        }

        /// <summary>
        /// The customer's last name (32 characters)
        /// </summary>
        public IBluePay20PostRequestBuilderFields Name2(string value)
        {
            this["NAME2"] = value;
            return this;
        }

        /// <summary>
        /// The customer's street address, necessary for AVS. (64 Characters)
        /// </summary>
        public IBluePay20PostRequestBuilderFields Addr1(string value)
        {
            this["ADDR1"] = value;
            return this;
        }

        /// <summary>
        /// The customer's second address line, if any (64 Characters)
        /// </summary>
        public IBluePay20PostRequestBuilderFields Addr2(string value)
        {
            this["ADDR2"] = value;
            return this;
        }

        /// <summary>
        /// The customer's city (32 Characters)
        /// </summary>
        public IBluePay20PostRequestBuilderFields City(string value)
        {
            this["CITY"] = value;
            return this;
        }

        /// <summary>
        /// The customers' state, province, or equivalent. (16 Characters max; 2-letter abbrev preferred)
        /// </summary>
        public IBluePay20PostRequestBuilderFields State(string value)
        {
            this["STATE"] = value;
            return this;
        }

        /// <summary>
        /// The customer's zipcode or equivalent. (16 Characters)
        /// </summary>
        public IBluePay20PostRequestBuilderFields Zip(string value)
        {
            this["ZIP"] = value;
            return this;
        }

        /// <summary>
        /// The customer's country (64 Characters)
        /// </summary>
        public IBluePay20PostRequestBuilderFields Country(string value)
        {
            this["COUNTRY"] = value;
            return this;
        }

        /// <summary>
        /// The customer's email address.  (64 Characters)
        /// </summary>
        public IBluePay20PostRequestBuilderFields Email(string value)
        {
            this["EMAIL"] = value;
            return this;
        }

        /// <summary>
        /// The cusotmer's phone number.  (16 Characters)
        /// </summary>
        public IBluePay20PostRequestBuilderFields Phone(string value)
        {
            this["PHONE"] = value;
            return this;
        }
        #endregion

        /// <summary>
        /// 128-character descriptor field.
        /// </summary>
        public IBluePay20PostRequestBuilderFields Memo(string value)
        {
            this["MEMO"] = value;
            return this;
        }

        /// <summary>
        /// Merchant-specified data, searchable and reportable. (16 Characters)
        /// </summary>
        public IBluePay20PostRequestBuilderFields CustomId(string value)
        {
            this["CUSTOM_ID"] = value;
            return this;
        }

        /// <summary>
        /// Merchant-specified data, searchable and reportable. (64 Characters)
        /// </summary>
        public IBluePay20PostRequestBuilderFields CustomId2(string value)
        {
            this["CUSTOM_ID2"] = value;
            return this;
        }

        /// <summary>
        /// Set to 1 to turn off duplicate scrubbing for a transaction. Set to 0 or leave blank to process with duplicate srubbing.
        /// </summary>
        public IBluePay20PostRequestBuilderFields DuplicateOverride(bool value)
        {
            this["DUPLICATE_OVERRIDE"] = value ? "1" : "0";
            return this;
        }

        /// <summary>
        /// IP address of the customer. Used for velocity filtering. It is recommended that this value be sent.
        /// </summary>
        public IBluePay20PostRequestBuilderFields CustomerIp(string value)
        {
            this["CUSTOMER_IP"] = value;
            return this;
        }

        /// <summary>
        /// Determines fields included in output. See output section below.
        /// </summary>
        public IBluePay20PostRequestBuilderFields Version(string value)
        {
            this["VERSION"] = value;
            return this;
        }

        /// <summary>
        /// Optional Purchase Order ID (128 Chracters)
        /// </summary>
        public IBluePay20PostRequestBuilderFields OrderId(string value)
        {
            this["ORDER_ID"] = value;
            return this;
        }

        /// <summary>
        /// Optional Invoice ID (64 Characters)
        /// </summary>
        public IBluePay20PostRequestBuilderFields InvoiceId(string value)
        {
            this["INVOICE_ID"] = value;
            return this;
        }

        /// <summary>
        /// The tip amount, if any.
        /// </summary>
        public IBluePay20PostRequestBuilderFields AmountTip(decimal value)
        {
            this["AMOUNT_TIP"] = value.ToString("0.00", NumberFormatInfo.InvariantInfo);
            return this;
        }

        /// <summary>
        /// The tax amount, if any.
        /// </summary>
        public IBluePay20PostRequestBuilderFields AmountTax(decimal value)
        {
            this["AMOUNT_TAX"] = value.ToString("0.00", NumberFormatInfo.InvariantInfo); ;
            return this;
        }

        /// <summary>
        /// The amount of the total that was spent on food, for restaurants
        /// </summary>
        public IBluePay20PostRequestBuilderFields AmountFood(decimal value)
        {
            this["AMOUNT_FOOD"] = value.ToString("0.00", NumberFormatInfo.InvariantInfo); ;
            return this;
        }

        /// <summary>
        /// The amount of the total that was spent on beverages and other.
        /// </summary>
        public IBluePay20PostRequestBuilderFields AmountMisc(decimal value)
        {
            this["AMOUNT_MISC"] = value.ToString("0.00", NumberFormatInfo.InvariantInfo); ;
            return this;
        }

        /// <summary>
        /// 1 for rebilling, not set or 0 for no rebilling.
        /// </summary>
        public IBluePay20PostRequestBuilderFields DoRebill(bool value)
        {
            this["DO_REBILL"] = value ? "1" : "0";
            return this;
        }

        /// <summary>
        /// 1 for rebill transaction to be a Credit. Not set or 0 for Sale.
        /// </summary>
        public IBluePay20PostRequestBuilderFields RebIsCredit(bool value)
        {
            this["REB_IS_CREDIT"] = value ? "1" : "0";
            return this;
        }

        /// <summary>
        /// Date and time of first rebilling to run in ISO format (YYYY-MM-DD HH:MM:SS) or expression
        /// stating the amount of time from the current date. ("3 DAY" to run three days from
        /// transaction date, "1 MONTH" to run 1 month from transaction date, "1 YEAR" to run 1 year
        /// from transaction date, "2 WEEK" to 2 weeks from transaction date, etc.)
        /// Required for rebilling  Time portion is optional; will default to midnight on the
        /// date specified.
        /// </summary>
        public IBluePay20PostRequestBuilderFields RebFirstDate(string value)
        {
            this["REB_FIRST_DATE"] = value;
            return this;
        }

        /// <summary>
        /// How often to run rebilling, in date expression format ("3 DAY" to run every three days,
        /// "1 MONTH" to run monthly, "1 YEAR" to run yearly, "2 WEEK" to run bi-weekly, etc.) Required
        /// for rebilling.
        /// </summary>
        public IBluePay20PostRequestBuilderFields RebExpr(string value)
        {
            this["REB_EXPR"] = value;
            return this;
        }

        /// <summary>
        /// Optional.  How many times to rebill.  Not set to rebill until cancelled.
        /// </summary>
        public IBluePay20PostRequestBuilderFields RebCycles(string value)
        {
            this["REB_CYCLES"] = value;
            return this;
        }

        /// <summary>
        /// Optional.  How much to charge when rebillings are run.  Defaults to amount of transaction being run.
        /// </summary>
        public IBluePay20PostRequestBuilderFields RebAmount(decimal value)
        {
            this["REB_AMOUNT"] = value.ToString("0.00", NumberFormatInfo.InvariantInfo);
            return this;
        }

        /// <summary>
        /// The customer's SSN or TAXID, all digits (no dashes) (9 Characters).
        /// </summary>
        public IBluePay20PostRequestBuilderFields Ssn(string value)
        {
            this["SSN"] = value;
            return this;
        }

        /// <summary>
        /// The customer's birthdate in ISO format (YYYY-MM-DD)
        /// </summary>
        public IBluePay20PostRequestBuilderFields Birthdate(DateTime value)
        {
            this["BIRTHDATE"] = value.ToString("yyyy-MM-dd");
            return this;
        }

        /// <summary>
        /// The customer's state ID or driver's license.  (25 Characters)
        /// </summary>
        public IBluePay20PostRequestBuilderFields CustId(string value)
        {
            this["CUST_ID"] = value;
            return this;
        }

        /// <summary>
        /// The state of issuance of the customer's ID. (16 Characters)
        /// </summary>
        public IBluePay20PostRequestBuilderFields CustIdState(string value)
        {
            this["CUST_ID_STATE"] = value;
            return this;
        }

        #endregion

        #region DebitOnly
        /// <summary>
        /// Required, this is the encryption key information
        /// </summary>
        public IBluePay20PostRequestBuilderFields SmidId(string value)
        {
            this["SMID_ID"] = value;
            return this;
        }

        /// <summary>
        /// Required, this is the encrypted PIN
        /// </summary>
        public IBluePay20PostRequestBuilderFields PinBlock(string value)
        {
            this["PIN_BLOCK"] = value;
            return this;
        }

        /// <summary>
        /// Optional, amount of cash given to customer
        /// </summary>
        public IBluePay20PostRequestBuilderFields AmountCashback(decimal value)
        {
            this["AMOUNT_CASHBACK"] = value.ToString("0.00", NumberFormatInfo.InvariantInfo);
            return this;
        }

        /// <summary>
        /// Optional, surcharge for performing cash back
        /// </summary>
        public IBluePay20PostRequestBuilderFields AmountSurcharge(decimal value)
        {
            this["AMOUNT_SURCHARGE"] = value.ToString("0.00", NumberFormatInfo.InvariantInfo);
            return this;
        }

        #endregion

        /// <summary>
        /// This option allows a merchant to determine which fields are included in the
        /// TAMPER_PROOF_SEAL.If set blank or not sent, it will default to the fields
        /// as described under TAMPER_PROOF_SEAL, above.If set to a space-seperated list
        /// of field names, then the TPS will be calculated using the fields listed, in order.
        /// 
        /// The secret key is always the first field, and should not be listed
        /// 
        /// ** NOTICE: THE USE OF THIS FIELD CAN POSSIBLY WEAKEN YOUR SECURITY.  PLEASE
        /// BE SURE YOU UNDERSTAND HOW THE TAMPER_PROOF_SEAL WORKS BEFORE YOU CONSIDER
        /// USING THIS OPTION. **
        /// </summary>
        public List<string> TamperProofSealFields { get; set; }

        public IBluePay20PostRequestBuilder WithFields(Action<IBluePay20PostRequestBuilderFields> action)
        {
            action(this);
            return this;
        }

        /// <summary>
        /// Create the message
        /// </summary>
        /// <param name="secretKey">Secret Key</param>
        /// <returns></returns>
        public BluePayMessage ToMessage(string secretKey)
        {
            var values = new StringBuilder();

            values.Append(secretKey);
            foreach (var key in TamperProofSealFields)
            {
                values.Append(this[key] ?? "");
            }
            
            using (var hash = MD5.Create())
            {
                var data = hash.ComputeHash(Encoding.UTF8.GetBytes(values.ToString()));
                this["TAMPER_PROOF_SEAL"] = string.Join("", data.Select(b => b.ToString("x2")));
            }

            this["TPS_DEF"] = string.Join(" ", TamperProofSealFields);

            return new BluePayMessage(_fields.ToDictionary(kvp => kvp.Key, kvp => (object)kvp.Value));
        }
        
        public IBluePay20PostRequestBuilder ForCustomer(string name1, string name2 = null, string addr1 = null, string addr2 = null,
            string city = null, string state = null, string zip = null, string country = null, string phone = null,
            string email = null)
        {
            return (IBluePay20PostRequestBuilder)this.Name1(name1)
                .Name2(name2)
                .Addr1(addr1)
                .Addr2(addr2)
                .City(city)
                .State(state)
                .Zip(zip)
                .Country(country)
                .Phone(phone)
                .Email(email);
        }

        private static readonly Regex Track1And2Regex = new Regex(@"(%B)\d{0,19}\^([\w\s]*)\/([\w\s]*)([\s]*)\^\d{7}\w*\?;\d{0,19}=\d{7}\w*\?");
        private static readonly Regex Track2OnlyRegex = new Regex(@";\d{0,19}=\d{7}\w*\?");

        public IBluePay20PostRequestBuilder FromSwipe(string swipedata)
        {
            string swipe = null;
            string track2 = null;

            if (Track1And2Regex.Match(swipedata).Success)
                swipe = swipedata;
            else if (Track2OnlyRegex.Match(swipedata).Success)
                track2 = swipedata;

            this.PaymentType(Interfaces.BluePay20Post.PaymentType.CREDIT)
                .UnencryptedSwipe(new UnencryptedSwipe()
                {
                    Swipe = swipe,
                    Track2 = track2
                });

            return this;
        }

        public IBluePay20PostRequestBuilder FromCreditCard(string cardNumber, DateTime expiration, string cvv2)
        {
            this.PaymentType(Interfaces.BluePay20Post.PaymentType.CREDIT)
                .PaymentAccount(cardNumber)
                .CardExpire(expiration)
                .CardCvv2(cvv2);

            return this;
        }

        public IBluePay20PostRequestBuilder FromCreditCard(string cardNumber, string expiration, string cvv2)
        {
            this.PaymentType(Interfaces.BluePay20Post.PaymentType.CREDIT)
                .PaymentAccount(cardNumber)
                .CardExpire(expiration)
                .CardCvv2(cvv2);

            return this;
        }

        public IBluePay20PostRequestBuilder Sale(decimal amount, string transactionId=null)
        {
            this.TransType(TransactionType.SALE)
                .Amount(amount);

            if (!string.IsNullOrWhiteSpace(transactionId))
                this.MasterId(transactionId);

            return this;
        }

        public IBluePay20PostRequestBuilder Auth(decimal amount, string transactionId = null)
        {
            this.TransType(TransactionType.AUTH)
                .Amount(amount);

            if (!string.IsNullOrWhiteSpace(transactionId))
                this.MasterId(transactionId);

            return this;
        }

        public IBluePay20PostRequestBuilder Refund(string transactionId, decimal? amount = null)
        {
            this.TransType(TransactionType.REFUND)
                .MasterId(transactionId);

            if (amount != null)
                this.Amount(amount.Value);

            return this;
        }

        public IBluePay20PostRequestBuilder Capture(string transactionId, decimal? amount = null)
        {
            this.TransType(TransactionType.CAPTURE)
                .MasterId(transactionId);

            if (amount != null)
                this.Amount(amount.Value);

            return this;
        }

        public IBluePay20PostRequestBuilder Void(string transactionId)
        {
            this.TransType(TransactionType.VOID)
                .MasterId(transactionId);
            return this;
        }

        public IBluePay20PostRequestBuilder FromAch(string routingNumber, string accountNumber, AccountType accountType, DocType docType)
        {
            var acctType = accountType == AccountType.Checking ? "C" : "S";

            this.PaymentType(Interfaces.BluePay20Post.PaymentType.ACH)
                .PaymentAccount(string.Format("{0}:{1}:{2}", acctType, routingNumber, accountNumber))
                .DocType(docType);
            return this;
        }
    }
}