using System;

namespace TaleDynamicBot
{
    public static class Configuration
    {
        public readonly static string BotToken = Environment.GetEnvironmentVariable("bot.token");
    }
}