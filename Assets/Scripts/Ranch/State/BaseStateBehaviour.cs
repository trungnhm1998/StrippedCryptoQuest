using CryptoQuest.Input;
using CryptoQuest.Ranch.Upgrade.UI;
using CryptoQuest.UI.Dialogs.Dialogue;
using UnityEngine;
using UnityEngine.Animations;

namespace CryptoQuest.Ranch.State
{
    public abstract class BaseStateBehaviour : StateMachineBehaviour
    {
        private bool _hasEntered = false;
        protected Animator StateMachine { get; private set; }

        protected static readonly int OverviewState = Animator.StringToHash("OverviewState");

        protected static readonly int SwapState = Animator.StringToHash("SwapState");
        protected static readonly int SwapConfirmState = Animator.StringToHash("ConfirmState");

        protected static readonly int UpgradeState = Animator.StringToHash("UpgradeState");
        protected static readonly int SelectLevelState = Animator.StringToHash("SelectLevelState");
        protected static readonly int UpgradeResultState = Animator.StringToHash("UpgradeResultState");

        protected static readonly int SelectMaterialState = Animator.StringToHash("EvolveSelectMaterialState");
        protected static readonly int EvolveState = Animator.StringToHash("EvolveState");
        protected static readonly int ResultState = Animator.StringToHash("EvolveResultState");
        protected static readonly int EvolveConfirmState = Animator.StringToHash("EvolveConfirmState");


        protected RanchStateController _stateController;
        protected MerchantsInputManager _input;
        protected UIDialogueForGenericMerchant _dialogue;
        protected UIConfigBeastUpgradePresenter _config;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _hasEntered = true;
            StateMachine = animator;

            _stateController = StateMachine.GetComponent<RanchStateController>();
            _config = _stateController.UIConfig;
            _input = _stateController.Controller.Input;
            _dialogue = _stateController.DialogController.NormalDialogue;

            OnEnter();
        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            _hasEntered = true;
            StateMachine = animator;
            OnEnter();
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) => OnExit();

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller) => OnExit();

        public void Exit()
        {
            if (!_hasEntered) return;
            OnExit();
        }

        protected abstract void OnEnter();
        protected abstract void OnExit();
    }
}