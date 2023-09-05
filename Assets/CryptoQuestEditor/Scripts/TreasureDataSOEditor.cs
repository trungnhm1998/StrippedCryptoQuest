using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Gameplay.Inventory.Items;
using CryptoQuest.Gameplay.Treasure;
using UnityEditor;
using UnityEngine;

namespace CryptoQuestEditor
{
    [CustomEditor(typeof(TreasureDataSO))]
    public class TreasureDataSOEditor : Editor
    {
        private TreasureDataSO Target => target as TreasureDataSO;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Add Equipment"))
            {
                Editor_AddLoot(new EquipmentInfo());
            }

            if (GUILayout.Button("Add Consumable"))
            {
                Editor_AddLoot(new UsableInfo());
            }

            if (GUILayout.Button("Add Currency"))
            {
                Editor_AddLoot(new CurrencyInfo());
            }
        }

        private void Editor_AddLoot(EquipmentInfo equipment)
        {
            Target.LootInfos.Add(equipment);
        }

        private void Editor_AddLoot(UsableInfo consumable)
        {
            Target.LootInfos.Add(consumable);
        }

        private void Editor_AddLoot(CurrencyInfo currency)
        {
            Target.LootInfos.Add(currency);
        }
    }
}