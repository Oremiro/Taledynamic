using System;
using System.IO;
using System.Net;
using System.Text.Encodings.Web;
using System.Text.Json;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TaleDynamicBot.States
{
    public class StateMessageHandling:State
    {
        public override void Auth(ITelegramBotClient botClient, Update update)
        {
            botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "Вы уже авторизованы."
            );
        }

        public override void SendingData(ITelegramBotClient botClient, Message message)
        {
            botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Обработка уже запущена."
            );
        }

        public override void StopSendingData(ITelegramBotClient botClient, Update update)
        {
            botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "Останавливаю обработку..."
            );
            this._user.ChangeState(new StateStopped());
        }

        public override async void DefaultAction(ITelegramBotClient botClient, Message message)
        {
            if (message.Type == MessageType.Text)
            {
                var options = new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    WriteIndented = true
                };
                string json = JsonSerializer.Serialize<Message>(message,options);
                Log.Information(json);
            }
            else if (message.Type == MessageType.Photo)
            {
                await botClient.GetFileAsync(message.Photo[message.Photo.Length - 1].FileId);
            }
        }
        
        /*var test =  botClient.GetFileAsync(e.Message.Photo[e.Message.Photo.Count() - 1].FileId);
        var download_url = @"https://api.telegram.org/file/bot<token>/" + test.Result.FilePath;
            using (WebClient client = new WebClient())
        {
            client.DownloadFile(new Uri(download_url), @"c:\temp\NewCompanyPicure.png");
        }*/
    }
}