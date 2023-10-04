using System;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Item.Equipment;
using CryptoQuest.Menu;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.BlackSmith.EvolveStates.UI
{
    public class UIEquipmentItem : MonoBehaviour
    {
        public void SetItemData(EquipmentInfo equipment)
        {
            Debug.Log($"EvolveUIEquipmentItem::equipment = [{equipment}]");
        }
    }
}