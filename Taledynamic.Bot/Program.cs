using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Serilog;
using System.IO;
using System.Threading;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TaleDynamicBot
{
    public static class Program 
    {
        public static async Task Main()
        { 
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string fullPath = Directory.GetParent(baseDirectory)?.Parent?.Parent?.Parent?.FullName;

            var botClient = new TelegramBotClient(Configuration.BotToken);

            var me = await botClient.GetMeAsync();

            using var cts = new CancellationTokenSource();
            
            
            
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File(fullPath + @"/Logs/Logs.txt",rollingInterval: RollingInterval.Day)
                .CreateLogger();
            
            ReceiverOptions receiverOptions = new()
            {
                AllowedUpdates = new[]{ UpdateType.Message, UpdateType.CallbackQuery }
            };

            botClient.StartReceiving(Handlers.HandleUpdateAsync,
                Handlers.HandleErrorAsync,
                receiverOptions,
                cts.Token);
            
            Log.Information($"Start listening for @{me.Username}");
            Console.ReadLine();
            
            cts.Cancel();
            Log.CloseAndFlush();
        }
    }
}