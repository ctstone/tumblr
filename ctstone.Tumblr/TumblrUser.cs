using ctstone.OAuth;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctstone.Tumblr
{
    public class TumblrUser
    {
        private TumblrClient _tumblr;

        internal TumblrUser(TumblrClient tumblr)
        {
            _tumblr = tumblr;
        }

        public dynamic GetInfo()
        {
            return _tumblr.GET(new Uri("http://api.tumblr.com/v2/user/info"));
        }
        public dynamic GetDashboard(int? limit = null, int? offset = null, string type = null, long? sinceId = null, bool? reblogInfo = null, bool? notesInfo = null)
        {
            FormParameters query = new FormParameters
            {
                { "limit", limit },
                { "offset", offset },
                { "type", type },
                { "since_id", sinceId},
                { "reblog_info", reblogInfo},
                { "notes_info", notesInfo },
            };

            StringBuilder sb = new StringBuilder();
            sb.Append("http://api.tumblr.com/v2/user/dashboard");
            sb.Append(query);
            return _tumblr.GET(new Uri(sb.ToString()));
        }
        public dynamic GetLikes(int? limit = null, int? offset = null)
        {
            FormParameters query = new FormParameters
            {
                { "limit", limit },
                { "offset", offset },
            };

            StringBuilder sb = new StringBuilder();
            sb.Append("http://api.tumblr.com/v2/user/likes");
            sb.Append(query);
            return _tumblr.GET(new Uri(sb.ToString()));
        }
        public dynamic GetFollowing(int? limit = null, int? offset = null)
        {
            FormParameters query = new FormParameters
            {
                { "limit", limit },
                { "offset", offset },
            };

            StringBuilder sb = new StringBuilder();
            sb.Append("http://api.tumblr.com/v2/user/following");
            sb.Append(query);
            return _tumblr.GET(new Uri(sb.ToString()));
        }
        public dynamic Follow(string url)
        {
            if (url == null)
                throw new ArgumentNullException("url");

            FormParameters form = new FormParameters
            {
                { "url", url },
            };
            return _tumblr.POST(new Uri("http://api.tumblr.com/v2/user/follow"), form);
        }
        public dynamic Unfollow(string url)
        {
            if (url == null)
                throw new ArgumentNullException("url");

            FormParameters form = new FormParameters
            {
                { "url", url },
            };
            return _tumblr.POST(new Uri("http://api.tumblr.com/v2/user/unfollow"), form);
        }

        public dynamic Like(long id, string reblogKey)
        {
            if (reblogKey == null)
                throw new ArgumentNullException("reblogKey");

            FormParameters form = new FormParameters
            {
                { "id", id },
                { "reblog_key", reblogKey },
            };
            return _tumblr.POST(new Uri("http://api.tumblr.com/v2/user/like"), form);
        }
        public dynamic Unlike(long id, string reblogKey)
        {
            if (reblogKey == null)
                throw new ArgumentNullException("reblogKey");

            FormParameters form = new FormParameters
            {
                { "id", id },
                { "reblog_key", reblogKey },
            };
            return _tumblr.POST(new Uri("http://api.tumblr.com/v2/user/unlike"), form);
        }

    }
}
