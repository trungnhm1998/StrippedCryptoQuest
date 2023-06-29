using CryptoQuest.System.Settings;
using NUnit.Framework;
using UnityEngine;

namespace Tests.EditMode.CryptoQuest.NamingValidation
{
    [TestFixture]
    public class NameValidationUtilsTests
    {
        private StringValidator _stringValidator;
        private TextAsset _textAsset;

        [SetUp]
        public void Setup()
        {
            _textAsset = Resources.Load<TextAsset>("words");

            Assert.NotNull(_textAsset);

            _stringValidator = new StringValidator(_textAsset);
        }

        [TestCase("badWord")]
        [TestCase("anotherBadWord")]
        [TestCase("VeryBadWord")]
        [TestCase("Defa;ult Player?")]
        [TestCase("~`NormalName")]
        [TestCase("FUCK")]
        [TestCase("nigger")]
        [TestCase("nigga")]
        [TestCase("HELL")]
        public void Validate_BadWord_ShouldReturnFalse(string input)
        {
            var result = _stringValidator.Validate(input);

            Assert.IsFalse(result);
        }

        [TestCase("someName")]
        [TestCase("Default Player")]
        [TestCase("NORMAL NAME")]
        public void Validate_NormalInput_ShouldReturnTrue(string input)
        {
            var result = _stringValidator.Validate(input);

            Assert.IsTrue(result);
        }

        [TestCase("a")]
        [TestCase("hereIsLongName")]
        [TestCase("ThisisTheLongestNameInTheWorld")]
        public void Validate_ShortInput_ShouldReturnFalse(string input)
        {
            var result = _stringValidator.Validate(input);

            Assert.IsFalse(result);
        }
    }
}