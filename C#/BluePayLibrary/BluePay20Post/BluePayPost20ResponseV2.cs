namespace BluePayLibrary.Interfaces.BluePay20Post
{
    public class BluePayPost20ResponseV2 : BluePayPost20ResponseV1
    {
        /// <summary>
        /// Bank Identification Number, first six digits of the credit card number used.Identifies the bank that issued the card.
        /// </summary>
        public string Bin { get; set; }
    }
}