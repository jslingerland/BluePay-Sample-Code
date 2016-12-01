namespace BluePayLibrary.Interfaces.BluePay20Post
{
    public class EncryptedSwipe
    {
        /// <summary>
        /// Key Serial Number
        /// </summary>
        public string KeySerialNumber { get; set; }

        /// <summary>
        /// Track 1 data encrypted
        /// </summary>
        public string Track1Enc { get; set; }

        /// <summary>
        /// Pre-encryption track 1 length
        /// </summary>
        public string Track1Edl { get; set; }

        /// <summary>
        /// Track 2 data encrypted
        /// </summary>
        public string Track2Enc { get; set; }

        /// <summary>
        /// Pre-encryption track 2 length
        /// </summary>
        public string Track2Edl { get; set; }
    }
}