namespace BluePayLibrary.Interfaces.BluePay20Post
{
    public class BluePayPost20ResponseV1 : BluePayPost20Response
    {
        /// <summary>
        /// The auth code for successful AUTH
        /// </summary>
        public string AuthCode { get; set; }

        /// <summary>
        /// The used credit card or ACH account, masked with 'X' as appropriate
        /// </summary>
        public string PaymentAccountMask { get; set; }

        /// <summary>
        /// VISA, MC, DISC, AMEX, ACH, etc
        /// </summary>
        public string CardType { get; set; }
    }
}