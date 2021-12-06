using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using File = System.IO.File;

namespace TaleDynamicBot.States
{
    public class StateMessageHandling:State
    {
        private static readonly HttpClient client = new HttpClient();
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
                    IgnoreNullValues = true,
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    WriteIndented = true
                };
                string json = JsonSerializer.Serialize<Message>(message, options);

                Log.Information(json);

                /*var values = new Dictionary<string, string>
                {
                    { "Datajson", json } //не знаю как будет называться value
                };

                var content = new FormUrlEncodedContent(values);

                var response = await client.PostAsync("auth/User/send", content); //post запрос

                var responseString = await response.Content.ReadAsStringAsync();
                
                Log.Information($"Response : {responseString}");*/
            }
            else
            {
                var file = await botClient.GetFileAsync(message.Photo[^1].FileId);
                await botClient.DownloadFileAsync(file.FilePath, File.OpenWrite("file.jpg"));
            }
        }
    }
}