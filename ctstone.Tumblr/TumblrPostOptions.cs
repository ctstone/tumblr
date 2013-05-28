using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctstone.Tumblr
{
    public class TumblrPostOptions
    {
        public string State { get; set; }
        public string Tags { get; set; }
        public string Tweet { get; set; }
        public DateTime? Date { get; set; }
        public string Format { get; set; }
        public string Slug { get; set; }
    }
}
