using UnityEngine;

namespace CryptoQuest.System.Cheat.States
{
    public class CloseState : TerminalStateBase
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            Manager.Terminal.Close();
            Manager.OnOpenTerminalPressed = OpenTerminal;
            Manager.EnableLastEnabledActionMap();
            Manager.OnCommandNavigatePressed = null;
            Manager.OnCloseTerminalPressed = null;
            Manager.OnEnterPressed = null;
            Manager.OnTabPressed = null;
        }

        private void OpenTerminal()
        {
            var openFull = Anim.GetBool(CheatManager.FullSize);
            Anim.Play(openFull ? "OpenFull" : "OpenSmall");
        }
    }
}