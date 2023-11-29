using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CryptoQuest.Menus.Home.UI
{
    public class UIOverview : MonoBehaviour
    {
        public event UnityAction ChangeOrderEvent;
        public event UnityAction ViewCharacterListEvent;

        [SerializeField] private Button[] _overviewTabs;

        public void EnableSelectActions()
        {
            SetButtonsActive(true);
            _overviewTabs[0].Select();
        }

        public void ChangOrderButtonPressed()
        {
            SetButtonsActive(false);
            ChangeOrderEvent?.Invoke();
        }

        public void CharacterListButtonPressed()
        {
            SetButtonsActive(false);
            ViewCharacterListEvent?.Invoke();
        }

        private void SetButtonsActive(bool active)
        {
            foreach (var button in _overviewTabs)
            {
                button.enabled = active;
            }
        }
    }
}