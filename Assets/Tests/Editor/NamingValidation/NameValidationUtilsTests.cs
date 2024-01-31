using System.Collections.Generic;
using System.Linq;
using CryptoQuest.System.Settings;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace CryptoQuest.Tests.Editor.NamingValidation
{
    [TestFixture]
    public class NameValidationUtilsTests
    {
        private IStringValidator nameValidator;

        private TextAsset _textAsset;
        private TextAsset _specialCharAsset;

        private static string BAD_WORD_ASSET_PATH = "Assets/Settings/Gameplay/BadWords.txt";
        private static string SPECIAL_CHAR_ASSET_PATH = "Assets/Settings/Gameplay/SpecialCharacters.txt";

        private static string[] badWords =
        {
            "nigga", "nigger", "HELL", "FUCK", "bitch", "hell", "asshole", "dickhead", "cunt", "idiot", "moron", "shit",
            "jerk", "bastard", "fuckwit", "douchebag", "wanker", "twat", "prick", "cockwomble", "motherfucker",
            "dumbass", "scumbag"
        };

        private static string[] specialCharacters =
            { ".dot..", "???nani", ".!@#$%", "?!@#$%^", "&*()_+-", "<>={}|/", "`~;:", "£§€", ".,", "'" };

        private static string[] normalNames =
            { "a", "MyNameJeff", "Linda", "Simon", "CrypQuest", "Steve", "Queen", "Assert", "Yui" };

        private static string[] invalidNamesLongerThanTen =
        {
            "ThisIsLongerThanTen", "ThisIsLongerThanTenToo", "ThisIsLongerThanTenTooToo",
            "ThisIsLongerThanTenTooTooToo", "LongestNameEverInThisWorld", "LongestNameEverInThisWorldToo"
        };

        private static string[] nullNames = { "", " ", "     " };
        private static string[] numberNames = { "1", "123", "123456789", "Saymyname9", "N0ic3" };

        [SetUp]
        public void Setup()
        {
            _textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(BAD_WORD_ASSET_PATH);
            _specialCharAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(SPECIAL_CHAR_ASSET_PATH);

            Assert.NotNull(_textAsset);
            Assert.NotNull(_specialCharAsset);

            nameValidator = new NameValidator(_textAsset, _specialCharAsset);
        }

        [TestCaseSource(nameof(BadWordTestCases))]
        public void Validate_BadWord_ShouldReturnEValidationBadWord(string input, EValidation expected)
        {
            EValidation result = nameValidator.Validate(input);

            Assert.AreEqual(expected, result, $"Input: {input} - Output: {result}");
        }

        [TestCaseSource(nameof(SpecialCharacterTestCases))]
        public void Validate_SpecialCharacter_ShouldReturnEValidationSpecialChars(string input, EValidation expected)
        {
            EValidation result = nameValidator.Validate(input);

            Assert.AreEqual(expected, result, $"Input: {input} - Output: {result}");
        }

        [TestCaseSource(nameof(NormalNameTestCases))]
        public void Validate_NormalNameBetweenOneToTen_ShouldReturnEValidationValid(string input, EValidation expected)
        {
            EValidation result = nameValidator.Validate(input);

            Assert.AreEqual(expected, result, $"Input: {input} - Output: {result}");
        }

        [TestCaseSource(nameof(InvalidNameLongerThanTenTestCases))]
        public void Validate_InvalidNameLongerThanTen_ShouldReturnEValidationLongWord(string input,
            EValidation expected)
        {
            EValidation result = nameValidator.Validate(input);

            Assert.AreEqual(expected, result, $"Input: {input} - Output: {result}");
        }

        [TestCaseSource(nameof(NullNameTestCases))]
        public void Validate_NullName_ShouldReturnEValidationNull(string input, EValidation expected)
        {
            EValidation result = nameValidator.Validate(input);

            Assert.AreEqual(expected, result, $"Input: {input} - Output: {result}");
        }

        [TestCaseSource(nameof(NumberNameTestCases))]
        public void Validate_NumberName_ShouldReturnEValidationValid(string input, EValidation expected)
        {
            EValidation result = nameValidator.Validate(input);

            Assert.AreEqual(expected, result, $"Input: {input} - Output: {result}");
        }

        private static IEnumerable<TestCaseData> BadWordTestCases => GetTestCases(badWords, EValidation.BadWord);

        private static IEnumerable<TestCaseData> SpecialCharacterTestCases =>
            GetTestCases(specialCharacters, EValidation.SpecialChars);

        private static IEnumerable<TestCaseData> NormalNameTestCases => GetTestCases(normalNames, EValidation.Valid);

        private static IEnumerable<TestCaseData> InvalidNameLongerThanTenTestCases =>
            GetTestCases(invalidNamesLongerThanTen, EValidation.LongWord);

        private static IEnumerable<TestCaseData> NullNameTestCases => GetTestCases(nullNames, EValidation.Null);
        private static IEnumerable<TestCaseData> NumberNameTestCases => GetTestCases(numberNames, EValidation.Valid);

        private static IEnumerable<TestCaseData> GetTestCases(string[] inputs, EValidation expected) =>
            inputs.Select(input => new TestCaseData(input, expected));
    }
}