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
        private bool _isAlive;

        protected override void OnEnter()
        {
            _stateController = StateMachine.GetComponent<ChurchStateController>();
            _stateController.Input.SubmitEvent += ChangeState;
            _stateController.Input.CancelEvent += ChangeState;
            ValidateToRevive();
        }

        protected override void OnExit()
        {
            _stateController.Input.SubmitEvent -= ChangeState;
            _stateController.Input.CancelEvent -= ChangeState;
        }

        private void ChangeState()
        {
            StateMachine.Play(_isAlive || !_isEnoughGold ? SelectCharacter : ContinueToRevive);
        }

        private void ValidateToRevive()
        {
            UpdateStatus();
            SetMessage();
        }

        private void UpdateStatus()
        {
            _isEnoughGold = _stateController.Presenter.IsEnoughGold;
            _isAlive = _stateController.Presenter.IsAlive;
        }

        private void SetMessage()
        {
            LocalizedString message = _isEnoughGold ? GetMessageForEnoughGold() : _notEnoughGoldMessage;
            _stateController.DialogController.Dialogue.SetMessage(message).Show();
        }

        private LocalizedString GetMessageForEnoughGold()
        {
            return _isAlive ? _reviveFailMessage : _reviveSuccessMessage;
        }
    }
}
