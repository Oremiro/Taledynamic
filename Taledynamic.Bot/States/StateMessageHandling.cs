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
        public override async Task Auth(ITelegramBotClient botClient, Message message)
        {
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Вы уже авторизованы."
            );
        }

        public override async Task SendingData(ITelegramBotClient botClient, Message message)
        {
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Обработка уже запущена."
            );
        }

        public override async Task StopSendingData(ITelegramBotClient botClient, Message message)
        {
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Останавливаю обработку..."
            );
            this._user.ChangeState(new StateStopped());
        }

        public override async Task DefaultAction(ITelegramBotClient botClient, Message message)
        {
            var options = new JsonSerializerOptions
            {
                IgnoreNullValues = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            };

            if (message.Type == MessageType.Text)
            {
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
                string json = JsonSerializer.Serialize<Message>(message, options);

                Log.Information(json);
                
                var file = await botClient.GetFileAsync(message.Photo[^1].FileId);
                
                using (var saveImageStream = new FileStream($"{message.Chat.Username}_file.jpg", FileMode.Create))
                {
                    await botClient.DownloadFileAsync(file.FilePath,saveImageStream);
                }
                string base64Image = Convert.ToBase64String(await File.ReadAllBytesAsync($"{message.Chat.Username}_file.jpg"));
                
                /*var values = new Dictionary<string, string>
                {
                    { "Image", base64Image } //не знаю как будет называться value
                };

                var content = new FormUrlEncodedContent(values);

                var response = await client.PostAsync("auth/User/send", content); //post запрос

                var responseString = await response.Content.ReadAsStringAsync();
                
                Log.Information($"Response : {responseString}");*/
            }
        }

        public override async Task CallbackQueryHandler(ITelegramBotClient botclient, CallbackQuery callbackQuery)
        {
            /*var response = await client.GetAsync("auth/User/send"); //get запрос

            var responseString = await response.Content.ReadAsStringAsync();
            
            if (responseString == "Success")
            {
                await botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "Success");
            }
            else 
            {
                this._user.ChangeState(new StateNonAuth());
            }*/
            
        }
    }
}