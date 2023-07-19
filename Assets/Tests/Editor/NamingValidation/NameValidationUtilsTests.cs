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


        [SetUp]
        public void Setup()
        {
            _textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(BAD_WORD_ASSET_PATH);
            _specialCharAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(SPECIAL_CHAR_ASSET_PATH);

            Assert.NotNull(_textAsset);
            Assert.NotNull(_specialCharAsset);

            nameValidator = new NameValidator(_textAsset, _specialCharAsset);
        }

        [TestCase("nigga")]
        [TestCase("nigger")]
        [TestCase("nigga")]
        [TestCase("HELL")]
        [TestCase("FUCK")]
        [TestCase("bitch")]
        [TestCase("hell")]
        [TestCase("asshole")]
        [TestCase("dickhead")]
        [TestCase("cunt")]
        [TestCase("idiot")]
        [TestCase("moron")]
        [TestCase("shit")]
        [TestCase("jerk")]
        [TestCase("bastard")]
        [TestCase("fuckwit")]
        [TestCase("douchebag")]
        [TestCase("wanker")]
        [TestCase("twat")]
        [TestCase("prick")]
        [TestCase("cockwomble")]
        [TestCase("motherfucker")]
        [TestCase("dumbass")]
        [TestCase("scumbag")]
        public void Validate_BadWord_ShouldReturnEValidationBadWord(string input)
        {
            EValidation result = nameValidator.Validate(input);

            Assert.AreEqual(EValidation.BadWord, result);
        }


        [TestCase(".dot..")]
        [TestCase("???nani")]
        [TestCase(".!@#$%")]
        [TestCase("?!@#$%^")]
        [TestCase("&*()_+-")]
        [TestCase("<>={}|/")]
        [TestCase("`~;:")]
        [TestCase("£§€")]
        [TestCase(".,")]
        [TestCase("'")]
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

        [TestCase("")]
        public void Validate_NullName_ShouldReturnTrue(string input)
        {
            EValidation result = nameValidator.Validate(input);

            Assert.AreEqual(EValidation.Null, result);
        }
        
        [TestCase("1")]
        [TestCase("123")]
        [TestCase("123456789")]
        [TestCase("Saymyname9")]
        [TestCase("N0ic3")]
        public void Validate_NumberName_ShouldReturnTrue(string input)
        {
            EValidation result = nameValidator.Validate(input);

            Assert.AreEqual(EValidation.Valid, result);
        }
    }
}