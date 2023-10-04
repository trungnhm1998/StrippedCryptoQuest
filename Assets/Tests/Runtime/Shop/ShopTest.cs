using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEditor;
using CryptoQuest.Character;
using UnityEditor.VersionControl;
using CryptoQuest.Shop.UI.ScriptableObjects;
using CryptoQuest.Item.Equipment;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Shop.UI.Item;
using CryptoQuest.Item;
using CryptoQuest.Shop;
using CryptoQuest.System;
using CryptoQuest.Gameplay.Inventory;

namespace CryptoQuest.Tests.Runtime.Shop
{
    [TestFixture]
    [Category("Integration")]
    public class ShopTest
    {
#if UNITY_EDITOR
        private const string SHOP_SCENE = "Assets/Tests/Runtime/Shop/Shop.unity";
        private const string EQUIPMENT_DEF_DATABASE = "Assets/ScriptableObjects/Inventory/EquipmentDefDatabase.asset";
        private const string EQUIPMENT_DATABASE = "Assets/ScriptableObjects/Inventory/EquipmentDatabase.asset";
        private const string CONSUMABLE_DATABASE = "Assets/ScriptableObjects/Inventory/ConsumableDatabase.asset";
        private const string INVENTORY_PATH = "Assets/ScriptableObjects/Inventory/MainInventory.asset";

        private string shopObjectName = "ShopPanel";
        private string shopDialogName = "DialogPanel";
        private ShowShopEventChannelSO _showShopEvent;
        private InventorySO _inventorySO;
        private IShopInventoryController _shopInventoryController;
        private IEquipmentDefProvider _equipmentDefProvider;
        private IConsumableProvider _consumableProvider;

        [UnitySetUp]
        public IEnumerator OneTimeSetUp()
        {
            _inventorySO = AssetDatabase.LoadAssetAtPath<InventorySO>(INVENTORY_PATH);

            Assert.IsNotNull(_inventorySO, "_inventorySO database could not be null");
            //Load event

            var eventGuids = AssetDatabase.FindAssets("t:ShowShopEventChannelSO");

            foreach (var eventGuid in eventGuids)
            {
                _showShopEvent = AssetDatabase.LoadAssetAtPath<ShowShopEventChannelSO>(AssetDatabase.GUIDToAssetPath(eventGuid));
            }

            Assert.IsNotNull(_showShopEvent, "show shop event could not be null!");

            yield return EditorSceneManager.LoadSceneAsyncInPlayMode(SHOP_SCENE, new LoadSceneParameters(LoadSceneMode.Single));

            _shopInventoryController = ServiceProvider.GetService<IShopInventoryController>();
            _equipmentDefProvider = ServiceProvider.GetService<IEquipmentDefProvider>();
            _consumableProvider = ServiceProvider.GetService<IConsumableProvider>();

        }
        [UnityTest]
        public IEnumerator ShowShop_WithShowShopEventSO_ShouldShowShopUIAndWelcomeDialog()
        {
            Assert.IsNotNull(_showShopEvent, "Event was null");
            _showShopEvent.RaiseEvent(null);


            yield return null;

            var GO = GameObject.Find(shopObjectName);


            Assert.IsNotNull(GO, "Shop doesn't show after trigger event");

            Assert.IsTrue(GO.gameObject.activeSelf, "Expected : shop panel should be displayed");

            var dialogGO = GameObject.Find(shopObjectName);


            Assert.IsNotNull(dialogGO, "Message Dialog doesn't show after trigger event");

            Assert.IsTrue(dialogGO.gameObject.activeSelf, "Expected: Dialog message should be displayed");

        }


        [UnityTest]
        public IEnumerator BuyEquipmentItem_207011000110_WithEnoughMoney_ReturnSuccess()
        {
            string itemId = "201011000111";

            var equipment = new EquipmentInfo(itemId);

            yield return _equipmentDefProvider.Load(equipment);

            IShopItem shopItem = new EquipmentItem(equipment);

            _inventorySO.WalletController.Wallet.Gold.SetCurrencyAmount(equipment.Price * 2);

            var result = shopItem.TryToBuy(_shopInventoryController);

            Assert.IsTrue(result, "Expect : buy item sucess");
        }

        [UnityTest]
        public IEnumerator BuyEquipmentItem_201011000111_WithNotEnoughMoney_ReturnFail()
        {
            string itemId = "201011000111";

            var equipment = new EquipmentInfo(itemId);

            yield return _equipmentDefProvider.Load(equipment);

            IShopItem shopItem = new EquipmentItem(equipment);

            _inventorySO.WalletController.Wallet.Gold.SetCurrencyAmount(0);

            var result = shopItem.TryToBuy(_shopInventoryController);

            Assert.IsFalse(result, "Expect : buy item fail");
        }

        [UnityTest]
        public IEnumerator BuyConsumableItem_5001_WithEnoughMoney_ReturnSuccess()
        {
            string itemId = "5001";

            var item = new ConsumableInfo(itemId);
            yield return _consumableProvider.Load(item);

            IShopItem shopItem = new ConsumableItem(item);


            _inventorySO.WalletController.Wallet.Gold.SetCurrencyAmount(item.Price * 2);

            var result = shopItem.TryToBuy(_shopInventoryController);

            Assert.IsTrue(result, "Expect : buy item sucess");
        }

        [UnityTest]
        public IEnumerator BuyConsumableItem_5001_WithNotEnoughMoney_ReturnFail()
        {
            string itemId = "5001";
            var item = new ConsumableInfo(itemId);
            yield return _consumableProvider.Load(item);

            IShopItem shopItem = new ConsumableItem(item);

            _inventorySO.WalletController.Wallet.Gold.SetCurrencyAmount(0);

            var result = shopItem.TryToBuy(_shopInventoryController);

            Assert.IsFalse(result, "Expect : buy item sucess");
        }

        [UnityTest]
        public IEnumerator SellItemFromInventory_WithWeaponItem_ItemRemoveSuccessAndGoldIncrease()
        {
            yield return new WaitForSeconds(5); // Wait for inventory data loaded
            var items = _inventorySO.GetWeapons();
            var goldInfo = _inventorySO.WalletController.Wallet.Gold;

            foreach (var item in items)
            {
                IShopItem shopItem = new EquipmentItem(item);

                var currentGold = goldInfo.Amount;
                var sellPrice = shopItem.SellPrice;

                var result = shopItem.TryToSell(_shopInventoryController);

                Assert.IsTrue(result, "Expected : Sell Success");

                Assert.AreEqual(goldInfo.Amount, currentGold + sellPrice, "Expected : New Amount equal last amount + selling price");

                foreach (var equip in _inventorySO.Equipments)
                {
                    Assert.AreNotEqual(equip.Id, item.Id, "Expected : Item removed from inventory");
                }
            }
        }

        [UnityTest]
        public IEnumerator SellItemFromInventory_WithNonWeaponItem_ItemRemoveSuccessAndGoldIncrease()
        {
            yield return new WaitForSeconds(5); // Wait for inventory data loaded
            var items = _inventorySO.GetNonWeapons();
            var goldInfo = _inventorySO.WalletController.Wallet.Gold;

            foreach (var item in items)
            {
                IShopItem shopItem = new EquipmentItem(item);

                var currentGold = goldInfo.Amount;
                var sellPrice = shopItem.SellPrice;

                var result = shopItem.TryToSell(_shopInventoryController);

                Assert.IsTrue(result, "Expected : Sell Success");

                Assert.AreEqual(goldInfo.Amount, currentGold + sellPrice, "Expected : New Amount equal last amount + selling price");

                foreach (var equip in _inventorySO.Equipments)
                {
                    Assert.AreNotEqual(equip.Id, item.Id, "Expected : Item removed from inventory");
                }
            }
        }

        [UnityTest]
        public IEnumerator SellItemFromInventory_WithConsumableItem_ItemRemoveSuccessAndGoldIncrease()
        {
            yield return null;
            var items = _inventorySO.GetConsumables();
            var goldInfo = _inventorySO.WalletController.Wallet.Gold;

            foreach (var item in items)
            {
                IShopItem shopItem = new ConsumableItem(item);

                var currentGold = goldInfo.Amount;
                var sellPrice = shopItem.SellPrice;
                var quantity = item.Quantity - 1;

                var result = shopItem.TryToSell(_shopInventoryController);

                Assert.IsTrue(result, "Expected : Sell Success");

                Assert.AreEqual(goldInfo.Amount, currentGold + sellPrice, "Expected : New Amount equal last amount + selling price");

                foreach (var consumable in _inventorySO.Consumables)
                {
                    if (item.Quantity > 1 && consumable.Data.ID == item.Data.ID)
                    {
                        Assert.AreEqual(consumable.Quantity, quantity, "Expected : Item quantity decrease 1 from inventory");
                    }
                    else
                    {
                        Assert.AreNotEqual(consumable.Data.ID, item.Data.ID, "Expected : Item removed from inventory");
                    }
                }
            }
        }
#endif
    }
}
