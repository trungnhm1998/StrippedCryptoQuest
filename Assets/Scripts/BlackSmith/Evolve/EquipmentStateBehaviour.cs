using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.BlackSmith.EvolveStates
{
    public class EquipmentStateBehaviour : StateMachineBehaviour
    {
        [SerializeField] private LocalizedString _selectTargetMessage;

        private Animator _animator;
        private EvolveStateController _stateController;
        private static readonly int ConfirmState = Animator.StringToHash("isConfirm");

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _animator = animator;

            _stateController = animator.GetComponent<EvolveStateController>();
            _stateController.EvolvePanel.EnterConfirmPhaseEvent += GoToConfirmState;
            _stateController.EvolvePanel.HadMethodRunned = false; // Code smell

            GetDataFromPresenterAndPassToScrollView();
            SetUpDialogs();
        }

        private void GoToConfirmState()
        {
            _animator.SetTrigger(ConfirmState);
        }

        private void SetUpDialogs()
        {
            _stateController.DialogsPresenter.Dialogue
                .SetMessage(_selectTargetMessage)
                .Show();
        }

        private void GetDataFromPresenterAndPassToScrollView()
        {
            var data = _stateController.EvolvePanel.GameData;
            _stateController.EvolveEquipmentList.RenderEquipments(data);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _stateController.EvolvePanel.EnterConfirmPhaseEvent -= GoToConfirmState;
        }
    }
}