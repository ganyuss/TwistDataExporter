using System.Collections.Generic;

namespace Client.RemoteModel {
    public class Channel {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int creator { get; set; }
        public List<int> user_ids { get; set; }
        public int color { get; set; }
        public bool @public { get; set; }
        public int workspace_id { get; set; }
        public bool archived { get; set; }
        public int created_ts { get; set; }
        public bool use_default_recipients { get; set; }
        public List<int> default_groups { get; set; }
        public List<int> default_recipients { get; set; }
        public bool is_favorited { get; set; }
        public int icon { get; set; }
    }
}