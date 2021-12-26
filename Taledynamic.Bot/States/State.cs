using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TaleDynamicBot
{
    public abstract class State
    {
        protected User _user;
        public abstract Task Auth(ITelegramBotClient botClient, Message message);
        public abstract Task SendingData(ITelegramBotClient botClient, Message message);
        public abstract Task StopSendingData(ITelegramBotClient botClient,Message message);
        public abstract Task DefaultAction(ITelegramBotClient botClient, Message message);
        public abstract Task CallbackQueryHandler(ITelegramBotClient botClient, CallbackQuery callbackQuery);

        public void SetUser(User user) => this._user = user;
    }
}