using CryptoQuest.Gameplay;
using CryptoQuest.Input;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.UI.Menu
{
    /// <summary>
    /// Management show and hide main menu state based on input
    /// </summary>
    public class MainMenuStateManager : MonoBehaviour
    {
        [SerializeField] private VoidEventChannelSO _forceCloseMainMenuEvent;
        [SerializeField] private InputMediatorSO _input;
        [SerializeField] private GameStateSO _gameStateSO;
        [SerializeField] private UIMainMenu _uiMainMenuPanel;
        [SerializeField] private TabManager _tabManager;

        private void OnEnable()
        {
            _input.ShowMainMenuEvent += OpenMainMenu;
            UIMainMenu.BackToNavigation += EnableBackToCloseMenu;
            _tabManager.OpeningTab += DisableBackToCloseMenu;
            _input.MenuCancelEvent += CloseMainMenuUsingBack;
        }

        private void OnDisable()
        {
            UIMainMenu.BackToNavigation -= EnableBackToCloseMenu;
            _tabManager.OpeningTab -= DisableBackToCloseMenu;
            _input.MenuCancelEvent -= CloseMainMenuUsingBack;
            _input.ShowMainMenuEvent -= OpenMainMenu;
            _input.CloseMainMenuEvent -= CloseMainMenu;
            _forceCloseMainMenuEvent.EventRaised -= CloseMainMenu;
        }

        private void CloseMainMenuUsingBack()
        {
            if (!_enableBackToCloseMenu) return;
            CloseMainMenu();
        }

        private bool _enableBackToCloseMenu = false;
        private void EnableBackToCloseMenu() => _enableBackToCloseMenu = true;

        private void DisableBackToCloseMenu(UITabButton _) => _enableBackToCloseMenu = false;

        private void OpenMainMenu()
        {
            _input.CloseMainMenuEvent += CloseMainMenu;
            _forceCloseMainMenuEvent.EventRaised += CloseMainMenu;
            _uiMainMenuPanel.gameObject.SetActive(true);
            _input.EnableInputMap("Menus");
            _gameStateSO.UpdateGameState(EGameState.Menu);
        }

        private void CloseMainMenu()
        {
            _input.CloseMainMenuEvent -= CloseMainMenu;
            _forceCloseMainMenuEvent.EventRaised -= CloseMainMenu;
            _uiMainMenuPanel.gameObject.SetActive(false);
            _input.EnableInputMap("MapGameplay");
            _gameStateSO.UpdateGameState(EGameState.Field);
        }
    }
}