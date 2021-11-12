using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Serilog;
using System.IO;
using System.Threading;

namespace TaleDynamicBot
{
    public static class Program 
    {
        public static async Task Main()
        {
            string NameDir_of_Logs = "Logs.bot";
            string fullpath = Path.GetFullPath(NameDir_of_Logs);

            var botClient = new TelegramBotClient(Configuration.BotToken);

            var me = await botClient.GetMeAsync();

            using var cts = new CancellationTokenSource();
            
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File(fullpath + @"\Logs.txt",rollingInterval: RollingInterval.Day)
                .CreateLogger();

            botClient.StartReceiving(
                new DefaultUpdateHandler(Handlers.HandleUpdateAsync, Handlers.HandleErrorAsync),
                cts.Token);
            
            Log.Information($"Start listening for @{me.Username}");
            Console.ReadLine();
            
            cts.Cancel();
            Log.CloseAndFlush();
        }
    }
}