using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Gameplay.Loot;
using CryptoQuest.Item;
using CryptoQuest.Item.Equipment;
using UnityEditor;
using UnityEngine;

namespace CryptoQuestEditor
{
    [CustomEditor(typeof(LootTable))]
    public class LootDataSOEditor : Editor
    {
        private LootTable Target => target as LootTable;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Add Equipment"))
            {
                Editor_AddLoot(new EquipmentInfo());
            }

            if (GUILayout.Button("Add Consumable"))
            {
                Editor_AddLoot(new ConsumableInfo());
            }

            if (GUILayout.Button("Add Currency"))
            {
                Editor_AddLoot(new CurrencyInfo());
            }
        }

        private void Editor_AddLoot(EquipmentInfo equipment)
        {
            Target.LootInfos.Add(new EquipmentLootInfo(equipment));
        }

        private void Editor_AddLoot(ConsumableInfo consumable)
        {
            Target.LootInfos.Add(new UsableLootInfo(consumable));
        }

        private void Editor_AddLoot(CurrencyInfo currency)
        {
            Target.LootInfos.Add(new CurrencyLootInfo(currency));
        }
    }
}