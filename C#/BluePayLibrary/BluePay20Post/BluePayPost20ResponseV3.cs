namespace BluePayLibrary.Interfaces.BluePay20Post
{
    public class BluePayPost20ResponseV3 : BluePayPost20ResponseV2
    {
        /// <summary>
        /// Up to 64 characters containing the customer's bank name
        /// 
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// Country of credit card issuer
        /// 
        /// </summary>
        public string CardCountry { get; set; }

        /// <summary>
        /// Transaction information returned by credit card processing network.
        /// </summary>
        public BinData Bindata { get; set; }
    }
}