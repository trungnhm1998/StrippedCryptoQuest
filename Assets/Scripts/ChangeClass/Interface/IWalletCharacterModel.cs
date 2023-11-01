
using System.Collections;
using System.Collections.Generic;

namespace CryptoQuest.ChangeClass.Interfaces
{
    public interface IWalletCharacterModel
    {
        public bool IsLoaded { get; }
        public List<ICharacterModel> Data { get; }
        public IEnumerator CoGetData();
    }
}