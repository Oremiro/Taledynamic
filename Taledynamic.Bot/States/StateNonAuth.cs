using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TaleDynamicBot.States
{
    public class StateNonAuth:State
    {
        public override void Auth(ITelegramBotClient botClient, Update update)
        {
            //авторизация в проекте.

            Log.Information($"Username {update.Message.Chat.Username} has logged");
            botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "....."
            );
            //if auth successed
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

        public override void DefaultAction(ITelegramBotClient botClient, Message message)
        {
            botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Пожалуйста, авторизуйтесь с помощью команды /auth ."
            );
        }
    }
}