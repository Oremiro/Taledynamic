using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TaleDynamicBot
{
    public class User
    {
        private State _state = null;

        public User(State state) => this.ChangeState(state);

        public void ChangeState(State state)
        {
            this._state = state;
            this._state.SetUser(this);
        }

        public async Task Auth(ITelegramBotClient botClient, Message message) => await this._state.Auth(botClient,message);

        public async Task SendingData(ITelegramBotClient botClient,Message message) => await this._state.SendingData(botClient,message);
        
        public async Task StopSendingData(ITelegramBotClient botClient,Message message) => await this._state.StopSendingData(botClient,message);

        public async Task DefaultAction(ITelegramBotClient botClient, Message message) => await this._state.DefaultAction(botClient, message);

        public async Task CallbackQueryHandler(ITelegramBotClient botClient, CallbackQuery callbackQuery) =>
            await this._state.CallbackQueryHandler(botClient, callbackQuery);

    }
}