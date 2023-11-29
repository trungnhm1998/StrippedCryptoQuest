using FSM;
using CryptoQuest.Menus.Home.UI;

namespace CryptoQuest.Menus.Home.States
{
    public class CharacterListState : StateBase
    {
        private UIHomeMenu _homePanel;
        public CharacterListState(UIHomeMenu panel) : base(false) => _homePanel = panel;

        public override void OnEnter()
        {
            _homePanel.Input.MenuCancelEvent += HandleCancel;
            _homePanel.UICharacterList.gameObject.SetActive(true);
        }

        public override void OnExit()
        {
            _homePanel.Input.MenuCancelEvent -= HandleCancel;
            _homePanel.UICharacterList.gameObject.SetActive(false);
        }

        private void HandleCancel()
        {
            fsm.RequestStateChange(HomeMenuStateMachine.Overview);
        }
    }
}