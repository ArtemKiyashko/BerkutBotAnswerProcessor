using System;
using Newtonsoft.Json;

namespace BerkutBot.Helpers
{
    public static class ObjectExtensions
    {
        public static string ToJson(this object update) => JsonConvert.SerializeObject(update);
    }
}

