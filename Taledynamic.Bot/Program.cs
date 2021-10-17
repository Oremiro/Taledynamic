using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;

namespace TaleDynamicBot
{
    public static class Program 
    {
        public static async Task Main()
        {
            var botClient = new TelegramBotClient(Configuration.BotToken);

            var me = await botClient.GetMeAsync();

            using var cts = new CancellationTokenSource();
            
            botClient.StartReceiving(
                new DefaultUpdateHandler(Handlers.HandleUpdateAsync, Handlers.HandleErrorAsync),
                cts.Token);


            
            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();
            
            cts.Cancel();
        }
    }
}