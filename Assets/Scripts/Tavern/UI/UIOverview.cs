using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CryptoQuest.Tavern.UI
{
    public class UIOverview : MonoBehaviour
    {
        public event UnityAction CharacterReplacementButtonPressedEvent;
        public event UnityAction PartyOrganizationButtonPressedEvent;

        [SerializeField] private Button _defaultSelectButton;
        [SerializeField] private Button[] _buttons;

        private void OnEnable()
        {
            SetButtonInteractable(false);
        }

        private void SetButtonInteractable(bool isEnable)
        {
            foreach (var button in _buttons)
            {
                button.enabled = isEnable;
            }
        }

        public void EnableOverviewButtons()
        {
            SetButtonInteractable(true);
            Invoke(nameof(SelectDefaultButton), .2f);
        }

        private void SelectDefaultButton() => _defaultSelectButton.Select();

        public void CharacterReplacementButtonPressed()
        {
            CharacterReplacementButtonPressedEvent?.Invoke();
        }

        public void PartyOrganizationButtonPressed()
        {
            PartyOrganizationButtonPressedEvent?.Invoke();
        }
    }
}