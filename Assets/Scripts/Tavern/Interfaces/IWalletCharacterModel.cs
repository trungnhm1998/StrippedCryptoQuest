using System.Collections;
using System.Collections.Generic;

namespace CryptoQuest.Tavern.Interfaces
{
    public interface IWalletCharacterModel
    {
        public List<IWalletCharacterData> Data { get; }
        public IEnumerator CoGetData();
    }
}