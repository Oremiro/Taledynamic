using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
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
            Log.Information($"Username {message.Chat.Username} has logged");
            this._user.ChangeState(new StateAuth());

            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "https://clck.ru/ZHbgx",
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

        public override async Task CallbackQueryHandler(ITelegramBotClient botclient, CallbackQuery callbackQuery)
        {
            Log.Information($"{callbackQuery.From}");

            var response = await client.GetAsync("auth/User/send"); //post запрос

            var responseString = await response.Content.ReadAsStringAsync();
                
            Log.Information($"Response : {responseString}");
        }
    }
}