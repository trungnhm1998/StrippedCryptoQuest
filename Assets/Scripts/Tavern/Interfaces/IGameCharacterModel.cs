using System.Collections;
using System.Collections.Generic;

namespace CryptoQuest.Tavern.Interfaces
{
    public interface IGameCharacterModel
    {
        public List<IGameCharacterData> Data { get; }
        public IEnumerator CoGetData();
    }
}