using CryptoQuest.Item.Equipment;
using CryptoQuest.Shop;
using CryptoQuest.UI.Menu.Panels.Status.Equipment;
using PolyAndCode.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.Shop.UI.Panels.Item
{
    public class UIShopBuyEquipment : UIShopBuy
    {
        [SerializeField] private EquipmentDatabaseSO _equipmentDatabaseSO;
        [SerializeField] private EquipmentDefineDatabase _equipmentDefineDatabase;
        protected override void InitItem()
        {
            StartCoroutine(LoadItem(_shopItemTable.Items));
        }

        IEnumerator LoadItem(List<string> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                var equipment = new EquipmentInfo(items[i]);
                yield return _equipmentDefineDatabase.LoadDataById(equipment.DefinitionId);
                equipment.Def = _equipmentDefineDatabase.GetDataById(equipment.DefinitionId);
                if (equipment.Def != null)
                {
                    yield return _equipmentDatabaseSO.LoadDataById(equipment.Def.PrefabId);
                    equipment.Prefab = _equipmentDatabaseSO.GetDataById(equipment.Def.PrefabId);
                    if (equipment.Prefab != null)
                    {
                        IShopItemData shopItemData = new EquipmentItem(equipment);

                        InstantiateItem(shopItemData, true, i);
                    }
                }
            }
            yield return SelectDefaultButton();
        }
    }
}
