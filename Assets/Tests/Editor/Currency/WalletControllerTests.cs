using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace CryptoQuest.Tests.Editor.Currency
{
    public class WalletControllerTests
    {
        private WalletSO _walletSo;
        private WalletControllerSO _walletControllerSo;
        private WalletController _walletController;
        private CurrencySO goldSo;
        private CurrencySO diamondSo;
        private CurrencySO soulSo;

        [SetUp]
        public void Setup()
        {
            _walletControllerSo = ScriptableObject.CreateInstance<WalletControllerSO>();
            goldSo = ScriptableObject.CreateInstance<CurrencySO>();
            diamondSo = ScriptableObject.CreateInstance<CurrencySO>();
            soulSo = ScriptableObject.CreateInstance<CurrencySO>();
            _walletSo = ScriptableObject.CreateInstance<WalletSO>();
            _walletSo.Gold = new CurrencyInfo(goldSo, 0);
            _walletSo.Diamond = new CurrencyInfo(diamondSo, 0);
            _walletSo.Soul = new CurrencyInfo(soulSo, 0);
            _walletSo.Editor_Enable();

            _walletSo.Gold.SetCurrencyAmount(0);
            _walletSo.Diamond.SetCurrencyAmount(0);
            _walletSo.Soul.SetCurrencyAmount(0);
            _walletController = Object.Instantiate(new GameObject()).AddComponent<WalletController>();
            _walletController.Editor_SetWallet(_walletSo);
            _walletController.Editor_SetWalletController(_walletControllerSo);
        }

        [TestCase(0, 0, 0, 4, 2, 3, 4, 2, 3)]
        [TestCase(0, 0, 0, -4, -2, -3, 0, 0, 0)]
        [TestCase(3, 4, 5, -4, -5, -6, 3, 4, 5)]
        [TestCase(-3, -1, -5, 0, 0, 0, 0, 0, 0)]
        [TestCase(3.1f, 2.1f, 5.2f, 3.1111111111111111f, 5.222222222222222222f, 1.11111111111111111111f,
            6.2111111111111111f, 7.322222222222222222f, 6.31111111111111111111f)]
        public void WalletController_UpdateCurrency_ReturnCorrectUpdatedValue(float defaultGold, float defaultDiamond,
            float defaultSoul,
            float amountToUpdateGold, float amountToUpdateDiamond,
            float amountToUpdateSoul, float expectedGold, float expectedDiamond, float expectedSoul)
        {
            _walletSo.OnValidate();
            _walletSo.Gold.SetCurrencyAmount(defaultGold);
            _walletSo.Diamond.SetCurrencyAmount(defaultDiamond);
            _walletSo.Soul.SetCurrencyAmount(defaultSoul);
            _walletController.UpdateCurrencyAmount(_walletSo.Gold.Data, amountToUpdateGold);
            _walletController.UpdateCurrencyAmount(_walletSo.Diamond.Data, amountToUpdateDiamond);
            _walletController.UpdateCurrencyAmount(_walletSo.Soul.Data, amountToUpdateSoul);
            _walletSo.OnValidate();
            Assert.AreEqual(expectedGold, _walletSo.Gold.Amount);
            Assert.AreEqual(expectedDiamond, _walletSo.Diamond.Amount);
            Assert.AreEqual(expectedSoul, _walletSo.Soul.Amount);
        }
    }
}