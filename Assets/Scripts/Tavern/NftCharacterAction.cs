using System.Collections.Generic;
using CryptoQuest.Core;
using CryptoQuest.Tavern.Interfaces;

namespace CryptoQuest.Tavern
{
    public class NftCharacterAction : ActionBase { }

    public class GetInGameNftCharactersSucceed : ActionBase
    {
        public List<IGameCharacterData> InGameCharacters { get; }

        public GetInGameNftCharactersSucceed(List<IGameCharacterData> inGameCharacters)
        {
            InGameCharacters = inGameCharacters;
        }
    }

    public class GetWalletNftCharactersSucceed : ActionBase
    {
        public List<IWalletCharacterData> WalletCharacters { get; }

        public GetWalletNftCharactersSucceed(List<IWalletCharacterData> walletCharacters)
        {
            WalletCharacters = walletCharacters;
        }
    }
}