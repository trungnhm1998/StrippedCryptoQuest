using UnityEngine;

namespace CryptoQuest.UI.Menu
{
    public class UIMainMenu : MonoBehaviour
    {
        [SerializeField] private TabManager _tabNavigation;

        private void OnEnable()
        {
            _tabNavigation.SelectFirstTab(); // This called first which the event hasn't sub yet
            _tabNavigation.TabChanged += DisableTabNavigation;
        }

        private void OnDisable()
        {
            _tabNavigation.TabChanged -= DisableTabNavigation;
        }

        private bool _interactingWithSubMenu = false;

        private void DisableTabNavigation(UITabButton uiTabButton)
        {
            foreach (var tab in _tabNavigation.Tabs)
            {
                tab.Interactable = false;
                tab.GetComponent<UITabFocus>().UnFocus();
            }

            uiTabButton.GetComponent<UITabFocus>().Focus();
            _interactingWithSubMenu = true;
        }
    }
}