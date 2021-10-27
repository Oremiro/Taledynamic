using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TaleDynamicBot
{
    public abstract class State
    {
        protected User _user;
        public abstract void Auth(ITelegramBotClient botClient, Update update);
        public abstract void SendingData(ITelegramBotClient botClient, Update update);
        public abstract void StopSendingData(ITelegramBotClient botClient,Update update);

        public void SetUser(User user) => this._user = user;
    }
}