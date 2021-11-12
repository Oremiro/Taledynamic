using System;
using System.IO;
using System.Text.Json;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TaleDynamicBot.States
{
    public class StateMessageHandling:State
    {
        public override void Auth(ITelegramBotClient botClient, Update update)
        {
            botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "Вы уже авторизованы"
            );
        }

        public override void SendingData(ITelegramBotClient botClient, Message message)
        {
            botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Обработка уже запущена"
            );
        }

        public override void StopSendingData(ITelegramBotClient botClient, Update update)
        {
            botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "Остановка...."
            );
            this._user.ChangeState(new StateStopped());
        }

        public override void DefaultAction(ITelegramBotClient botClient, Message message)
        {
            string json = JsonSerializer.Serialize<Message>(message);
        }
    }
}