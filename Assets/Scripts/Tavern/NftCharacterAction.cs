using System.Collections.Generic;
using CryptoQuest.Core;
using CryptoQuest.Tavern.Interfaces;

namespace CryptoQuest.Tavern
{
    public class NftCharacterAction : ActionBase { }

    public class GetGameNftCharactersSucceed : ActionBase
    {
        public List<ICharacterData> InGameCharacters { get; }

        public GetGameNftCharactersSucceed(List<ICharacterData> inGameCharacters)
        {
            InGameCharacters = inGameCharacters;
        }
    }

    public class GetWalletNftCharactersSucceed : ActionBase
    {
        public List<ICharacterData> WalletCharacters { get; }

        public GetWalletNftCharactersSucceed(List<ICharacterData> walletCharacters)
        {
            WalletCharacters = walletCharacters;
        }
    }
}