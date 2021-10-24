using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TaleDynamicBot
{
    public abstract class IState
    {
        protected User _user;
        public abstract void Auth(ITelegramBotClient botClient, Update update);
        public abstract void Sending_Data(ITelegramBotClient botClient, Update update);
        public abstract void Stop_sending_Data(ITelegramBotClient botClient,Update update);

        public void SetUser(User user) => this._user = user;
    }
}