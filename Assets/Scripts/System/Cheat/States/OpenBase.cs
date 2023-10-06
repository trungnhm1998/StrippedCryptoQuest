using UnityEngine;
using UnityEngine.InputSystem;

namespace CryptoQuest.System.Cheat.States
{
    public class OpenBase : TerminalStateBase
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            Manager.CacheLastEnabledActionMap();
            Manager.Input.DisableAllInput();
            Manager.OnCommandNavigatePressed = NavigateCommand;
            Manager.OnCloseTerminalPressed = () => animator.Play("Close");
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
    }
}