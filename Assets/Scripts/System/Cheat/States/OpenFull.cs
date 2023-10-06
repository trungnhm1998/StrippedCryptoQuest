using UnityEngine;

namespace CryptoQuest.System.Cheat.States
{
    public class OpenFull : OpenBase
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            Manager.Terminal.OpenFull();
        }
    }
}