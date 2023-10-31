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
        [SerializeField] private UIMainMenu _uiMainMenuPanel;

        private void OnEnable()
        {
            _input.ShowMainMenuEvent += OpenMainMenu;
        }

        private void OnDisable()
        {
            _input.ShowMainMenuEvent -= OpenMainMenu;
            _input.CloseMainMenuEvent -= CloseMainMenu;
            _forceCloseMainMenuEvent.EventRaised -= CloseMainMenu;
        }

        private void OpenMainMenu()
        {
            _input.CloseMainMenuEvent += CloseMainMenu;
            _forceCloseMainMenuEvent.EventRaised += CloseMainMenu;
            _uiMainMenuPanel.gameObject.SetActive(true);
            _input.EnableInputMap("Menus");
        }

        private void CloseMainMenu()
        {
            _input.CloseMainMenuEvent -= CloseMainMenu;
            _forceCloseMainMenuEvent.EventRaised -= CloseMainMenu;
            _uiMainMenuPanel.gameObject.SetActive(false);
            _input.EnableInputMap("MapGameplay");
        }
    }
}