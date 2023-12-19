using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Item.Equipment;

namespace CryptoQuest.BlackSmith.Interface
{
    public interface IUpgradeModel
    {
        public List<IEquipment> Equipments { get; }
        public IEnumerator CoGetData();
    }
}