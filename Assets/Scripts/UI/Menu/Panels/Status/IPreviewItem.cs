using CryptoQuest.Item;
using CryptoQuest.Item.Equipment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Panels.Status
{
    public interface IPreviewItem
    {
        void Preview(EquipmentInfo equipment);
        void Preview(ConsumableInfo consumable);
        void Hide();
    }
}
