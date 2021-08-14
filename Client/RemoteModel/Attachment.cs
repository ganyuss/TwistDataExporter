namespace Client.RemoteModel {
    public class Attachment {
        public string attachment_id { get; set; }
        public string title { get; set; }
        public string url { get; set; }
        public string file_name { get; set; }
        public int file_size { get; set; }
        public string underlying_type { get; set; }
        public string upload_state { get; set; }
        public string url_type { get; set; }
        public string image { get; set; }
        public int? image_width { get; set; }
        public int? image_height { get; set; }
        public string duration { get; set; }
    }
}