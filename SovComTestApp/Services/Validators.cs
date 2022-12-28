using System.Text.RegularExpressions;
using SovComTestApp.Interfaces;

namespace SovComTestApp.Services
{
    public class Validators : IValidators
    {
        /// <summary>
        /// Проверка на то, являются ли все номера телефонов валидными
        /// </summary>
        public bool ValidatePhoneNumbers(IList<string> phoneNumbers)
        {
            return phoneNumbers.All(p => p.StartsWith("7") && p.Length == 11 && long.TryParse(p, out _));
        }

        /// <summary>
        /// Проверка номеров на дубликаты
        /// </summary>
        public bool ValidateDuplicates(IList<string> phoneNumbers)
        {
            return !phoneNumbers.GroupBy(p => p).Any(g => g.Count() > 1);
        }

        /// <summary>
        /// Проверка на соответствие кодировке GSM 7
        /// </summary>
        public bool ValidateGSM(string message)
        {
            var strMap = new Regex(@"^[@£$¥èéùìòÇØøÅå_ÆæßÉ!""#%&'()*+,./0123456789:;<=>? ¡ABCDEFGHIJKLMNOPQRSTUVWXYZÄÖÑÜ§¿abcdefghijklmnopqrstuvwxyzäöñüà^{}\[~\]|€-]+$");
            return strMap.IsMatch(message);
        }

        /// <summary>
        /// Проверка на кириллицу
        /// </summary>
        public bool ValidateCyrillic(string message)
        {
            string pattern = @"\p{IsCyrillic}";
            return Regex.Matches(message, pattern).Count > 0;
        }

        /// <summary>
        /// Валидация длины
        /// </summary>
        public bool ValidateLength(string message)
        {
            if (message.ToLower().Any(c => c >= 'a' && c <= 'z'))
            {
                return message.Length <= 160;
            }

            return message.Length <= 128;
        }
    }
}
