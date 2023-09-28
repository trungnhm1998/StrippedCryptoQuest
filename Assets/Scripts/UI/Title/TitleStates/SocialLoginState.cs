using UnityEngine;

namespace CryptoQuest.UI.Title.TitleStates
{
    public class SocialLoginState : IState
    {
        protected UISocialPanel _socialPanel;
        protected TitlePanelController _titlePanelController;

        protected SocialLoginState(TitlePanelController titlePanelController)
        {
            _titlePanelController = titlePanelController;
            _socialPanel = titlePanelController.SocialPanel;
        }

        public virtual void OnEnter() { }

        public virtual void OnExit() { }
    }
}