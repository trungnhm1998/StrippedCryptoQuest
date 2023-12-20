using System.Collections.Generic;
using CryptoQuest.Sagas.Objects;
using IndiGames.Core.Events;

namespace CryptoQuest.Ranch
{
    public class GetBeasts : ActionBase
    {
        public EBeastStatus Status { get; set; } = EBeastStatus.All;
    }

    public class GetInBoxBeastsSucceed : ActionBase
    {
        public List<BeastResponse> WalletBeasts { get; }

        public GetInBoxBeastsSucceed(List<BeastResponse> walletBeasts)
        {
            WalletBeasts = walletBeasts;
        }
    }

    public class GetNftBeastsFailed : ActionBase { }

    public class SendBeastsToBothSide : ActionBase
    {
        public int[] SelectedInDimensionBoxBeasts { get; }
        public int[] SelectedInGameBeasts { get; }

        public SendBeastsToBothSide(int[] selectedInGameBeasts, int[] selectedInDimensionBoxBeasts)
        {
            SelectedInGameBeasts = selectedInGameBeasts;
            SelectedInDimensionBoxBeasts = selectedInDimensionBoxBeasts;
        }
    }

    public class TransferSucceed : ActionBase
    {
        public BeastResponse[] ResponseBeasts { get; }

        public TransferSucceed(BeastResponse[] responseBeasts)
        {
            ResponseBeasts = responseBeasts;
        }
    }

    public class TransferFailed : ActionBase { }

    public class GetInGameBeastsSucceed : ActionBase
    {
        public List<BeastResponse> InGameBeasts { get; }

        public GetInGameBeastsSucceed(List<BeastResponse> inGameBeasts)
        {
            InGameBeasts = inGameBeasts;
        }
    }

    public class GetNftBeastsSucceed : ActionBase { }
}