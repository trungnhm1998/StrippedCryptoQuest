using CryptoQuest.Events.UI;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.UI.Dialogs
{
    public class TestYesNoDialog : MonoBehaviour
    {
        [SerializeField] private DialogCallbackEventSO _yesButtonPressedEvennt;
        [SerializeField] private DialogCallbackEventSO _noButtonPressedEvent;
        [SerializeField] private YesNoDialogEventChannelSO _yesNoDialogEventChannel;

        private void Awake() {
            _yesButtonPressedEvennt.OnEventRaised += YesButtonPressed;
            _noButtonPressedEvent.OnEventRaised += NoButtonPressed;
        }

        private void OnDestroy() {
            _yesButtonPressedEvennt.OnEventRaised -= YesButtonPressed;
            _noButtonPressedEvent.OnEventRaised -= NoButtonPressed;
        }

        private void Start() {
            _yesNoDialogEventChannel.Show();
        }

        private void YesButtonPressed(UnityAction arg0)
        {
            Debug.Log("Test:: Yes Button Pressed");
        }

        private void NoButtonPressed(UnityAction arg0)
        {
            Debug.Log("Test:: No Button Pressed");
        }
    }
}
