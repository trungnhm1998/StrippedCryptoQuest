using CryptoQuest.Tavern.UI;
using UnityEngine;

namespace CryptoQuest.Tavern.States
{
    public class SelectCharacterState : StateMachineBehaviour
    {
        private Animator _animator;
        private TavernPresenter _presenter;

        private static readonly int TurnBack = Animator.StringToHash("isTurnBack");


        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            _animator = animator;

            _presenter = animator.GetComponent<TavernPresenter>();
            _presenter.UICharacterReplacement.gameObject.SetActive(true);
            
            _presenter.TavernInputManager.CancelEvent += CancelCharacterReplacement;

        }

        private void CancelCharacterReplacement()
        {
            _presenter.UICharacterReplacement.gameObject.SetActive(false);
            _animator.SetTrigger(TurnBack);

        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            _presenter.TavernInputManager.CancelEvent -= CancelCharacterReplacement;
        }
    }
}