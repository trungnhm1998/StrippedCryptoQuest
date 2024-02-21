using CryptoQuest.Gameplay;
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
            // TODO: refactor this Manager.EnableLastEnabledActionMap(); method 
            // because it cause error when open terminal and use cheat to enable another input 
            Manager.OnCommandNavigatePressed = null;
            Manager.OnCloseTerminalPressed = null;
            Manager.OnEnterPressed = null;
            Manager.OnTabPressed = null;
        }

        private void OpenTerminal()
        {
#if DEVELOPMENT_BUILD || UNITY_EDITOR || ENABLE_CHEAT
            if (Manager.GameState.CurrentGameState is EGameState.Field or EGameState.Battle)
            {
                Manager.Input.DisableAllInput();
                var openFull = Anim.GetBool(CheatManager.FullSize);
                Anim.Play(openFull ? "OpenFull" : "OpenSmall");
            }
#endif
        }
    }
}