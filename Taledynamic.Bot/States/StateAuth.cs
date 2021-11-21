using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TaleDynamicBot.States
{
    public class StateAuth:State
    {
        public override void Auth(ITelegramBotClient botClient, Update update)
        {
            botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "Вы уже авторизованы. Чтобы начать обработку сообщений, введите команду /sending ."
            );
        }

        public override void SendingData(ITelegramBotClient botClient, Message message)
        {
            botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Начинаю обработку..."
            );
            this._user.ChangeState(new StateMessageHandling());
        }

        public override void StopSendingData(ITelegramBotClient botClient,Update update)
        {
            botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "Вы ещё не начали обработку сообщений."
            );
        }

        public override void DefaultAction(ITelegramBotClient botClient, Message message)
        {
            botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Чтобы начать обработку сообщений, введите команду /sending ."
            );
        }
    }
}