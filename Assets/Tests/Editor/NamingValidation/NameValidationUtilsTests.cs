using CryptoQuest.System.Settings;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace Tests.EditMode.CryptoQuest.NamingValidation
{
    [TestFixture]
    public class NameValidationUtilsTests
    {
        private IStringValidator nameValidator;
        private TextAsset _textAsset;
        private static string ASSET_PATH = "Assets/Settings/Gameplay/words.txt";

        [SetUp]
        public void Setup()
        {
            _textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(ASSET_PATH);

            Assert.NotNull(_textAsset);

            nameValidator = new NameValidator(_textAsset);
        }

        [TestCase("badWord")]
        [TestCase("anotherBadWord")]
        [TestCase("VeryBadWord")]
        [TestCase("FUCK")]
        [TestCase("nigger")]
        [TestCase("nigga")]
        [TestCase("HELL")]
        public void Validate_BadWord_ShouldReturnFalse(string input)
        {
            EValidation result = nameValidator.Validate(input);

            Assert.AreEqual(EValidation.BadWord, result);
        }

        [TestCase("Defa;ult Player?")]
        [TestCase("~`NormalName")]
        [TestCase(".dot..")]
        [TestCase("???nani")]
        [TestCase("%^@#!$@!(*#@^&$%!&@")]
        public void Validate_SpecialCharacter_ShouldReturnFalse(string input)
        {
            EValidation result = nameValidator.Validate(input);

            Assert.AreEqual(EValidation.SpecialChars, result);
        }

        [TestCase("a")]
        [TestCase("MyNameJeff")]
        [TestCase("Linda")]
        [TestCase("Simon")]
        [TestCase("CrypQuest")]
        [TestCase("Steve")]
        [TestCase("Queen")]
        [TestCase("Assert")]
        [TestCase("Yui")]
        public void Validate_NormalNameBetweenOneToTen_ShouldReturnTrue(string input)
        {
            EValidation result = nameValidator.Validate(input);

            Assert.AreEqual(EValidation.Valid, result);
        }

        [TestCase("ThisIsLongerThanTen")]
        [TestCase("ThisIsLongerThanTenToo")]
        [TestCase("ThisIsLongerThanTenTooToo")]
        [TestCase("ThisIsLongerThanTenTooTooToo")]
        [TestCase("LongestNameEverInThisWorld")]
        [TestCase("LongestNameEverInThisWorldToo")]
        public void Validate_InvalidNameLongerThanTen_ShouldReturnFalse(string input)
        {
            EValidation result = nameValidator.Validate(input);

            Assert.AreEqual(EValidation.LongWord, result);
        }
    }
}