using CryptoQuest.Merchant;
using CryptoQuest.Tavern.UI;
using UnityEngine;

namespace CryptoQuest.Tavern.States
{
    public class PartyOrganizeState : StateMachineBehaviourBase
    {
        [SerializeField] private MerchantInput _merchantInput;

        private static readonly int OverviewState = Animator.StringToHash("Overview");

        private PartyOrganizePanel _partyOrganizePanel;

        protected override void OnEnter()
        {
            _partyOrganizePanel ??= StateMachine.GetComponent<PartyOrganizePanel>();
            _partyOrganizePanel.enabled = true;

            _merchantInput.CancelEvent += BackToOverview;
        }

        private void BackToOverview()
        {
            _partyOrganizePanel.enabled = false;
            StateMachine.Play(OverviewState);
        }

        protected override void OnExit()
        {
            _merchantInput.CancelEvent -= BackToOverview;
        }
    }
}