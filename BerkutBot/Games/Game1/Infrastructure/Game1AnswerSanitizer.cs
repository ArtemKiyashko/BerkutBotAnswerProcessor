using System;
namespace BerkutBot.Games.Game1.Infrastructure
{
    public static class Game1AnswerSanitizer
    {
        public static string Sanitize(this string rawAnswer) => rawAnswer
            .ToLower()
            .Replace(',', '.')
            .Replace("/", "");
    }
}

