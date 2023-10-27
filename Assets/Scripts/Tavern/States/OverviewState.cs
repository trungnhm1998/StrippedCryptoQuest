using UnityEngine;

namespace CryptoQuest.Tavern.States
{
    public class OverviewState : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            var presenter = animator.GetComponent<TavernOverviewPresenter>();
            presenter.TavernUiOverview.gameObject.SetActive(true);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex) { }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex) { }

        public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex) { }

        public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex) { }
    }
}