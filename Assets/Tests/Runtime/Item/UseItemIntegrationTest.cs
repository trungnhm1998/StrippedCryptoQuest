using System.Collections;
using CryptoQuest.Gameplay.Battle.Core.Components;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Skills;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using NUnit.Framework;

#if UNITY_EDITOR
using CryptoQuest.Data.Item;
using CryptoQuest.Gameplay.Inventory;
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace CryptoQuest.Tests.Runtime.Item
{
    public class UseItemIntegrationTest
    {
#if UNITY_EDITOR
        private readonly WaitForSeconds WAIT_ONE_SECOND = new(1);
        private const string ITEMS_TEST_SCENE = "Assets/Tests/Runtime/Items.unity";
        private UsableSO _healItem;
        private UsableInformation _healItemInfo;
        private GameObject _inventoryController;
        private InventoryController _inventoryControllerComponent;
        private BattleTeam _battleTeam;
        private AbilitySystemBehaviour _player;
        private AttributeScriptableObject _playerHealthAttribute;
        private string[] attributeGuids;

        [UnitySetUp]
        public IEnumerator OneTimeSetup()
        {
            yield return LoadTestScene();
            FindReferences();
        }

        private IEnumerator LoadTestScene()
        {
            yield return EditorSceneManager.LoadSceneAsyncInPlayMode(ITEMS_TEST_SCENE,
                new LoadSceneParameters(LoadSceneMode.Single));
        }

        private void FindReferences()
        {
            _healItem = GetItemSO("NFTItem");
            Assert.NotNull(_healItem);

            _inventoryController = GameObject.Find("InventoryController");
            Assert.NotNull(_inventoryController);

            _inventoryControllerComponent = _inventoryController.GetComponent<InventoryController>();
            Assert.NotNull(_inventoryControllerComponent);

            _battleTeam = GameObject.Find("PlayerPartyManager").GetComponent<BattleTeam>();
            Assert.NotNull(_battleTeam);

            _player = _battleTeam.Members[0];
            Assert.NotNull(_player);

            _playerHealthAttribute = GetAttributeScriptableObject("Player.HP");
            Assert.NotNull(_playerHealthAttribute);

            _healItemInfo = new UsableInformation(_healItem, _player);
            Assert.NotNull(_healItemInfo);
        }

        [UnityTest]
        public IEnumerator UseHealItem_ChangePlayerHealth_ReturnHealthIncrease()
        {
            _player.AttributeSystem.GetAttributeValue(_playerHealthAttribute, out var playerHealthBeforeUse);
            _healItemInfo.UseItem();
            yield return WAIT_ONE_SECOND;

            _player.AttributeSystem.GetAttributeValue(_playerHealthAttribute, out var playerHealthAfterUse);
            Assert.That(playerHealthBeforeUse.CurrentValue < playerHealthAfterUse.CurrentValue);
        }

        [UnityTest]
        public IEnumerator UseHealItem_Heal100HP_ReturnPlayerHPIncreased100()
        {
            SetupHealItem();
            yield return WAIT_ONE_SECOND;
            _player.AttributeSystem.GetAttributeValue(_playerHealthAttribute, out var playerHealthBeforeUse);

            _healItemInfo.UseItem();
            float expectedValue = playerHealthBeforeUse.CurrentValue + 100;
            yield return WAIT_ONE_SECOND;

            _player.AttributeSystem.GetAttributeValue(_playerHealthAttribute, out var playerHealthAfterUse);
            Assert.That(playerHealthAfterUse.CurrentValue == expectedValue);
        }

        private void SetupHealItem()
        {
            var expendableItem = _healItem as UsableSO;
            Assert.NotNull(expendableItem);

            var healAbility = expendableItem.Ability as AbilitySO;

            EffectAttributeModifier newModifier = new()
            {
                AttributeSO = _playerHealthAttribute,
                ModifierType = EAttributeModifierType.Add,
                ModifierComputationMethod = null,
                Value = 100
            };
            healAbility.EffectContainerMap[0].TargetContainer[0].Effects[0]
                .EffectDetails.Modifiers[0] = newModifier;
        }

        public UsableSO GetItemSO(string itemName)
        {
            var guids = AssetDatabase.FindAssets("t:UsableSO");
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var healItemSo = AssetDatabase.LoadAssetAtPath<UsableSO>(path);
                if (healItemSo.name == itemName)
                {
                    return healItemSo;
                }
            }

            return null;
        }

        public AttributeScriptableObject GetAttributeScriptableObject(string attributeName)
        {
            if (attributeGuids == null)
                attributeGuids = AssetDatabase.FindAssets("t:AttributeScriptableObject");
            foreach (var guid in attributeGuids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var attrSo = AssetDatabase.LoadAssetAtPath<AttributeScriptableObject>(path);
                if (attrSo.name == attributeName)
                {
                    return attrSo;
                }
            }

            return null;
        }
    }
#endif
}