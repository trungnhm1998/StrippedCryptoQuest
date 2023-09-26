using UnityEngine;

namespace CryptoQuest.UI.Title.TitleStates
{
    public class SocialLoginState : IState
    {
        protected UISocialPanel _socialPanel;
        private TitlePanelController _titlePanelController;

        protected SocialLoginState(TitlePanelController titlePanelController)
        {
            _titlePanelController = titlePanelController;
            _socialPanel = titlePanelController.SocialPanel;
        }

        public virtual void OnEnter()
        {
            _titlePanelController.Subscribe();
        }

        public virtual void OnExit()
        {
            _titlePanelController.Unsubscribe();
        }
    }
}