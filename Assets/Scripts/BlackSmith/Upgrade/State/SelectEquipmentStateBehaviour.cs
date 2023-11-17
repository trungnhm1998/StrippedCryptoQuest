using CryptoQuest.Input;
using Input;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.BlackSmith.Upgrade.State
{
    public class SelectEquipmentStateBehaviour : StateMachineBehaviour
    {
        [SerializeField] private LocalizedString _selectTargetMessage;
        private Animator _animator;
        private MerchantsInputManager _input;
        private UpgradeStateController _stateController;
        private static readonly int _submit = Animator.StringToHash("isUpgrade");
        private static readonly int _exit = Animator.StringToHash("isBlackSmithState");

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _animator = animator;
            _stateController = animator.GetComponent<UpgradeStateController>();
            _input = _stateController.InputManager;
            _stateController.SelectedEquipmentPanel.SetActive(true);
            _stateController.SelectActionPanel.SetActive(false);
            _stateController.UpgradePresenter.Init();
            _input.SubmitEvent += GoToUpgradeState;
            _input.CancelEvent += ExitState;
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _stateController.SelectedEquipmentPanel.SetActive(false);
            _input.SubmitEvent -= GoToUpgradeState;
            _input.CancelEvent -= ExitState;
        }

        private void GoToUpgradeState()
        {
            _animator.SetTrigger(_submit);
        }

        private void ExitState()
        {
            _stateController.UIBlackSmith.BlackSmithOpened();
            _animator.SetTrigger(_exit);
            _stateController.DialogsPresenter.Dialogue.SetMessage(_selectTargetMessage).Show();
        }
    }
}