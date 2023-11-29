using CryptoQuest.Menus.Beast.UI;
using FSM;

namespace CryptoQuest.Menus.Beast.States
{
    public class BeastMenuStateMachine : StateMachine
    {
        public static readonly string BeastSelection = "BeastSelection";

        public BeastMenuStateMachine(UIBeastMenu panel) : base(panel)
        {
            AddState(BeastSelection, new BeastOverviewState(panel));

            SetStartState(BeastSelection);
        }
    }
}