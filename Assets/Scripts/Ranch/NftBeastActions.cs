using System.Collections.Generic;
using CryptoQuest.Core;
using Obj = CryptoQuest.Sagas.Objects;

namespace CryptoQuest.Ranch
{
    public class GetBeasts : ActionBase
    {
        public Obj.EBeastStatus Status { get; set; } = Obj.EBeastStatus.All;
    }

    public class GetInBoxBeastsSucceed : ActionBase
    {
        public List<Obj.Beast> WalletBeasts { get; }

        public GetInBoxBeastsSucceed(List<Obj.Beast> walletBeasts)
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
        public Obj.Beast[] ResponseBeasts { get; }

        public TransferSucceed(Obj.Beast[] responseBeasts)
        {
            ResponseBeasts = responseBeasts;
        }
    }

    public class TransferFailed : ActionBase { }

    public class GetInGameBeastsSucceed : ActionBase
    {
        public List<Obj.Beast> InGameBeasts { get; }

        public GetInGameBeastsSucceed(List<Obj.Beast> inGameBeasts)
        {
            InGameBeasts = inGameBeasts;
        }
    }

    public class GetNftBeastsSucceed : ActionBase { }
}