using System.Collections.Generic;
using CryptoQuest.Sagas.Objects;
using IndiGames.Core.Events;

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
        public float Diamond { get; }
        public string WalletAddress { get; }

        public UpdateWallet(int gold, int soul, float diamond, string walletAddress)
        {
            Gold = gold;
            Soul = soul;
            Diamond = diamond;
            WalletAddress = walletAddress;
        }
    }

    public class FetchProfileBeastAction : ActionBase { }

    public class GetGameNftBeastsSucceed : ActionBase
    {
        public List<BeastResponse> InGameBeasts { get; }

        public GetGameNftBeastsSucceed(List<BeastResponse> inGameBeasts)
        {
            InGameBeasts = inGameBeasts;
        }
    }

    public class FetchProfileConsumablesAction : ActionBase { }
}