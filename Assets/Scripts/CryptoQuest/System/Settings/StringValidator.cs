using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace CryptoQuest.System.Settings
{
    public class StringValidator
    {
        private Regex _specialCharsRegex;
        private Regex _badWordsRegex;
        private InvalidType _invalidType;
        private readonly string[] _banWords;

        public StringValidator(TextAsset textAsset)
        {
            _banWords = textAsset.text.Split('\n');
            _banWords = _banWords.Select(x => x.ToLower()).ToArray();

            UpdateBadWordDictionary();
            UpdateSpecialCharsDictionary();
        }

        public void UpdateSpecialCharsDictionary(string specialChars = "@#$%^*()<>/|}{~:;?")
        {
            _specialCharsRegex = new Regex($@"[{specialChars}]");
        }

        public void UpdateBadWordDictionary()
        {
            var badWordsString = "";
            for (var index = 0; index < _banWords.Length; index++)
            {
                var word = _banWords[index];
                var or = index != _banWords.Length - 1 ? "|" : "";
                badWordsString += word + or;
            }

            var regexPattern = @".*(" + badWordsString + @").*";
            _badWordsRegex = new Regex(regexPattern, RegexOptions.IgnoreCase);
        }

        public bool Validate(string input)
        {
            input = input.ToLower();

            var containedBadWords = _badWordsRegex.IsMatch(input);
            if (containedBadWords)
            {
                _invalidType = InvalidType.BAN_WORD;
                return false;
            }

            var containedSpecialChars = _specialCharsRegex.IsMatch(input);
            if (containedSpecialChars)
            {
                _invalidType = InvalidType.SPEACIAL_CHARACTER;
                return false;
            }

            var isCharacterTooLong = input.Length > 10 || input.Length <= 1;
            if (isCharacterTooLong)
            {
                _invalidType = InvalidType.LONG_CHARACTER;
                return false;
            }

            return true;
        }

        public string WarningText()
        {
            return _invalidType switch
            {
                InvalidType.BAN_WORD => _invalidType.ToString(),
                InvalidType.LONG_CHARACTER => _invalidType.ToString(),
                InvalidType.SPEACIAL_CHARACTER => _invalidType.ToString(),
                _ => ""
            };
        }
    }

    public enum InvalidType
    {
        NONE = 0,
        BAN_WORD = 1,
        LONG_CHARACTER = 2,
        SPEACIAL_CHARACTER = 3
    }
}