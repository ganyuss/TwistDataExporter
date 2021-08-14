using System.Collections.Generic;

namespace Client.RemoteModel {
    public class Thread {
        public object actions { get; set; }
        public List<Attachment> attachments { get; set; }
        public int channel_id { get; set; }
        public int comment_count { get; set; }
        public string content { get; set; }
        public int creator { get; set; }
        public List<int> direct_group_mentions { get; set; }
        public List<int> direct_mentions { get; set; }
        public List<int> groups { get; set; }
        public int id { get; set; }
        public int? last_edited_ts { get; set; }
        public int last_obj_index { get; set; }
        public int last_updated_ts { get; set; }
        public int? muted_until_ts { get; set; }
        public List<int> participants { get; set; }
        public bool pinned { get; set; }
        public int? pinned_ts { get; set; }
        public int posted_ts { get; set; }
        public object reactions { get; set; }
        public object recipients { get; set; }
        public string snippet { get; set; }
        public int snippet_creator { get; set; }
        public bool starred { get; set; }
        public string system_message { get; set; }
        public string title { get; set; }
        public int workspace_id { get; set; }
        public bool is_archived { get; set; }
        public bool in_inbox { get; set; }
    }
}