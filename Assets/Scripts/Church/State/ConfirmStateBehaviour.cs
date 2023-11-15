using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Church.State
{
    public class ConfirmStateBehaviour : BaseStateBehaviour
    {
        [SerializeField] private LocalizedString _message;
        [SerializeField] private int _pricePerLevel;
        private ChurchStateController _stateController;
        private static readonly int Result = Animator.StringToHash("ResultState");
        private static readonly int SelectCharacter = Animator.StringToHash("SelectCharacterState");
        private float _costToRevive;

        protected override void OnEnter()
        {
            _stateController = StateMachine.GetComponent<ChurchStateController>();
            _stateController.DialogController.Dialogue.SetMessage(_message).Hide();
            SetupDialog();
        }

        protected override void OnExit() { }

        private void YesButtonPressed()
        {
            _stateController.DialogController.ChoiceDialog.Hide();
            _stateController.Presenter.ValidateGoldToRevive(_costToRevive);
            StateMachine.Play(Result);
        }

        private void NoButtonPressed()
        {
            _stateController.DialogController.ChoiceDialog.Hide();
            StateMachine.Play(SelectCharacter);
        }

        private void SetupDialog()
        {
            _costToRevive = _stateController.Presenter.CharacterToRevive.Level * _pricePerLevel;
            _message.Arguments = new object[] { _costToRevive };
            _stateController.DialogController.
            ChoiceDialog
                .SetButtonsEvent(YesButtonPressed, NoButtonPressed)
                .SetMessage(_message)
                .Show();
        }
    }
}
