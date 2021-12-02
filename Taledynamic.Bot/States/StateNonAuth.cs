using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TaleDynamicBot.States
{
    public class StateNonAuth:State
    {
        private static readonly HttpClient client = new HttpClient();
        public override async void Auth(ITelegramBotClient botClient, Update update)
        {
            await botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "http://taledynamic.com/tgauth?" + update.Message.Chat.Username
            );
            this._user.ChangeState(new StateAuth());
        }
        
        public override void SendingData(ITelegramBotClient botClient, Message message)
        {
            botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Пожалуйста, авторизуйтесь с помощью команды /auth ."
            );
        }

        public override void StopSendingData(ITelegramBotClient botClient, Update update)
        {
            botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "Пожалуйста, авторизуйтесь с помощью команды /auth ."
            );
        }

        public override async void DefaultAction(ITelegramBotClient botClient, Message message)
        { 
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Пожалуйста, авторизуйтесь с помощью команды /auth ."
            );
        }
    }
}