using System.Collections.Generic;
using CryptoQuest.Core;
using CryptoQuest.Sagas.Objects;
using Obj = CryptoQuest.Sagas.Objects;

namespace CryptoQuest.Tavern
{
    public class GetCharacters : ActionBase
    {
        public ECharacterStatus Status { get; set; } = ECharacterStatus.All;
    }

    public class GetWalletNftCharactersSucceed : ActionBase
    {
        public List<Obj.Character> WalletCharacters { get; }

        public GetWalletNftCharactersSucceed(List<Obj.Character> walletCharacters)
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

    public class SendCharactersToBothSide : ActionBase
    {
        public int[] SelectedInWalletCharacters { get; }
        public int[] SelectedInGameCharacters { get; }
        public SendCharactersToBothSide(int[] selectedInGameCharacters, int[] selectedInWalletCharacters)
        {
            SelectedInGameCharacters = selectedInGameCharacters;
            SelectedInWalletCharacters = selectedInWalletCharacters;
        }
    }

    public class TransferSucceed : ActionBase { }
    public class TransferFailed : ActionBase { }

    public class GetInPartyNftCharactersSucceed : ActionBase
    {
        public List<Obj.Character> InPartyCharacters { get; }

        public GetInPartyNftCharactersSucceed(List<Obj.Character> inPartyCharacters)
        {
            InPartyCharacters = inPartyCharacters;
        }
    }
}