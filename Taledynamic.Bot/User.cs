using Telegram.Bot;
using Telegram.Bot.Types;

namespace TaleDynamicBot
{
    public class User
    {
        private State _state = null;

        public User(State state) => this.Change_State(state);

        public void Change_State(State state)
        {
            this._state = state;
            this._state.SetUser(this);
        }

        public void Auth(ITelegramBotClient botClient, Update update) => this._state.Auth(botClient,update);

        public void SendingData(ITelegramBotClient botClient,Update update) => this._state.SendingData(botClient,update);
        
        public void StopSendingData(ITelegramBotClient botClient,Update update) => this._state.StopSendingData(botClient,update);
        
    }
}