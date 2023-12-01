using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;

namespace CryptoQuest.BlackSmith.Interface
{
    public interface IEvolvableModel
    {
        public List<IEvolvableEquipment> EvolvableEquipments { get; }
        public IEnumerator CoGetData(InventorySO inventory);
    }
}