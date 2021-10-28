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
using Serilog;

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

            Log.Error(ErrorMessage);
            return Task.CompletedTask;
        }

        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
            CancellationToken cancellationToken)
        {
            try
            {
                await BotOnMessageReceived(botClient,update.Message);
            }
            catch (Exception exception)
            {
                await HandleErrorAsync(botClient, exception, cancellationToken);
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static async Task BotOnMessageReceived(ITelegramBotClient botClient, Message message)
        {
            Log.Information($"Receive message type: {message.Type}");
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
            Log.Information($"The message was sent with id: {message.MessageId}");
        }

    }
}