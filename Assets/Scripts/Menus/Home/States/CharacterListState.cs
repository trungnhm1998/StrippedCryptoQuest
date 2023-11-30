using CryptoQuest.Character.Sagas;
using FSM;
using CryptoQuest.Menus.Home.UI;
using IndiGames.Core.Events;
using TinyMessenger;

namespace CryptoQuest.Menus.Home.States
{
    public class CharacterListState : StateBase
    {
        private UIHomeMenu _homePanel;
        private TinyMessageSubscriptionToken _fetchHeroesSucceeded;

        public CharacterListState(UIHomeMenu panel) : base(false) => _homePanel = panel;

        public override void OnEnter()
        {
            _fetchHeroesSucceeded = ActionDispatcher.Bind<FetchInGameHeroesSucceeded>(PassDataToUi);
            _homePanel.Input.MenuCancelEvent += HandleCancel;
            _homePanel.UICharacterList.gameObject.SetActive(true);
            
            ActionDispatcher.Dispatch(new GetInGameHeroes());
        }

        public override void OnExit()
        {
            ActionDispatcher.Unbind(_fetchHeroesSucceeded);
            _homePanel.Input.MenuCancelEvent -= HandleCancel;
            _homePanel.UICharacterList.gameObject.SetActive(false);
        }

        private void PassDataToUi(FetchInGameHeroesSucceeded ctx)
        {
            _homePanel.UICharacterList.SetData(ctx.InGameHeroes);
        }

        private void HandleCancel()
        {
            fsm.RequestStateChange(HomeMenuStateMachine.Overview);
        }
    }
}