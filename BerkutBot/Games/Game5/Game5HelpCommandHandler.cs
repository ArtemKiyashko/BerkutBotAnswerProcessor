using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using BerkutBot.Infrastructure;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BerkutBot.Games.Game5
{
    public class Game5HelpCommandHandler : IGameAnswer
    {
        private const string COMMAND = "/help";
        private const string ANSWER_IPHONE_BACKGROUND_TEXT = "iPhone: телефоны версии XR и выше поддерживают фоновое чтение NFC меток и не требуют дополнительной конфигурации или приложений.\n" +
            "Если вы обладатель этого чуда - поздравляем! Просто поднесите телефон задней стороной (в районе камеры) к метке и все должно сработать как надо";
        private const string ANSWER_IPHONE_OLD_TEXT = "iPhone: Если у вас телефон версии 7 - X, то это тоже сгодится. NFC ридер в нем есть, но его надо активировать каждый раз вручную.\n" +
            "NFC Reader находится в [Пункте управления](https://support.apple.com/ru-ru/guide/iphone/iph59095ec58/ios). Если его у вас там нет, вы можете добавить его через <Настройки>-<Пункт Управления>\n" +
            "Теперь, чтобы считать нашу метку, активируйте NFC Reader в пункте управления и поднесите телефон к метке.";
        private const string ANSWER_ANDROID_TEXT = "Android: все зависит от производителя телефона. Большинство современных телефонов поддерживают фоновую работу с NFC и тут все так же как на iPhone.\n" +
            "Если метки на вашем телефоне не читают - попробуйте поискать в настройках телефона что-то связанное с NFC или бесконтактными платежами и включить это, если было выключено.\n" +
            "Обратитесь к организаторам, если у вас что-то не получается - они помогут разобраться с вашей моделью телефона!";
        private const string ANSWER_IPHONE_PIC2_URL = "https://sawevprivate.blob.core.windows.net/public/use-nfc-iphone-add-nfc-tag-reader.webp";
        private const string ANSWER_IPHONE_PIC1_URL = "https://sawevprivate.blob.core.windows.net/public/use-nfc-iphone-control-center-settings-441x480.webp";
        private const string ANSWER_IPHONE_PIC3_URL = "https://sawevprivate.blob.core.windows.net/public/use-nfc-iphone-nfc-tag-reader-412x480.webp";

        private readonly ITelegramBotClient _telegramBotClient;
        private readonly ILogger<Game5HelpCommandHandler> _logger;

        public Game5HelpCommandHandler(ITelegramBotClient telegramBotClient, ILogger<Game5HelpCommandHandler> logger)
        {
            _telegramBotClient = telegramBotClient;
            _logger = logger;
        }

        public Func<string, bool> Intent => (string text) => text.StartsWith(COMMAND, StringComparison.OrdinalIgnoreCase);

        public int Order => 1;

        public async Task<string> Reply(Message message)
        {
            await _telegramBotClient.SendTextMessageAsync(message.Chat.Id, ANSWER_IPHONE_BACKGROUND_TEXT);

            await _telegramBotClient.SendMediaGroupAsync(
                chatId: message.Chat.Id,
                media: new IAlbumInputMedia[]
                {
                    new InputMediaPhoto(
                        InputFile.FromUri(ANSWER_IPHONE_PIC1_URL)),
                    new InputMediaPhoto(
                        InputFile.FromUri(ANSWER_IPHONE_PIC2_URL)),
                    new InputMediaPhoto(
                        InputFile.FromUri(ANSWER_IPHONE_PIC3_URL)),
                }
            );

            await _telegramBotClient.SendTextMessageAsync(message.Chat.Id, ANSWER_IPHONE_OLD_TEXT, parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);

            await _telegramBotClient.SendTextMessageAsync(message.Chat.Id, ANSWER_ANDROID_TEXT);

            return "Help sent";
        }
    }
}

