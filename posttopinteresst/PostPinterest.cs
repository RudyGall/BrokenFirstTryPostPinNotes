using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace posttopinteresst
{
    class PostPinterest
    {
        public static void Message(string text)
        {
            var oauth_token = "Ane9FdYP7YJNPdqqG8Rr5iTgW5utFYqud3VlVcBFrrB08mC4jAm_wDAAAiGTRa6zDAbAr7UAAAAA";
            //var oauth_token_secret =  "put stuff here";
            var oauth_consumer_key = "5021144650893801612";
            var oauth_consumer_secret = "ba3a73bd5aab2032c03535e2d0d29d059bc41a96d4efdf8e109ea32629fb2a95";

            var oauth_version = "2.0";
            var oauth_signature_method = "HMAC-SHA1";
            var oauth_nonce = Convert.ToBase64String(
                                              new ASCIIEncoding().GetBytes(
                                                   DateTime.Now.Ticks.ToString()));
            var timeSpan = DateTime.UtcNow
                                              - new DateTime(1970, 1, 1, 0, 0, 0, 0,
                                                   DateTimeKind.Utc);
            var oauth_timestamp = Convert.ToInt64(timeSpan.TotalSeconds).ToString();
            var resource_url = "https://api.pinterest.com/v1/pins/?board=GcSocialmediatest/Test&note=VisualStudioPost";
            var note = text;

            //var baseFormat = "oauth_consumer_key={0}&oauth_nonce={1}&oauth_signature_method={2}" +
            //    "&oauth_timestamp={3}&oauth_token={4}&oauth_version={5}";

            var baseFormat = "oauth_nonce={0}&oauth_signature_method={1}" +
                "&oauth_timestamp={2}&oauth_token={3}&oauth_version={4}";

            var baseString = string.Format(baseFormat,
                                        oauth_consumer_key,
                                        oauth_nonce,
                                        oauth_signature_method,
                                        oauth_timestamp,
                                        oauth_token,
                                        oauth_version,
                                        Uri.EscapeDataString(note)
                                        );

            baseString = string.Concat("POST&", Uri.EscapeDataString(resource_url),
                         "&", Uri.EscapeDataString(baseString));

            var compositeKey = string.Concat(Uri.EscapeDataString(oauth_consumer_secret),
                        "&", Uri.EscapeDataString(oauth_token));

            string oauth_signature;
            using (HMACSHA1 hasher = new HMACSHA1(Encoding.ASCII.GetBytes(compositeKey)))
            {
                oauth_signature = Convert.ToBase64String(
                    hasher.ComputeHash(Encoding.ASCII.GetBytes(baseString)));
            }

            var headerFormat = "OAuth oauth_nonce=\"{0}\", oauth_signature_method=\"{1}\", " +
                   "oauth_timestamp=\"{2}\", oauth_consumer_key=\"{3}\", " +
                   "oauth_token=\"{4}\", oauth_signature=\"{5}\", " +
                   "oauth_version=\"{6}\"";

            //var headerFormat = "OAuth oauth_nonce=\"{0}\", oauth_signature_method=\"{1}\", " +
            //       "oauth_timestamp=\"{2}\", " +
            //       "oauth_token=\"{3}\", oauth_signature=\"{4}\", " +
            //       "oauth_version=\"{5}\"";

            var authHeader = string.Format(headerFormat,
                                    Uri.EscapeDataString(oauth_nonce),
                                    Uri.EscapeDataString(oauth_signature_method),
                                    Uri.EscapeDataString(oauth_timestamp),
                                    Uri.EscapeDataString(oauth_consumer_key),
                                    Uri.EscapeDataString(oauth_token),
                                    Uri.EscapeDataString(oauth_signature),
                                    Uri.EscapeDataString(oauth_version)
                            );

            var postBody = "note=" + Uri.EscapeDataString(note);

            ServicePointManager.Expect100Continue = false;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(resource_url);
            request.Headers.Add("Authorization", authHeader);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            using (Stream stream = request.GetRequestStream())
            {
                byte[] content = Encoding.ASCII.GetBytes(postBody);
                stream.Write(content, 0, content.Length);
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Console.WriteLine(response.ToString());
            response.Close();
        }
    }
}
