using CryptoQuest.Ranch.UI;
using CryptoQuest.Sagas.Objects;
using IndiGames.Core.Events;

namespace CryptoQuest.Ranch
{
    public class FetchProfileBeastsAction : ActionBase
    {
    }

    public class BeastInventoryFilled : ActionBase
    {
    }

    public class GetBeastsFailed : ActionBase
    {
    }

    public class GetBeastSucceeded : ActionBase
    {
    }

    public class FetchInboxBeastSucceeded : ActionBase
    {
        public BeastResponse[] InBoxBeasts { get; }

        public FetchInboxBeastSucceeded(BeastResponse[] inBoxBeasts)
        {
            InBoxBeasts = inBoxBeasts;
        }
    }

    public class FetchInGameBeastSucceeded : ActionBase
    {
        public BeastResponse[] InGameBeasts { get; }

        public FetchInGameBeastSucceeded(BeastResponse[] inGameBeasts)
        {
            InGameBeasts = inGameBeasts;
        }
    }

    public class SendBeastsToBothSide : ActionBase
    {
        public UIBeastItem[] SelectedInDimensionBoxBeasts { get; }
        public UIBeastItem[] SelectedInGameBeasts { get; }

        public SendBeastsToBothSide(UIBeastItem[] selectedInGameBeasts, UIBeastItem[] selectedInDimensionBoxBeasts)
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

    public class TransferFailed : ActionBase
    {
    }
}