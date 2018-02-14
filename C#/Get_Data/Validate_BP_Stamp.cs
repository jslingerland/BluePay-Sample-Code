/*
* BluePay C#.NET Sample code.
*
* This code sample reads the values from a BP10emu redirect
* and authenticates the message using the the BP_STAMP
* provided in the response. Point the REDIRECT_URL of your 
* BP10emu request to the URI prefix specified below.
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
    class ValidateBPStamp
    {
        public static void Main()
        {
            string accountID = "Merchant's Account ID Here";
            string secretKey = "Merchant's Secret Key Here";
            string mode = "TEST";
            string prefix = "Merchant's Target URI Prefix Here" // Set the listener URI (and port if necessary)

            HttpListener listener = new HttpListener();
            listener.Prefixes.Add(prefix); 

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
                NameValueCollection responsePairs = context.Request.QueryString;
                string msg;
                if (responsePairs["BP_STAMP"] != null) // Check whether BP_STAMP is provided
                {
                    BluePay bp = new BluePay
                    (
                       accountID,
                       secretKey,
                       mode
                    );
                    
                    string bpStampString = "";
                    string[] defSeparator = new string[] { " " };
                    string[] bpStampFields = responsePairs["BP_STAMP_DEF"].Split(defSeparator, StringSplitOptions.RemoveEmptyEntries); // Split BP_STAMP_DEF on whitespace
                    foreach (string field in bpStampFields)
                    {
                        bpStampString += responsePairs[field]; // Concatenate values used to calculate expected BP_STAMP
                    }
                    string expectedStamp = bp.GenerateTPS(bpStampString, responsePairs["TPS_HASH_TYPE"]); // Calculate expected BP_STAMP using hash function specified in response
                    if (expectedStamp == responsePairs["BP_STAMP"]) // Compare expected BP_STAMP with received BP_STAMP
                    {
                        // Validate BP_STAMP and reads the response results
                        msg = "VALID BP_STAMP: TRUE\n";
                        foreach (string key in responsePairs)
                        {
                            msg += (key + ": " + responsePairs[key] + "\n");
                        }
                    }
                    else
                    {
                        msg = "ERROR: BP_STAMP VALUES DO NOT MATCH\n";
                    }
                } else
                {
                    msg = "ERROR: BP_STAMP NOT FOUND. CHECK MESSAGE & RESPONSEVERSION";
                }
                context.Response.ContentLength64 = Encoding.UTF8.GetByteCount(msg);
                context.Response.StatusCode = 200;
                using (Stream stream = context.Response.OutputStream)
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(msg);
                    }
                }
            }
            listener.Close();
        }
    }
}