using CryptoQuest.Input;
using CryptoQuest.ShopSystem;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CryptoQuest.Tests.Runtime.Shop
{
    [TestFixture]
    public class UIQuantityDialogTests : InputTestFixture
    {
        private UIQuantityDialog _dialog;
        private Gamepad _gamepad;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            var prefab =
                AssetDatabase.LoadAssetAtPath<UIQuantityDialog>("Assets/Prefabs/UI/Shop/UIQuantityDialog.prefab");
            _dialog = Object.Instantiate(prefab);
            AssetDatabase.LoadAssetAtPath<InputMediatorSO>(
                "Assets/ScriptableObjects/Input/InputMediatorSO.asset");
            _gamepad = InputSystem.AddDevice<Gamepad>();
        }

        [Test]
        public void Show_SetMaxQuantity()
        {
            _dialog.Show(10);
            Assert.AreEqual(10, _dialog.MaxQuantity);
        }

        [Test]
        public void Show_DefaultQuantityShouldBeOne()
        {
            _dialog.Show(10);
            Assert.AreEqual(1, _dialog.CurrentQuantity);
        }

        [Test]
        public void ChangeQuantity_WithPositiveY_ShouldChangeCurrentQuantity()
        {
            _dialog.Show(10);
            PressAndRelease(_gamepad.dpad.up);

            Assert.AreEqual(2, _dialog.CurrentQuantity);
        }

        [Test]
        public void ChangeQuantity_WithNegativeY_ShouldDecreaseQuantity()
        {
            _dialog.Show(10);
            PressAndRelease(_gamepad.dpad.up);
            PressAndRelease(_gamepad.dpad.down);

            Assert.AreEqual(1, _dialog.CurrentQuantity);
        }

        [Test]
        public void ChangeQuantity_NegativeY_ShouldNotBelowOne()
        {
            _dialog.Show(10);
            PressAndRelease(_gamepad.dpad.down);

            Assert.AreEqual(1, _dialog.CurrentQuantity);
        }

        [Test]
        public void ChangeQuantity_ShouldNotOverMax()
        {
            _dialog.Show(2);
            PressAndRelease(_gamepad.dpad.up);
            PressAndRelease(_gamepad.dpad.up);
            PressAndRelease(_gamepad.dpad.up);
            PressAndRelease(_gamepad.dpad.up);
            
            Assert.AreEqual(2, _dialog.CurrentQuantity);
        }

        [Test]
        public void OnConfirmQuantity_HideContentAndResetConfirmCallback()
        {
            var callbackInvoked = false;
            _dialog.WithConfirmCallback(() => callbackInvoked = true);
            _dialog.OnConfirmQuantity();
            Assert.True(callbackInvoked);
            callbackInvoked = false;
            Assert.False(callbackInvoked, "Callback still not clear");
        }
    }
}