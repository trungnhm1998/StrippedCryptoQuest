using System.Collections.Generic;
using CryptoQuest.Core;
using Obj = CryptoQuest.Sagas.Objects;

namespace CryptoQuest.Tavern
{
    public class GetCharacters : ActionBase
    {
        public Obj.ECharacterStatus Status { get; set; } = Obj.ECharacterStatus.All;
    }

    public class GetInGameHeroes : ActionBase
    {
        public Obj.ECharacterStatus Status { get; set; } = Obj.ECharacterStatus.All;
    }

    public class GetFilteredInGameNftCharactersSucceed : ActionBase
    {
        public List<Obj.Character> FilteredInGameCharacters { get; }

        public GetFilteredInGameNftCharactersSucceed(List<Obj.Character> filteredFilteredInGameCharacters)
        {
            FilteredInGameCharacters = filteredFilteredInGameCharacters;
        }
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

    public class TransferSucceed : ActionBase
    {
        public Obj.Character[] ResponseCharacters { get; }

        public TransferSucceed(Obj.Character[] responseCharacters)
        {
            ResponseCharacters = responseCharacters;
        }
    }

    public class TransferFailed : ActionBase { }

    public class FetchInGameHeroesSucceeded : ActionBase
    {
        public List<Obj.Character> InGameHeroes { get; }

        public FetchInGameHeroesSucceeded(List<Obj.Character> inGameHeroes)
        {
            InGameHeroes = inGameHeroes;
        }
    }
}