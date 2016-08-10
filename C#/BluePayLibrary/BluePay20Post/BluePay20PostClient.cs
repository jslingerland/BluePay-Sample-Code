using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BluePayLibrary.Interfaces.BluePay20Post
{
    public class BluePay20PostClient : IBluePay20PostClient
    {
        private readonly string _endpoint;
        private readonly IBluePayResponseParser<BluePayPost20ResponseV3> _parser;
        private readonly WebRequestProvider _webRequestProvider;

        public delegate BluePayPost20ResponseV3 BluePayParser(TextReader tr);
        public delegate Task<BluePayPost20ResponseV3> BluePayParserAsync(TextReader tr);
        public delegate WebRequest WebRequestProvider(string url);

        public BluePay20PostClient()
        {
            _endpoint = "https://secure.bluepay.com/interfaces/bp20post";
            _parser = new BluePayResponseParser<BluePayPost20ResponseV3>();
            _webRequestProvider = (url) =>
            {
                var ret = WebRequest.CreateHttp(url);
                ret.Method = "POST";
                ret.ContentType = "application/x-www-form-urlencoded";
                return ret;
            };
        }

        public BluePay20PostClient(
            string endpoint,
            IBluePayResponseParser<BluePayPost20ResponseV3> parser,
            WebRequestProvider webRequestProvider)
        {
            _endpoint = endpoint;
            _parser = parser;
            _webRequestProvider = webRequestProvider;
        }
        
        public BluePayPost20ResponseV3 Process(BluePayMessage msg)
        {
            try
            {
                var req = _webRequestProvider(_endpoint);
                using (var writer = new StreamWriter(req.GetRequestStream()))
                {
                    var isFirst = true;
                    foreach (var kvp in msg.GetFields())
                    {
                        if (!isFirst)
                            writer.Write("&");
                        else
                            isFirst = false;
                            
                        writer.Write(HttpUtility.UrlEncode(kvp.Key));
                        writer.Write('=');
                        writer.Write(HttpUtility.UrlEncode(kvp.Value?.ToString()));
                    }
                }
                var response = req.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    return _parser.Parse(reader);
                }
            }
            catch (WebException e)
            {
                using (var response = e.Response)
                {
                    if (response == null)
                        throw;
                    
                    using (var stream = response.GetResponseStream())
                    {
                        if (stream == null)
                            throw new BluePayException(e.Message, e); ;

                        using (var reader = new StreamReader(stream))
                        {
                            return _parser.Parse(reader);
                        }
                    }
                }
            }
        }

        public async Task<BluePayPost20ResponseV3> ProcessAsync(BluePayMessage msg)
        {
            try
            {
                var req = _webRequestProvider(_endpoint);
                using (var writer = new StreamWriter(req.GetRequestStream()))
                {
                    var delimeter = "";
                    foreach (var kvp in msg.GetFields())
                    {
                        var key = HttpUtility.UrlEncode(kvp.Key);
                        var value = HttpUtility.UrlEncode(kvp.Value?.ToString());
                        
                        await writer.WriteAsync($"{delimeter}{key}={value}");
                        delimeter = "&";
                    }
                }
                var response = await req.GetResponseAsync();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    return await _parser.ParseAsync(reader);
                }
            }
            catch (WebException e)
            {
                using (var response = e.Response)
                {
                    if (response == null)
                        throw;
                    
                    using (var stream = response.GetResponseStream())
                    {
                        if (stream == null)
                            throw new BluePayException(e.Message, e); ;

                        using (var reader = new StreamReader(stream))
                        {
                            return await _parser.ParseAsync(reader);
                        }
                    }
                }
            }
        }
    }

    public class BluePayException : IOException
    {
        public BluePayException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
