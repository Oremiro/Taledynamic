﻿using Telegram.Bot;
using Telegram.Bot.Types;

namespace TaleDynamicBot
{
    public abstract class State
    {
        protected User _user;
        public abstract void Auth(ITelegramBotClient botClient, Update update);
        public abstract void SendingData(ITelegramBotClient botClient, Message message);
        public abstract void StopSendingData(ITelegramBotClient botClient,Update update);
        public abstract void DefaultAction(ITelegramBotClient botClient, Message message);

        public void SetUser(User user) => this._user = user;
    }
}