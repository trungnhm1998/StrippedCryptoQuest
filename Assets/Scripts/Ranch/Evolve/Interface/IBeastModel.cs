using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Beast;
using CryptoQuest.Item.Equipment;

namespace CryptoQuest.Ranch.Evolve.Interface
{
    public interface IBeastModel
    {
        public List<Beast.Beast> Beasts { get; }
        public IEnumerator CoGetData(BeastInventorySO inventory);
    }
}