using CryptoQuest.Menu;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.BlackSmith.Evolve.UI
{
    public class UIResultPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _resultText;
        [SerializeField] private MultiInputButton _defaultButton;
        [SerializeField] private LocalizedString _successMessage;
        [SerializeField] private LocalizedString _failMessage;

        private void OnEnable()
        {
            _defaultButton.Select();
        }

        public void UpdateUIFail()
        {
            _resultText.text = _failMessage.GetLocalizedString();
        }

        public void UpdateUISuccess()
        {
            _resultText.text = _successMessage.GetLocalizedString();
        }
    }
}
