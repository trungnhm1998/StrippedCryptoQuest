using UnityEngine;

namespace CryptoQuest.System.Cheat.States
{
    public class OpenSmall : OpenBase
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            Manager.Terminal.OpenSmall();
        }
    }
}