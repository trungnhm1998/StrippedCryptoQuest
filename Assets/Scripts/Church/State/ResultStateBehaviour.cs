using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Church.State
{
    public class ResultStateBehaviour : BaseStateBehaviour
    {
        [SerializeField] private LocalizedString _reviveSuccessMessage;
        [SerializeField] private LocalizedString _reviveFailMessage;
        [SerializeField] private LocalizedString _notEnoughGoldMessage;
        private ChurchStateController _stateController;
        private static readonly int SelectCharacter = Animator.StringToHash("SelectCharacterState");
        private static readonly int ContinueToRevive = Animator.StringToHash("ContinueReviveState");
        private bool _isEnoughGold;
        private bool _reviveSuccess;

        protected override void OnEnter()
        {
            _stateController = StateMachine.GetComponent<ChurchStateController>();
            _stateController.Input.SubmitEvent += ChangeState;
            ValidateToRevive();
        }

        protected override void OnExit()
        {
            _stateController.Input.SubmitEvent -= ChangeState;
        }

        private void ChangeState()
        {
            StateMachine.Play(_reviveSuccess ? SelectCharacter : ContinueToRevive);
        }

        private void ValidateToRevive()
        {
            UpdateStatus();
            SetMessage();
        }

        private void UpdateStatus()
        {
            _isEnoughGold = _stateController.Presenter.IsEnoughGold;
            _reviveSuccess = !_stateController.Presenter.IsAlive;
        }

        private void SetMessage()
        {
            LocalizedString message = _isEnoughGold ? GetMessageForEnoughGold() : _notEnoughGoldMessage;
            _stateController.DialogController.Dialogue.SetMessage(message).Show();
        }

        private LocalizedString GetMessageForEnoughGold()
        {
            return _reviveSuccess ? _reviveFailMessage : _reviveSuccessMessage;
        }
    }
}
