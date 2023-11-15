using CryptoQuest.Gameplay;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CryptoQuest.System.Cheat.States
{
    public class OpenBase : TerminalStateBase
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            
            // TODO: refactor this cache and Manager.Input.DisableAllInput(); method
            // so user can't operate while terminal opening
            
            Manager.OnCommandNavigatePressed = NavigateCommand;
            Manager.OnCloseTerminalPressed = () => CloseTerminal(animator);
            Manager.OnEnterPressed = () => Manager.Terminal.EnterPressed();
            Manager.OnTabPressed = () => Manager.Terminal.TabPressed();
        }

        private void NavigateCommand(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            var readValue = (int)context.ReadValue<float>();
            if (readValue == 1)
            {
                Manager.Terminal.NextCommand();
            }
            else if (readValue == -1)
            {
                Manager.Terminal.PreviousCommand();
            }
        }

        private void CloseTerminal(Animator animator)
        {
            animator.Play("Close");

            switch (Manager.GameState.CurrentGameState)
            {
                case EGameState.Battle:
                    Manager.BattleInput.EnableBattleInput();
                    break;
                case EGameState.Dialogue:
                    Manager.Input.EnableDialogueInput();
                    break;
                case EGameState.Menu:
                    Manager.Input.EnableMenuInput();
                    break;
                default:
                    Manager.Input.EnableMapGameplayInput();
                    break;
            }
        }
    }
}