using System.Collections;
using System.Collections.Generic;

namespace CryptoQuest.BlackSmith.Interface
{
    public interface IEvolvableEquipment
    {
        public List<IEvolvableData> EvolvableData { get; }
        public IEnumerator CoGetData();
    }
}