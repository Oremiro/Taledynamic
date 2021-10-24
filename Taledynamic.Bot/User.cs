using Telegram.Bot;
using Telegram.Bot.Types;

namespace TaleDynamicBot
{
    public class User
    {
        private IState _state = null;

        public User(IState state) => this.Change_State(state);

        public void Change_State(IState state)
        {
            this._state = state;
            this._state.SetUser(this);
        }

        public void Auth(ITelegramBotClient botClient, Update update) => this._state.Auth(botClient,update);

        public void Sending_Data(ITelegramBotClient botClient,Update update) => this._state.Sending_Data(botClient,update);
        
        public void Stop_sending_Data(ITelegramBotClient botClient,Update update) => this._state.Stop_sending_Data(botClient,update);
        
    }
}