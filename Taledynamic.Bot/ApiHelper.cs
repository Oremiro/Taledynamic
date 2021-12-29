using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Taledynamic.DAL.Models.Responses.UserResponses;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using File = Telegram.Bot.Types.File;

namespace TaleDynamicBot
{
    public static class WebHelper
    {
        public static (bool isSuccessStatusCode, string result) PostJson(string url, string authorization,
            string accept = "application/json", string data = "")
        {
            using (var client = new HttpClient())
            {
                if (authorization != "")
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);
                }

                //client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(accept));

                var httpResponseMessage = client.PostAsync(url,
                        !string.IsNullOrEmpty(data) ? new StringContent(data, Encoding.UTF8, "application/json") : null)
                    .Result;
                var result = httpResponseMessage.Content.ReadAsStringAsync().Result;
                return new(httpResponseMessage.IsSuccessStatusCode, result);
            }
        }
    }

    public class ApiHelper : IDisposable
    {
        private string _api { get; set; }
        private ITelegramBotClient _client { get; set; }
        private string _jwtToken { get; set; }

        public ApiHelper(ITelegramBotClient botClient, string api, string telegramUserId, string name)
        {
            _api = api;
            _client = botClient;
            Auth(telegramUserId, name);
        }

        private void Auth(string telegramUserId, string name)
        {
            var data = JsonConvert.SerializeObject(new
            {
                telegramUserId = telegramUserId
            });
            var result = WebHelper.PostJson(_api + "/integration/Telegram/tg-authorize", "", data: data);
            if (!result.isSuccessStatusCode)
            {
                var task = _client.SendTextMessageAsync(
                    chatId: telegramUserId,
                    text:
                    $"Вы не авторизированы в системе. пожалуйста перейдите по ссылке, чтобы зарегистрироваться в системе <a href=\"http://127.0.0.1:3000/integration/telegram?tgId={telegramUserId}&tgName={name}\">Ссылка</a>\n",
                    parseMode: ParseMode.Html
                );
                var res = task.Result;
            }
            else
            {
                var deserializeObject = JsonConvert.DeserializeObject<AuthenticateResponse>(result.result);
                _jwtToken = deserializeObject.JwtToken;
            }
        }

        public async Task<(bool isSuccessStatusCode, string result)> SaveMessage(ITelegramBotClient botClient,
            Message message)
        {
            File picture = null;
            string base64Image = "";
            if (message?.Photo != null)
            {
                var pictureId = message?.Photo.FirstOrDefault()?.FileId;
                picture = await botClient.GetFileAsync(pictureId);
                using (var saveImageStream = new FileStream($"{message.Chat.Username}_file.jpg", FileMode.Create))
                {
                    await botClient.DownloadFileAsync(picture?.FilePath,saveImageStream);
                }
                base64Image = Convert.ToBase64String(await System.IO.File.ReadAllBytesAsync($"{message.Chat.Username}_file.jpg"));

            }

            var text = message?.Text;
            var date = message?.Date;
             var telegramUpdateMessage = new
            {
                JsonContent = new
                {
                    Picture = base64Image,
                    DateTime = date.ToString(),
                    Text = text
                }
            };
            var jsonString = JsonConvert.SerializeObject(telegramUpdateMessage);
            var result = WebHelper.PostJson(_api + "/integration/Telegram/sync-table", _jwtToken, data: jsonString);
            return result;
        }

        public void Dispose()
        {
        }
    }

    public class TelegramUpdateMessage
    {
        public string Picture { get; set; }
        public string DateTime { get; set; }

        public string Text { get; set; }
    }
}