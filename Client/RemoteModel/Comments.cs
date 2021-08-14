using System.Collections.Generic;

namespace Client.RemoteModel {
    public class Comment {
        public int id { get; set; }
        public string content { get; set; }
        public int creator { get; set; }
        public int thread_id { get; set; }
        public int channel_id { get; set; }
        public int workspace_id { get; set; }
        public int obj_index { get; set; }
        public List<Attachment> attachments { get; set; }
        public object actions { get; set; }
        public object recipients { get; set; }
        public object groups { get; set; }
        public object reactions { get; set; }
        public object direct_mentions { get; set; }
        public object direct_group_mentions { get; set; }
        public bool deleted { get; set; }
        public int? deleted_by { get; set; }
        public object system_message { get; set; }
        public int posted_ts { get; set; }
        public int? last_edited_ts { get; set; }
    }
}