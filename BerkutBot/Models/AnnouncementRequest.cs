using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BerkutBot.Models
{
    public class AnnouncementRequest
    {
        [JsonRequired]
        public Announcement Announcement { get; set; }

        [JsonRequired]
        public bool SendToAll { get; set; }

        public IEnumerable<long> Chats { get; set; }

        [JsonRequired]
        public DateTime StartTime { get; set; }
    }
}

