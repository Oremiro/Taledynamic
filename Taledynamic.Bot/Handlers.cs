using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace TaleDynamicBot
{
    public class Handlers
    {
        public static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception,
            CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException =>
                    $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
            CancellationToken cancellationToken)
        {
            var Message = new Dictionary<string, string>()
            {
                {"text",update.Message.Text}
                
            };


            
            
            var handler = update.Type switch
            {
                UpdateType.Message            => BotOnMessageReceived(botClient, update.Message)
            };

            try
            {
                await handler;
            }
            catch (Exception exception)
            {
                await HandleErrorAsync(botClient, exception, cancellationToken);
            }
        }

        public static async Task BotOnMessageReceived(ITelegramBotClient botClient, Message message)
        {
            Console.WriteLine($"Receive message type: {message.Type}");
            if (message.Type != MessageType.Text)
                return;


            //if (user is not in data base)
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "You are not in our database, please register.",
                parseMode: ParseMode.Markdown,
                disableNotification: true,
                replyMarkup: new InlineKeyboardMarkup(InlineKeyboardButton.WithUrl(
                    "TaleDynamic",
                    "https://google.com"
                ))
            );
            Console.WriteLine($"The message was sent with id: {message.MessageId}");
        }

    }
}