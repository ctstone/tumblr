using ctstone.Json;
using ctstone.OAuth;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace ctstone.Tumblr
{
    public class TumblrClient : OAuthClientBase
    {
        public TumblrBlog Blog { get; private set; }
        public TumblrUser User { get; private set; }

        protected override Uri TemporaryCredentialsUri
        {
            get { return new Uri("http://www.tumblr.com/oauth/request_token"); }
        }
        protected override Uri AuthorizeUri
        {
            get { return new Uri("http://www.tumblr.com/oauth/authorize"); }
        }
        protected override Uri AccessTokenUri
        {
            get { return new Uri("http://www.tumblr.com/oauth/access_token"); }
        }

        public TumblrClient(string key, string secret, string callback)
            : this(key, secret, callback, null, null)
        { }

        public TumblrClient(string key, string secret, string callback, string authorizedToken, string authorizedTokenSecret)
            : base(key, secret, callback, authorizedToken, authorizedTokenSecret)
        {
            Blog = new TumblrBlog(this);
            User = new TumblrUser(this);
        }

        public dynamic GetTagged(string tag, int? before = null, int? limit = null, string filter = null)
        {
            if (tag == null)
                throw new ArgumentNullException("tag");

            FormParameters form = new FormParameters
            {
                { "id", tag },
                { "before", before },
                { "limit", before },
                { "filter", before },
            };
            if (String.IsNullOrEmpty(AuthorizedToken))
                form.Add("api_key", ConsumerKey);
            return POST(new Uri("http://api.tumblr.com/v2/user/unlike"), form);
        }


        internal dynamic GET(Uri uri)
        {
            return ReadJson(AuthorizedGET(uri));
        }

        internal dynamic POST(Uri uri, FormParameters form = null)
        {
            return ReadJson(AuthorizedPOST(uri, form));
        }

        internal static byte[] ReadBytes(Stream stream)
        {
            using (var mem = new MemoryStream())
            {
                stream.CopyTo(mem);
                return mem.ToArray();
            }
        }

        private static string Read(HttpWebResponse response)
        {
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                return sr.ReadToEnd();
            }
        }

        private static dynamic ReadJson(HttpWebResponse response)
        {
            using (response)
            {
                return JsonTokenizer.Parse(Read(response));
            }
        }
    }
}
