using UnityEngine;

namespace CryptoQuest.BlackSmith.Upgrade.StateMachine
{
    public class SelectEquipmentStateBehaviour : StateMachineBehaviour
    {
        private Animator _animator;
        private UpgradeStateController _stateController;
        private static readonly int _submit = Animator.StringToHash("isUpgrade");

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _stateController = animator.GetComponent<UpgradeStateController>();
            _animator = animator;
            _stateController.SelectedEquipmentPanel.SetActive(true);
            _stateController.InstantiateEquipment();
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _stateController.SelectedEquipmentPanel.SetActive(false);
        }

        private void GoToUpgradeState()
        {
            _animator.SetTrigger(_submit);
        }
    }
}