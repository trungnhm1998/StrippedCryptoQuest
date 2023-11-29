using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;

namespace CryptoQuest.BlackSmith.Interface
{
    public interface IUpgradeModel
    {
        public List<IUpgradeEquipment> Equipments { get; }
        public IEnumerator CoGetData(InventorySO inventory);
    }
}