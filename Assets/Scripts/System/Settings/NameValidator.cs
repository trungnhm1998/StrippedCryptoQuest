using System;
using System.Collections.Generic;
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
        private static readonly char[] DELIMS = { '\r', '\n' };

        public NameValidator(TextAsset badWordsAsset, TextAsset specialCharsAsset)
        {
            var badWords = badWordsAsset.text.Split(DELIMS, StringSplitOptions.RemoveEmptyEntries);
            var specialChars = specialCharsAsset.text;

            UpdateBadWordDictionary(ref badWords);
            UpdateSpecialCharsDictionary(ref specialChars);
        }

        private void UpdateSpecialCharsDictionary(ref string specialChars)
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

            _badWordsRegex = new Regex($@"\b(" + badWordsString + @")\b", RegexOptions.IgnoreCase);
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

            return EValidation.Valid;
        }
    }
}