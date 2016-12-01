namespace BluePayLibrary.Interfaces.BluePay20Post
{
    public class BluePayPost20Response : BluePayMessage
    {
        /// <summary>
        /// The ID assigned to the transaction by the gateway
        /// 
        /// </summary>
        public string TransId { get; set; }

        /// <summary>
        /// '1' for APPROVED
        /// '0' for DECLINE
        /// 'E' and all other responses are ERROR.
        /// Note: If a transaction is blocked by Duplicate Scrub STATUS will be
        /// 1 and MESSAGE will be DUPLICATE. All other fields will be
        /// from the original transaction, not the current transaction.
        /// Note: The results of a test transaction are determined by the
        /// dollar portion of the amount without cents. If the dollars
        /// are odd an approval is returned. If the dollars are even a
        /// decline is returned
        /// 
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Whether or not transaction is approved
        /// </summary>
        public bool IsApproved
        {
            get { return Status == "1"; }
        }

        /// <summary>
        /// Whether or not transaction is an error
        /// </summary>
        public bool IsError
        {
            get { return Status != "1" && Status != "0"; }
        }

        /// <summary>
        /// Address Verification System (AVS) response code received on the transaction. Possible codes are:
        /// A - Partial match - Street Address matches, ZIP Code does not
        /// B - International street address match, postal code not verified due to incompatible formats
        /// C - International street address and postal code not verified due to incompatible formats
        /// D - International street address and postal code match
        /// E - Not a mail or phone order
        /// F - Address and Postal Code match (UK only)
        /// G - Service Not supported, non-US Issuer does not participate
        /// I - Address information not verified for international transaction
        /// M - Address and Postal Code match
        /// N - No match - No Address or ZIP Code match
        /// P - International postal code match, street address not verified due to incompatible format
        /// Q - Bill to address did not pass edit checks/Card Association can't verify the authentication of an address
        /// R - Retry - Issuer system unavailable, retry later
        /// S - Service not supported
        /// W - Partial match - ZIP Code matches, Street Address does not
        /// U - Unavailable - Address information is unavailable for that account number, or the card issuer does not support
        /// X - Exact match, 9 digit zip - Street Address, and 9 digit ZIP Code match
        /// Y - Exact match, 5 digit zip - Street Address, and 5 digit ZIP Code match
        /// Z - Partial match - 5 digit ZIP Code match only
        /// 1 - Cardholder name matches
        /// 2 - Cardholder name, billing address, and postal code match
        /// 3 - Cardholder name and billing postal code match
        /// 4 - Cardholder name and billing address match
        /// 5 - Cardholder name incorrect, billing address and postal code match
        /// 6 - Cardholder name incorrect, billing postal code matches
        /// 7 - Cardholder name incorrect, billing address matches
        /// 8 - Cardholder name, billing address, and postal code are all incorrect
        /// Note: When a transaction is processed with MODE=TEST if the first character of ADDR1 is one of the possible AVS
        /// response codes that value will be returned as the AVS response value.
        /// 
        /// </summary>
        public string Avs { get; set; }

        /// <summary>
        /// Card Verification Value 2 response code. Result of the validation of the CVV2 value entered by the payer. Possible response codes are:
        /// _ = Unsupported on this network or transaction type
        /// M = CVV2 Match
        /// N = CVV2 did not match
        /// P = CVV2 was not processed
        /// S = CVV2 exists but was not input
        /// U = Card issuer does not provide CVV2 service
        /// X = No response from association
        /// Y = CVV2 Match (Amex only when processed through BluePay Canada)
        /// Note: When a transaction is processed with MODE=TEST if the first character of ADDR2 is one of the possible CVV2
        /// response codes that value will be returned as the CVV2 response value.
        /// 
        /// </summary>
        public string Cvv2 { get; set; }

        /// <summary>
        /// A human-readable description of the transaction status
        /// 
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The ID of the newly-created rebilling sequence, if any
        /// 
        /// </summary>
        public string Rebid { get; set; }

        /// <summary>
        /// The transaction type. The only time it should be different from the
        /// requested TRANS_TYPE is in the case of VOID promotion where a
        /// REFUND was sent and we returned a VOID
        /// </summary>
        public TransactionType TransType { get; set; }
    }
}