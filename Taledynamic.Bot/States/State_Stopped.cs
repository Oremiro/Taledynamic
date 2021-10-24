using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TaleDynamicBot.States
{
    public class State_Stopped:IState
    {

        public override void Auth(ITelegramBotClient botClient, Update update)
        { 
            botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "You are already logged into the system"
            );
        }

        public override void Sending_Data(ITelegramBotClient botClient, Update update)
        {
            
             botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "Please,Wait"
            );
             this._user.Change_State(new State_Auth());
        }

        public override void Stop_sending_Data(ITelegramBotClient botClient,Update update)
        {
            botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "You already stopped"
            );
        }
    }
}