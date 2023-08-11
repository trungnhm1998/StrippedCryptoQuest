using CryptoQuest.UI.Battle.MenuStateMachine.States;
using CryptoQuest.PushdownFSM;
using UnityEngine;

namespace CryptoQuest.UI.Battle.MenuStateMachine
{
    public class BattleMenuStateMachine : PushdownStateMachine
    {
        public static readonly string SelectCommandState = "SelectCommand";
        public static readonly string SelectCommandContentState = "SelectCommandContent";
        public static readonly string SelectHeroState = "SelectHero";

        public BattlePanelController BattlePanelController { get; }

        public new BattleMenuStateBase ActiveState { get; set; }

        public BattleMenuStateMachine(BattlePanelController controller) : base(false)
        {
            BattlePanelController = controller;
            
            AddState(SelectCommandState, new SelectCommandState(this));
            AddState(SelectCommandContentState, new SelectCommandContentState(this));
            AddState(SelectHeroState, new SelectHeroState(this));

            SetStartState(SelectCommandState);
            Init();
        }

        public void HandleCancel()
        {
            PushdownState();
        }

    }
}