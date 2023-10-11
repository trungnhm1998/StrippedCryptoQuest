using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using CryptoQuest.Item;
using CryptoQuestEditor.Helper;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.GameplayEffectActions;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using NotImplementedException = System.NotImplementedException;

namespace CryptoQuestEditor
{
    [CustomEditor(typeof(ConsumableSO))]
    public class UsableSOEditor : Editor
    {
        private const string MAIN_INVENTORY = "MainInventory";

        [SerializeField] private VisualTreeAsset _uxml;
        public VisualTreeAsset _consumableUxml;
        private ConsumableSO Target => target as ConsumableSO;
        private InventorySO InventorySO { get; set; }

        private HelpBox _helpBox;
        private ObjectField _inventoryField;
        private Button _addButton;
        private Button _addEffectBtn;
        private Button _addAbilityBtn;

        private void OnEnable()
        {
            InventorySO = ToolsHelper.GetAsset<InventorySO>(MAIN_INVENTORY);
        }

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            _uxml.CloneTree(root);
            if (_consumableUxml != null) _consumableUxml.CloneTree(root);

            _helpBox = root.Q<HelpBox>("help-box");
            _inventoryField = root.Q<ObjectField>("inventory-field");
            _inventoryField.value = InventorySO;
            _addButton = root.Q<Button>("add-button");
            _addAbilityBtn = root.Q<Button>("add-ability-button");
            _addEffectBtn = root.Q<Button>("add-effect-button");

            Notification();

            _inventoryField.RegisterValueChangedCallback(Notification);
            _addButton.clicked += AddEquipment;
            if (Target.Effect == null)
            {
                _addEffectBtn.text = "Add Effect";
                _addEffectBtn.clicked += AddConsumableEffect;
            }
            else
            {
                _addEffectBtn.text = "Remove Effect";
                _addEffectBtn.clicked += RemoveEffect;
            }

            if (Target.Ability == null)
            {
                _addAbilityBtn.text = "Add Ability";
                _addAbilityBtn.clicked += AddAbility;
            }
            else
            {
                _addAbilityBtn.text = "Remove Ability";
                _addAbilityBtn.clicked += RemoveAbility;
            }


            return root;
        }

        private void Notification(ChangeEvent<Object> evt = null)
        {
            if (_inventoryField.value == null || evt?.newValue == null)
            {
                _helpBox.messageType = HelpBoxMessageType.Warning;
                _helpBox.text = "If you don't set inventory, default inventory will be MainInventory";
            }
            else
            {
                InventorySO = (InventorySO)_inventoryField.value;

                _helpBox.messageType = HelpBoxMessageType.Info;
                _helpBox.text =
                    $"The current inventory is {InventorySO.name}, {Target.name} will be added into this inventory";
            }
        }

        private void AddEquipment()
        {
            InventorySO.Consumables.Add(new ConsumableInfo(Target));
        }

        private void AddAbility()
        {
            _addAbilityBtn.clicked -= AddAbility;
            _addAbilityBtn.clicked += RemoveAbility;
            _addAbilityBtn.text = "Remove Ability";
            var ability = CreateInstance<ConsumeItemAbility>();
            ability.name = $"{Target.name}Ability";
            AssetDatabase.AddObjectToAsset(ability, Target);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Target.Editor_SetAbility(ability);
        }

        private void RemoveAbility()
        {
            _addAbilityBtn.clicked -= RemoveAbility;
            _addAbilityBtn.clicked += AddAbility;
            _addAbilityBtn.text = "Add Ability";
            AssetDatabase.RemoveObjectFromAsset(Target.Ability);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Target.Editor_SetAbility(null);
        }


        private void RemoveEffect()
        {
            _addEffectBtn.clicked -= RemoveEffect;
            _addEffectBtn.clicked += AddConsumableEffect;
            _addEffectBtn.text = "Add Effect";
            AssetDatabase.RemoveObjectFromAsset(Target.Effect);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Target.Editor_SetEffect(null);
        }

        private void AddConsumableEffect()
        {
            _addEffectBtn.clicked -= AddConsumableEffect;
            _addEffectBtn.clicked += RemoveEffect;
            _addEffectBtn.text = "Remove Effect";
            var effect = CreateInstance<GameplayEffectDefinition>();
            effect.name = $"{Target.name}Effect";
            effect.Policy = new InstantPolicy();
            AssetDatabase.AddObjectToAsset(effect, Target);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Target.Editor_SetEffect(effect);
        }
    }
}