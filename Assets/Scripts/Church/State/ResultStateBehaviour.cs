using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Church.State
{
    public class ResultStateBehaviour : BaseStateBehaviour
    {
        [SerializeField] private LocalizedString _reviveSuccessMessage;
        [SerializeField] private LocalizedString _reviveFailMessage;
        private ChurchStateController _stateController;
        private static readonly int SelectCharacter = Animator.StringToHash("SelectCharacterState");

        protected override void OnEnter()
        {
            _stateController = StateMachine.GetComponent<ChurchStateController>();
            _stateController.Input.SubmitEvent += ChangeState;
            _stateController.Presenter.IsReviveSuccessEvent += SetMessage;
        }

        protected override void OnExit()
        {
            _stateController.Input.SubmitEvent -= ChangeState;
            _stateController.Presenter.IsReviveSuccessEvent -= SetMessage;
        }

        private void ChangeState()
        {
            StateMachine.Play(SelectCharacter);
        }

        private void SetMessage(bool isSuccess)
        {
            LocalizedString message = isSuccess ? _reviveSuccessMessage : _reviveFailMessage;
            _stateController.DialogController.Dialogue.SetMessage(message).Show();
        }
    }
}
