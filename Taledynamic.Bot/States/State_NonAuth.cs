using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TaleDynamicBot.States
{
    public class State_NonAuth:IState
    {
        public override void Auth(ITelegramBotClient botClient, Update update)
        {
            //авторизация в проекте.

            Log.Information($"Username {update.Message.Chat.Username} has logged");
            botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "You logging...."
            );
            //if auth successed
            this._user.Change_State(new State_Auth());
        }
        
        public override void Sending_Data(ITelegramBotClient botClient, Update update)
        {
             botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "You are not logged in the system"
            );
        }

        public override void Stop_sending_Data(ITelegramBotClient botClient, Update update)
        {
            botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "You are not logged in the system"
            );
        }
    }
}