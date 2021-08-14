namespace Client.RemoteModel {
    public class User {
        public int id { get; set; }
        public object scheduled_banners { get; set; }
        public string short_name { get; set; }
        public string contact_info { get; set; }
        public bool bot { get; set; }
        public string profession { get; set; }
        public string snooze_dnd_start { get; set; }
        public string client_id { get; set; }
        public string timezone { get; set; }
        public bool removed { get; set; }
        public string avatar_id { get; set; }
        public object avatar_urls { get; set; }
        public string comet_channel { get; set; }
        public string lang { get; set; }
        public object away_mode { get; set; }
        public string first_name { get; set; }
        public string comet_server { get; set; }
        public string name { get; set; }
        public object off_days { get; set; }
        public bool restricted { get; set; }
        public int default_workspace { get; set; }
        public string token { get; set; }
        public string snooze_dnd_end { get; set; }
        public bool snoozed { get; set; }
        public string email { get; set; }
        public object setup_pending { get; set; }
        public int? snooze_until { get; set; }
    }
}