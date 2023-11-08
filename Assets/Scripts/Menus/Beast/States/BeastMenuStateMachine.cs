using CryptoQuest.Menus.Beast.UI;
using FSM;

namespace CryptoQuest.Menus.Beast.States
{
    public class BeastMenuStateMachine : StateMachine
    {
        public static readonly string BeastOverview = "BeastOverview";

        private readonly UIBeastMenu _beastMenuPanel;

        public BeastMenuStateMachine(UIBeastMenu beastMenuPanel) : base(false)
        {
            _beastMenuPanel = beastMenuPanel;

            AddState(BeastOverview, new BeastOverviewState(beastMenuPanel));

            SetStartState(BeastOverview);
        }
    }
}
