namespace Client.RemoteModel {
    public class Workspace {
        public int id { get; set; }
        public string name { get; set; }
        public int default_channel { get; set; }
        public int default_conversation { get; set; }
        public int creator { get; set; }
        public int created_ts { get; set; }
        public string avatar_id { get; set; }
        public object avatar_urls { get; set; }
        public string plan { get; set; }
    }
}