using System.Linq;
using UnityEngine;

namespace CryptoQuest.System.Settings
{
    public class TextValidation
    {
        private static string[] _banWords;

        public static bool ValidateWords(string word, TextAsset textAsset)
        {
            _banWords = textAsset.text.Split('\n');
            _banWords = _banWords.Select(x => x.ToLower()).ToArray();
            word = word.ToLower();

            return CheckWordsInFile(word);
        }

        private static bool CheckWordsInFile(string word)
        {
            if (!_banWords.Contains(word))
            {
                return true;
            }

            return false;
        }
    }
}