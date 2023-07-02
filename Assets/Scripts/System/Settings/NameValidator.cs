using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace CryptoQuest.System.Settings
{
    public enum EValidation
    {
        Valid = 0,
        BadWord = 1,
        LongWord = 2,
        SpecialChars = 3,
        Null = 4
    }

    public class NameValidator : IStringValidator
    {
        private Regex _specialCharsRegex;
        private Regex _badWordsRegex;

        public NameValidator(TextAsset textAsset)
        {
            var badWords = textAsset.text.Split('\n');
            badWords = badWords.Select(x => x.ToLower()).ToArray();

            UpdateBadWordDictionary(ref badWords);
            UpdateSpecialCharsDictionary();
        }

        private void UpdateSpecialCharsDictionary(string specialChars = "@#$%^*()<>/|}{~:.;?")
        {
            _specialCharsRegex = new Regex($@"[{specialChars}]");
        }

        private void UpdateBadWordDictionary(ref string[] badWords)
        {
            var badWordsString = "";
            for (var index = 0; index < badWords.Length; index++)
            {
                var word = badWords[index];
                var or = index != badWords.Length - 1 ? "|" : "";
                badWordsString += word + or;
            }

            var regexPattern = @".*(" + badWordsString + @").*";
            _badWordsRegex = new Regex(regexPattern, RegexOptions.IgnoreCase);
        }

        public EValidation Validate(string input)
        {
            input = input.ToLower();

            var containedBadWords = _badWordsRegex.IsMatch(input);
            if (containedBadWords)
            {
                return EValidation.BadWord;
            }

            var containedSpecialChars = _specialCharsRegex.IsMatch(input);
            if (containedSpecialChars)
            {
                return EValidation.SpecialChars;
            }

            var isCharacterTooLong = input.Length > 10;
            if (isCharacterTooLong)
            {
                return EValidation.LongWord;
            }

            if (string.IsNullOrEmpty(input))
            {
                return EValidation.Null;
            }

            return EValidation.Valid;
        }
    }
}