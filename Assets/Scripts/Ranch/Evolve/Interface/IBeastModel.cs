using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Beast;
using CryptoQuest.Item.Equipment;

namespace CryptoQuest.Ranch.Evolve.Interface
{
    public interface IBeastModel
    {
        public List<IBeast> Beasts { get; }
        public IEnumerator CoGetData(BeastInventorySO inventory);
    }
}