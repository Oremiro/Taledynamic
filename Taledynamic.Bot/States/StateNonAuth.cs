using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Web;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TaleDynamicBot.States
{
    public class StateNonAuth:State
    {
        private static readonly HttpClient client = new HttpClient();
        
        public override async Task Auth(ITelegramBotClient botClient, Message message)
        { 
            InlineKeyboardMarkup inlineKeyboard = new(
                new[]
                {
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("Проверить статус", "Auth"),
                    }
                });
            string uri = HttpUtility.UrlEncode("http://localhost:3000/account/binding?tgId=" + message.Chat.Id + "&tgName=" +
                         message.Chat.Username);
            
            var response = await client.GetAsync("https://clck.ru/--?url="+uri);

            var responseString = await response.Content.ReadAsStringAsync();

            Log.Information($"Response : {responseString}");
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: responseString,
                replyMarkup:inlineKeyboard
            );
        }
        
        public override async Task SendingData(ITelegramBotClient botClient, Message message)
        {
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Пожалуйста, авторизуйтесь с помощью команды /auth ."
            );
        }

        public override async Task StopSendingData(ITelegramBotClient botClient, Message message)
        {
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Пожалуйста, авторизуйтесь с помощью команды /auth ."
            );
        }

        public override async Task DefaultAction(ITelegramBotClient botClient, Message message)
        {
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Пожалуйста, авторизуйтесь с помощью команды /auth ."
            );
        }

        public override async Task CallbackQueryHandler(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            Log.Information($"{callbackQuery.From}");

            /*var response = await client.GetAsync("auth/User/send"); //post запрос

            var responseString = await response.Content.ReadAsStringAsync();
                
            Log.Information($"Response : {responseString}");*/

            await botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "Success");
            /*if (responseString == "Success")
            {
                Log.Information($"Username {callbackQuery.From} has logged");
                await botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "Success");
                this._user.ChangeState(new StateAuth());
            }*/
        }
    }
}