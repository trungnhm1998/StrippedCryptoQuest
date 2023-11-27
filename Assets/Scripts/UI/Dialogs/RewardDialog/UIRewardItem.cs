using System.Collections;
using CryptoQuest.Gameplay.Loot;
using CryptoQuest.Item.Equipment;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace CryptoQuest.UI.Dialogs.RewardDialog
{
    public class UIRewardItem : MonoBehaviour
    {
        [SerializeField] private EquipmentPrefabDatabase _prefabDatabase;
        [SerializeField] private LocalizedString _itemWithQuantity;
        [SerializeField] private LocalizeStringEvent _content;
        [SerializeField] private TMP_Text _text;

        public void SetLoot(LootInfo loot)
        {
            loot.Accept(this);
        }

        public void Visit(ConsumableLootInfo loot)
        {
            var msg = new LocalizedString(_itemWithQuantity.TableReference, _itemWithQuantity.TableEntryReference)
            {
                {
                    "item", loot.Item.Data.DisplayName
                },
                {
                    "quantity", new IntVariable { Value = loot.Item.Quantity }
                }
            };
            _content.StringReference = msg;
        }

        public void Visit(CurrencyLootInfo loot)
        {
            _text.text = $"{loot.Item.Amount} {loot.Item.Data.DisplayName.GetLocalizedString()}";
        }

        public void Visit(EquipmentLoot loot)
        {
            _text.text = loot.EquipmentSO.name;
            StartCoroutine(CoLoadName(loot.EquipmentSO.Data.PrefabId));
        }
        
        private IEnumerator CoLoadName(string prefabId)
        {
            yield return _prefabDatabase.LoadDataById(prefabId);
            _content.StringReference = _prefabDatabase.GetDataById(prefabId).DisplayName;
        }

        public void Visit(ExpLoot loot) => _text.text = $"{loot.Exp} EXP";

        private void OnDisable() => _prefabDatabase.ReleaseAllData();
    }
}