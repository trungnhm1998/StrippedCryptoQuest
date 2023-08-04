using System;
using CryptoQuest.Data.Item;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory
{
    [Serializable]
    public class EquipmentInformation : ItemInformation
    {
        [SerializeField] private RaritySO _raritySo;
        protected override void Activate() { }

        public void Equip() => Activate();
    }
}