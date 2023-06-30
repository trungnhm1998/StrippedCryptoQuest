using CryptoQuest.Input;
using IndiGames.Core.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI
{
    public class UITitle : MonoBehaviour
    {
        [SerializeField] private SceneScriptableObject _sceneToLoad;
        [SerializeField] private InputMediatorSO _inputMediatorSO;
        [SerializeField] private Button _startGameButton;

        [Header("Listen on")]
        [SerializeField] private VoidEventChannelSO _sceneLoaded;

        [Header("Raise on")]
        [SerializeField] private LoadSceneEventChannelSO _loadMapEvent;

        private void OnEnable()
        {
            _sceneLoaded.EventRaised += SceneLoadedEvent_Raised;
        }
        
        private void OnDisable()
        {
            _sceneLoaded.EventRaised -= SceneLoadedEvent_Raised;
        }

        private void SceneLoadedEvent_Raised()
        {
            _inputMediatorSO.EnableMenuInput();
            _startGameButton.Select();
        }

        public void StartGameButtonClicked()
        {
            _loadMapEvent.RequestLoad(_sceneToLoad);
        }
    }
}