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

        private void OnEnable()
        {
            _defaultSelectButton.Select();
        }

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