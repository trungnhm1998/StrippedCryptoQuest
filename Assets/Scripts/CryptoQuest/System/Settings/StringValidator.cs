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
                _invalidType = InvalidType.ContainedBanWord;
                return false;
            }


            var containedSpecialChars = _specialCharsRegex.IsMatch(input);
            if (containedSpecialChars)
            {
                _invalidType = InvalidType.ContainedSpecialChars;
                return false;
            }


            return true;
        }

        public string WarningText()
        {
            switch (_invalidType)
            {
                case InvalidType.ContainedBanWord:
                    return "This word is not allowed";
                case InvalidType.ContainedTooLongCharacter:
                    return "1 to 10 characters only";
                case InvalidType.ContainedSpecialChars:
                    return "Special characters are not allowed";
                default:
                    return "";
            }
        }
    }


    public enum InvalidType
    {
        None = 0,
        ContainedSpecialChars = 1,
        ContainedBanWord = 2,
        ContainedTooLongCharacter = 3,
    }
}