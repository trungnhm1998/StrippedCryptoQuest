using System;
using CryptoQuest.BlackSmith.Interface;
using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Input;
using CryptoQuest.Item.Equipment;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.BlackSmith.Upgrade
{
    public interface IUpgradeEquipmentValidator
    {
        bool CanUpgrade(IEquipment equipment, int toLevel);
        bool IsEnoughGoldToUpgrade(float goldOwned, float cost);
    }

    public class UpgradeEquipmentValidator : IUpgradeEquipmentValidator
    {
        public bool CanUpgrade(IEquipment equipment, int toLevel)
            => toLevel > equipment.Level && toLevel <= equipment.Data.MaxLevel;

        public bool IsEnoughGoldToUpgrade(float goldOwned, float cost)
        {
            return goldOwned >= cost;
        }
    }
}