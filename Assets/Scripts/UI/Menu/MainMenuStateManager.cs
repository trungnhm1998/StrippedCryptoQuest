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

        private void OnEnable()
        {
            _input.ShowMainMenuEvent += OpenMainMenu;
            _input.MenuCancelEvent += CloseMainMenuUsingBack;
        }

        private void OnDisable()
        {
            _input.MenuCancelEvent -= CloseMainMenuUsingBack;
            _input.ShowMainMenuEvent -= OpenMainMenu;
            _input.CloseMainMenuEvent -= CloseMainMenu;
            _forceCloseMainMenuEvent.EventRaised -= CloseMainMenu;
        }

        private void CloseMainMenuUsingBack()
        {
            if (!_uiMainMenuPanel.IsNavigating) return; // only allow back to close menu when in navigating state
            CloseMainMenu();
        }

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