namespace BluePayLibrary.Interfaces.BluePay20Post
{
    public class UnencryptedSwipe
    {
        /// <summary>
        /// The full swiped track data, just the way it comes to you from the card reader,
        /// including both Track1 and Track2.
        /// </summary>
        public string Swipe { get; set; }

        /// <summary>
        /// Only Track2 of the swiped data.   
        /// </summary>
        public string Track2 { get; set; }
    }
}