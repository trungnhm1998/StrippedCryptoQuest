using System.Collections.Generic;
using CryptoQuest.Core;
using CryptoQuest.Tavern.Interfaces;
using CryptoQuest.Tavern.Objects;

namespace CryptoQuest.Tavern
{
    public class GetCharacters : ActionBase
    {
        public ETavernStatus Status { get; set; } = ETavernStatus.All;
        public bool ForceRefresh { get; set; } = false;
    }

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

    public class GetNftCharactersFailed : ActionBase { }

    public class SendCharactersToWallet : ActionBase
    {
        public int[] SelectedInGameCharacters { get; }
        public SendCharactersToWallet(int[] selectedInGameCharacters)
        {
            SelectedInGameCharacters = selectedInGameCharacters;
        }
    }

    public class SendCharactersToGame : ActionBase
    {
        public int[] SelectedInWalletCharacters { get; }
        public SendCharactersToGame(int[] selectedInWalletCharacters)
        {
            SelectedInWalletCharacters = selectedInWalletCharacters;
        }
    }

    public class TransferSucceed : ActionBase { }
}