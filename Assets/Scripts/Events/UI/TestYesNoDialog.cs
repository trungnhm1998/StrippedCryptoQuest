using CryptoQuest.Events.UI;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.UI.Dialogs
{
    public class TestYesNoDialog : MonoBehaviour
    {
        [SerializeField] private YesNoDialogEventChannelSO _yesNoDialogEventChannel;

        private void Start() {
            _yesNoDialogEventChannel.Show(YesButtonPressed, NoButtonPressed);
        }

        private void YesButtonPressed()
        {
            Debug.Log("Test:: Yes Button Pressed");
        }

        private void NoButtonPressed()
        {
            Debug.Log("Test:: No Button Pressed");
        }
    }
}
