using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;

namespace CryptoQuest.BlackSmith.Interface
{
    public interface IUpgradeModel
    {
        public List<IUpgradeEquipment> ListEquipment { get; }
        public void CoGetData(InventorySO inventory);
    }
}