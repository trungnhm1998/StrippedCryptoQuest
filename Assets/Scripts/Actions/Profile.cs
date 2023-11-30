using System.Collections.Generic;
using CryptoQuest.Sagas.Objects;
using IndiGames.Core.Events;
using Obj = CryptoQuest.Sagas.Objects;

namespace CryptoQuest.Actions
{
    public class GetProfile : ActionBase { }

    public class GetProfileSucceed : ActionBase { }

    public class GetProfileFailed : ActionBase { }

    public class FetchProfileEquipmentsAction : ActionBase { }

    public class SaveGameAction : ActionBase { }

    public class InventoryFilled : ActionBase { }

    public class UpdateWallet : ActionBase
    {
        public int Gold { get; }
        public int Soul { get; }
        public int Diamond { get; }
        public string WalletAddress { get; }

        public UpdateWallet(int gold, int soul, int diamond, string walletAddress)
        {
            Gold = gold;
            Soul = soul;
            Diamond = diamond;
            WalletAddress = walletAddress;
        }
    }

    public class FetchProfileCharactersAction : ActionBase { }

    public class GetGameNftCharactersSucceed : ActionBase
    {
        public List<Obj.Character> InGameCharacters { get; }

        public GetGameNftCharactersSucceed(List<Obj.Character> inGameCharacters)
        {
            InGameCharacters = inGameCharacters;
        }
    }

    public class FetchProfileBeastAction : ActionBase { }

    public class GetGameNftBeastsSucceed : ActionBase
    {
        public List<BeastData> InGameBeasts { get; }
        
        public GetGameNftBeastsSucceed(List<BeastData> inGameBeasts)
        {
            InGameBeasts = inGameBeasts;
        }
    }
}