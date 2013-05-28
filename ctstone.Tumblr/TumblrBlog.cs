using ctstone.OAuth;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctstone.Tumblr
{
    public class TumblrBlog
    {
        private TumblrClient _tumblr;

        internal TumblrBlog(TumblrClient tumblr)
        {
            _tumblr = tumblr;
        }

        public dynamic GetInfo(string baseHostname)
        {
            Uri uri = new Uri(String.Format("http://api.tumblr.com/v2/blog/{0}/info?api_key={1}", baseHostname, _tumblr.ConsumerKey));
            return _tumblr.GET(uri);
        }
        public dynamic GetAvatar(string baseHostname, int? size = null)
        {
            string url = String.Format("http://api.tumblr.com/v2/blog/{0}/avatar", baseHostname);
            if (size.HasValue)
                url += '/' + size.Value;
            Uri uri = new Uri(url);
            return _tumblr.GET(uri);
        }
        public dynamic GetLikes(string baseHostname)
        {
            Uri uri = new Uri(String.Format("http://api.tumblr.com/v2/blog/{0}/likes?api_key={1}", baseHostname, _tumblr.ConsumerKey));
            return _tumblr.GET(uri);
        }
        public dynamic GetFollowers(string baseHostname)
        {
            Uri uri = new Uri(String.Format("http://api.tumblr.com/v2/blog/{0}/followers", baseHostname));
            return _tumblr.GET(uri);
        }
        public dynamic GetPosts(string baseHostname, string type = null, long? id = null, string tag = null, int? limit = null, int? offset = null, bool? reblogInfo = null, bool? notesInfo = null, string filter = null)
        {
            FormParameters query = new FormParameters
            {
                { "api_key", _tumblr.ConsumerKey },
                { "id", id },
                { "tag", tag },
                { "limit", limit },
                { "offset", offset },
                { "reblog_info", reblogInfo },
                { "notes_info", notesInfo },
                { "filter", filter },
            };
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("http://api.tumblr.com/v2/blog/{0}/posts", baseHostname);
            if (type != null)
                sb.AppendFormat("/{0}", type);
            sb.Append(query);
            return _tumblr.GET(new Uri(sb.ToString()));
        }
        public dynamic GetQueue(string baseHostname, string offset = null, int? limit = null, string filter = null)
        {
            FormParameters query = new FormParameters 
            {
                { "offset", offset },
                { "limit", limit },
                { "filter", filter },
            };
            string url = String.Format("http://api.tumblr.com/v2/blog/{0}/posts/queue{1}", baseHostname, query);
            return _tumblr.GET(new Uri(url));
        }
        public dynamic GetDrafts(string baseHostname, string filter = null)
        {
            FormParameters query = new FormParameters 
            {
                { "filter", filter },
            };
            string url = String.Format("http://api.tumblr.com/v2/blog/{0}/posts/draft{1}", baseHostname, query);
            return _tumblr.GET(new Uri(url));
        }
        public dynamic GetSubmissions(string baseHostname, string offset = null, string filter = null)
        {
            FormParameters query = new FormParameters 
            {
                { "offset", offset },
                { "filter", filter },
            };
            string url = String.Format("http://api.tumblr.com/v2/blog/{0}/posts/submission{1}", baseHostname, query);
            return _tumblr.GET(new Uri(url));
        }

        public dynamic PostText(string baseHostname, string body, string title = null, TumblrPostOptions options = null)
        {
            if (body == null)
                throw new ArgumentNullException("body");
            if (options == null)
                options = new TumblrPostOptions();

            FormParameters form = GetPostParameters(options);
            form.Add("type", "text");
            form.Add("title", title);
            form.Add("body", body);
            string url = String.Format("http://api.tumblr.com/v2/blog/{0}/post", baseHostname);
            return _tumblr.POST(new Uri(url), form);
        }
        public dynamic PostPhoto(string baseHostname, string source, string caption = null, string link = null, TumblrPostOptions options = null)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (options == null)
                options = new TumblrPostOptions();

            FormParameters form = GetPostParameters(options);
            form.Add("type", "photo");
            form.Add("source", source);
            form.Add("caption", caption);
            form.Add("link", link);

            string url = String.Format("http://api.tumblr.com/v2/blog/{0}/post", baseHostname);
            return _tumblr.POST(new Uri(url), form);
        }
        public dynamic PostPhoto(string baseHostname, Stream data, string caption = null, string link = null, TumblrPostOptions options = null)
        {
            return PostPhoto(baseHostname, new[] { data }, caption, link, options);
        }
        public dynamic PostPhoto(string baseHostname, Stream[] data, string caption = null, string link = null, TumblrPostOptions options = null)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (options == null)
                options = new TumblrPostOptions();

            FormParameters form = GetPostParameters(options);
            for (int i = 0; i < data.Length; i++)
                form.Add(String.Format("data[{0}]", i), TumblrClient.ReadBytes(data[i]));
            form.Add("type", "photo");
            form.Add("caption", caption);
            form.Add("link", link);
            string url = String.Format("http://api.tumblr.com/v2/blog/{0}/post", baseHostname);
            return _tumblr.POST(new Uri(url), form);
        }
        public dynamic PostQuote(string baseHostname, string quote, string source = null, TumblrPostOptions options = null)
        {
            if (quote == null)
                throw new ArgumentNullException("quote");
            if (options == null)
                options = new TumblrPostOptions();

            FormParameters form = GetPostParameters(options);
            form.Add("type", "quote");
            form.Add("quote", quote);
            form.Add("source", source);
            string url = String.Format("http://api.tumblr.com/v2/blog/{0}/post", baseHostname);
            return _tumblr.POST(new Uri(url), form);
        }
        public dynamic PostLink(string baseHostname, string url, string title = null, string description = null, TumblrPostOptions options = null)
        {
            if (url == null)
                throw new ArgumentNullException("url");
            if (options == null)
                options = new TumblrPostOptions();

            FormParameters form = GetPostParameters(options);
            form.Add("type", "link");
            form.Add("url", url);
            form.Add("title", title);
            form.Add("description", description);

            string u = String.Format("http://api.tumblr.com/v2/blog/{0}/post", baseHostname);
            return _tumblr.POST(new Uri(u), form);
        }
        public dynamic PostChat(string baseHostname, string conversation, string title = null, TumblrPostOptions options = null)
        {
            if (conversation == null)
                throw new ArgumentNullException("conversation");
            if (options == null)
                options = new TumblrPostOptions();

            FormParameters form = GetPostParameters(options);
            form.Add("type", "chat");
            form.Add("conversation", conversation);
            form.Add("title", title);
            string url = String.Format("http://api.tumblr.com/v2/blog/{0}/post", baseHostname);
            return _tumblr.POST(new Uri(url), form);
        }
        public dynamic PostAudio(string baseHostname, string externalUrl, string caption = null, TumblrPostOptions options = null)
        {
            if (externalUrl == null)
                throw new ArgumentNullException("externalUrl");
            if (options == null)
                options = new TumblrPostOptions();

            FormParameters form = GetPostParameters(options);
            form.Add("type", "audio");
            form.Add("external_url", externalUrl);
            form.Add("caption", caption);
            string url = String.Format("http://api.tumblr.com/v2/blog/{0}/post", baseHostname);
            return _tumblr.POST(new Uri(url), form);
        }
        public dynamic PostAudio(string baseHostname, Stream data, string caption = null, TumblrPostOptions options = null)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (options == null)
                options = new TumblrPostOptions();

            FormParameters form = GetPostParameters(options);
            form.Add("type", "audio");
            form.Add("caption", caption);
            form.Add("data", TumblrClient.ReadBytes(data));
            string url = String.Format("http://api.tumblr.com/v2/blog/{0}/post", baseHostname);
            return _tumblr.POST(new Uri(url), form);
        }
        public dynamic PostVideo(string baseHostname, string embed, string caption = null, TumblrPostOptions options = null)
        {
            if (embed == null)
                throw new ArgumentNullException("embed");
            if (options == null)
                options = new TumblrPostOptions();

            FormParameters form = GetPostParameters(options);
            form.Add("type", "video");
            form.Add("caption", caption);
            form.Add("embed", embed);
            string url = String.Format("http://api.tumblr.com/v2/blog/{0}/post", baseHostname);
            return _tumblr.POST(new Uri(url), form);
        }
        public dynamic PostVideo(string baseHostname, Stream data, string caption = null, TumblrPostOptions options = null)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (options == null)
                options = new TumblrPostOptions();

            FormParameters form = GetPostParameters(options);
            form.Add("type", "video");
            form.Add("caption", caption);
            form.Add("data", TumblrClient.ReadBytes(data));
            string url = String.Format("http://api.tumblr.com/v2/blog/{0}/post", baseHostname);
            return _tumblr.POST(new Uri(url), form);
        }

        public dynamic EditText(string baseHostname, long id, string body = null, string title = null, TumblrPostOptions options = null)
        {
            if (options == null)
                options = new TumblrPostOptions();

            FormParameters form = GetPostParameters(options);
            form.Add("title", title);
            form.Add("body", body);
            return EditPost(baseHostname, id, form);
        }
        public dynamic EditPhoto(string baseHostname, long id, string source = null, string caption = null, string link = null, TumblrPostOptions options = null)
        {
            if (options == null)
                options = new TumblrPostOptions();

            FormParameters form = GetPostParameters(options);
            form.Add("source", source);
            form.Add("caption", caption);
            form.Add("link", link);
            return EditPost(baseHostname, id, form);
        }
        public dynamic EditPhoto(string baseHostname, long id, Stream data = null, string caption = null, string link = null, TumblrPostOptions options = null)
        {
            Stream[] ar = data == null ? null : new[] { data };
            return EditPhoto(baseHostname, id, ar, caption, link, options);
        }
        public dynamic EditPhoto(string baseHostname, long id, Stream[] data = null, string caption = null, string link = null, TumblrPostOptions options = null)
        {
            if (options == null)
                options = new TumblrPostOptions();
            FormParameters form = GetPostParameters(options);
            if (data != null)
            {
                for (int i = 0; i < data.Length; i++)
                    form.Add(String.Format("data[{0}]", i), TumblrClient.ReadBytes(data[i]));
            }
            form.Add("caption", caption);
            form.Add("link", link);
            return EditPost(baseHostname, id, form);
        }
        public dynamic EditQuote(string baseHostname, long id, string quote = null, string source = null, TumblrPostOptions options = null)
        {
            if (options == null)
                options = new TumblrPostOptions();

            FormParameters form = GetPostParameters(options);
            form.Add("quote", quote);
            form.Add("source", source);
            return EditPost(baseHostname, id, form);
        }
        public dynamic EditLink(string baseHostname, long id, string url = null, string title = null, string description = null, TumblrPostOptions options = null)
        {
            if (options == null)
                options = new TumblrPostOptions();

            FormParameters form = GetPostParameters(options);
            form.Add("url", url);
            form.Add("title", title);
            form.Add("description", description);
            return EditPost(baseHostname, id, form);
        }
        public dynamic EditChat(string baseHostname, long id, string conversation = null, string title = null, TumblrPostOptions options = null)
        {
            if (options == null)
                options = new TumblrPostOptions();

            FormParameters form = GetPostParameters(options);
            form.Add("conversation", conversation);
            form.Add("title", title);
            return EditPost(baseHostname, id, form);
        }
        public dynamic EditAudio(string baseHostname, long id, string externalUrl = null, string caption = null, TumblrPostOptions options = null)
        {
            if (options == null)
                options = new TumblrPostOptions();

            FormParameters form = GetPostParameters(options);
            form.Add("external_url", externalUrl);
            form.Add("caption", caption);
            return EditPost(baseHostname, id, form);
        }
        public dynamic EditAudio(string baseHostname, long id, Stream data = null, string caption = null, TumblrPostOptions options = null)
        {
            if (options == null)
                options = new TumblrPostOptions();

            FormParameters form = GetPostParameters(options);
            form.Add("caption", caption);
            if (data != null)
                form.Add("data", TumblrClient.ReadBytes(data));
            return EditPost(baseHostname, id, form);
        }
        public dynamic EditVideo(string baseHostname, long id, string embed = null, string caption = null, TumblrPostOptions options = null)
        {
            if (options == null)
                options = new TumblrPostOptions();

            FormParameters form = GetPostParameters(options);
            form.Add("caption", caption);
            form.Add("embed", embed);
            return EditPost(baseHostname, id, form);
        }
        public dynamic EditVideo(string baseHostname, long id, Stream data = null, string caption = null, TumblrPostOptions options = null)
        {
            if (options == null)
                options = new TumblrPostOptions();

            FormParameters form = GetPostParameters(options);
            form.Add("caption", caption);
            if (data != null)
                form.Add("data", TumblrClient.ReadBytes(data));
            return EditPost(baseHostname, id, form);
        }

        public dynamic ReblogPost(string baseHostname, long id, string reblogKey, string type, string comment = null, TumblrPostOptions options = null)
        {
            if (options == null)
                options = new TumblrPostOptions();
            FormParameters form = GetPostParameters(options);
            form.Add("type", type);
            form.Add("id", id);
            form.Add("reblog_key", reblogKey);
            form.Add("comment", comment);
            string url = String.Format("http://api.tumblr.com/v2/blog/{0}/post/reblog", baseHostname);
            return _tumblr.POST(new Uri(url), form);
        }
        public dynamic DeletePost(string baseHostname, long id)
        {
            FormParameters form = new FormParameters();
            form.Add("id", id);
            string url = String.Format("http://api.tumblr.com/v2/blog/{0}/post/delete", baseHostname);
            return _tumblr.POST(new Uri(url), form);
        }

        private dynamic EditPost(string baseHostname, long id, FormParameters form)
        {
            form.Set("id", id);
            string url = String.Format("http://api.tumblr.com/v2/blog/{0}/post/edit", baseHostname);
            return _tumblr.POST(new Uri(url), form);
        }

        private FormParameters GetPostParameters(TumblrPostOptions options)
        {
            return new FormParameters
            {
                { "state", options.State },
                { "tags", options.Tags },
                { "tweet", options.Tweet },
                { "date", options.Date },
                { "format", options.Format },
                { "slug", options.Slug },
            };
        }
    }
}
