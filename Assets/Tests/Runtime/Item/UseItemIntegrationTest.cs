using System.Collections;
using CryptoQuest.Gameplay.Battle.Core.Components;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Skills;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace CryptoQuest.Tests.Runtime.Item
{
    public class UseItemIntegrationTest
    {
        private const int SECOND_TO_WAIT = 3;

        private ItemSO healItem;
        private GameObject iventoryController;
        private InventoryController inventoryControllerComponent;
        private BattleTeam battleTeam;
        private AbilitySystemBehaviour player;
        private AttributeScriptableObject playerHealthAttribute;

        public void SetUp()
        {
            healItem = GetItemSO("HealItem");
            Assert.NotNull(healItem);

            iventoryController = GameObject.Find("InventoryController");
            Assert.NotNull(iventoryController);

            inventoryControllerComponent = iventoryController.GetComponent<InventoryController>();
            Assert.NotNull(inventoryControllerComponent);

            battleTeam = GameObject.Find("PlayerPartyManager").GetComponent<BattleTeam>();
            Assert.NotNull(battleTeam);

            player = battleTeam.Members[0];
            Assert.NotNull(player);

            playerHealthAttribute = GetAttributeScriptableObject("Player.HP");
            Assert.NotNull(playerHealthAttribute);
        }

        [UnityTest]
        public IEnumerator UseHealItem_ChangePlayerHealth_ReturnHealthIncrease()
        {
            const string startupSceneName = "Startup";
            yield return SceneManager.LoadSceneAsync(startupSceneName, LoadSceneMode.Single);

            yield return new WaitForSeconds(SECOND_TO_WAIT);

            Assert.That(SceneManager.GetSceneByName("GlobalManagers").isLoaded);
            SetUp();

            player.AttributeSystem.GetAttributeValue(playerHealthAttribute, out var playerHealthBeforeUse);
            inventoryControllerComponent.CurrentOwnerAbilitySystemBehaviour = player;
            healItem.Use(player);
            yield return new WaitForSeconds(1);

            player.AttributeSystem.GetAttributeValue(playerHealthAttribute, out var playerHealthAfterUse);
            Assert.That(playerHealthBeforeUse.CurrentValue < playerHealthAfterUse.CurrentValue);
        }

        [UnityTest]
        public IEnumerator UseHealItem_Heal100HP_ReturnPlayerHPIncreased100()
        {
            const string startupSceneName = "Startup";
            yield return SceneManager.LoadSceneAsync(startupSceneName, LoadSceneMode.Single);

            yield return new WaitForSeconds(SECOND_TO_WAIT);

            Assert.That(SceneManager.GetSceneByName("GlobalManagers").isLoaded);
            SetUp();
            ExpendableItemSO expendableItem = healItem as ExpendableItemSO;
            Assert.NotNull(expendableItem);

            AbilitySO healAbility = expendableItem.Ability as AbilitySO;

            EffectAttributeModifier newModifier = new()
            {
                AttributeSO = playerHealthAttribute,
                ModifierType = EAttributeModifierType.Add,
                ModifierComputationMethod = null,
                Value = 100
            };
            healAbility.EffectContainerMap[0].TargetContainer[0].Effects[0]
                .EffectDetails.Modifiers[0] = newModifier;
            yield return new WaitForSeconds(1);
            player.AttributeSystem.GetAttributeValue(playerHealthAttribute, out var playerHealthBeforeUse);
            inventoryControllerComponent.CurrentOwnerAbilitySystemBehaviour = player;
            healItem.Use(player);
            float expectedValue = playerHealthBeforeUse.CurrentValue + 100;
            yield return new WaitForSeconds(1);

            player.AttributeSystem.GetAttributeValue(playerHealthAttribute, out var playerHealthAfterUse);
            Assert.That(playerHealthAfterUse.CurrentValue == expectedValue);
        }


        public ItemSO GetItemSO(string itemName)
        {
            ItemSO itemSO = null;
            var guids = AssetDatabase.FindAssets("t:ItemSO");
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var healItemSo = AssetDatabase.LoadAssetAtPath<ItemSO>(path);
                if (healItemSo.name == itemName)
                    itemSO = healItemSo;
            }

            return itemSO;
        }

        public AttributeScriptableObject GetAttributeScriptableObject(string attributeName)
        {
            AttributeScriptableObject attribureSO = null;
            var guids = AssetDatabase.FindAssets("t:AttributeScriptableObject");
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var attrSo = AssetDatabase.LoadAssetAtPath<AttributeScriptableObject>(path);
                if (attrSo.name == attributeName)
                    attribureSO = attrSo;
            }

            return attribureSO;
        }
    }
}