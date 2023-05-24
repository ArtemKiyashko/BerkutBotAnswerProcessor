using System;
using Newtonsoft.Json;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace BerkutBot.Models
{
    public class Announcement
    {
        [JsonRequired]
        public MessageType MessageType { get; set; }
        public string Text { get; set; }
        public Uri ContentUrl { get; set; }
        public InlineKeyboardMarkup ReplyMarkup { get; set; }
    }
}

