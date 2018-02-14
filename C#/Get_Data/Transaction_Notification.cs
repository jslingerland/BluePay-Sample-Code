/*
* BluePay C#.NET Sample code.
*
* This code sample shows a very based approach
* on handling data that is posted to a script running
* a merchant's server after a transaction is processed
* through their BluePay gateway account.
*/

using System;
using BluePayLibrary;
using System.Collections.Specialized;
using System.Web;
using System.Net;
using System.IO;
using System.Text;

namespace GetData
{
    class TransactionNotification
    {
        public static void Main()
        {
            string accountID = "Merchant's Account ID Here";
            string secretKey = "Merchant's Secret Key Here";
            string mode = "TEST";
            
            BluePay tps = new BluePay
            (
                accountID,
                secretKey,
                mode
            );

            HttpListener listener = new HttpListener();
            string response = "";

            try
            {
                // Listen for incoming data
                listener.Start();
            }
            catch (HttpListenerException)
            {
                return;
            }
            while (listener.IsListening)
            {
                var context = listener.GetContext();
                var body = new StreamReader(context.Request.InputStream).ReadToEnd();

                byte[] b = Encoding.UTF8.GetBytes("ACK");

                // Return HTTP Status of 200 to BluePay
                context.Response.StatusCode = 200;
                context.Response.KeepAlive = false;
                context.Response.ContentLength64 = b.Length;

                var output = context.Response.OutputStream;
                output.Write(b, 0, b.Length);

                // Get Reponse
                using (StreamReader reader = new StreamReader(output))
                {
                    response = reader.ReadToEnd();
                }
                context.Response.Close();
            }
            listener.Close();
            NameValueCollection vals = HttpUtility.ParseQueryString(response);

            // Parse data into a NVP collection
            string transID = vals["trans_id"];
            string transStatus = vals["trans_stats"];
            string transType = vals["trans_type"];
            string amount = vals["amount"];
            string rebillID = vals["rebill_id"];
            string rebillAmount = vals["rebill_amount"];
            string rebillStatus = vals["rebill_status"];
            string tpsHashType = vals["TPS_HASH_TYPE"];
            string bpStamp = vals["BP_STAMP"];
            string bpStampDef = vals["BP_STAMP_DEF"];

            // calculate the expected BP_STAMP
            string bpStampString = "";
            string[] defSeparator = new string[] { " " };
            string[] bpStampFields = bpStampDef.Split(defSeparator, StringSplitOptions.RemoveEmptyEntries);
            foreach (string field in bpStampFields)
            {
                bpStampString += vals[field];
            }
            string expectedStamp = tps.GenerateTPS(bpStampString, tpsHashType);

            // Output data if the expected BP_STAMP matches the actual BP_STAMP
            if (expectedStamp == bpStamp) {
                Console.Write("Transaction ID: " + transID);
                Console.Write("Transaction Status: " + transStatus);
                Console.Write("Transaction Type: " + transType);
                Console.Write("Transaction Amount: " + amount);
                Console.Write("Rebill ID: " + rebillID);
                Console.Write("Rebill Amount: " + rebillAmount);
                Console.Write("Rebill Status: " + rebillStatus);
            } else {
                Console.Write("ERROR IN RECEIVING DATA FROM BLUEPAY");
            }
        }
    }
}