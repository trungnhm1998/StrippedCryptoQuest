using CryptoQuest.Character.MonoBehaviours;
using CryptoQuest.Gameplay;
using CryptoQuest.System;
using IndiGames.Core.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using IndiGames.Core.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using IndiGames.Core.EditorTools;
#endif

namespace CryptoQuest.Map.CheckPoint
{
    public interface ICheckPointController
    {
        bool IsBackToCheckPoint { get; }
        Vector3 CheckPointPosition { get; }
        CharacterBehaviour.EFacingDirection FacingDirection { get; }
        void SaveCheckPoint(Vector3 position, int facingDirection);
        void BackToCheckPoint();
        void FinishBackToCheckPoint();
    }
    public class CheckPointController : MonoBehaviour, ICheckPointController
    {
        public bool IsBackToCheckPoint => _isBackToCheckPoint;
        public Vector3 CheckPointPosition => _lastCheckPointPosition;
        public CharacterBehaviour.EFacingDirection FacingDirection => GetFacingDirection();

        private SceneScriptableObject _lastCheckPointScene;
        private string _lastSceneCheckPointName;
        private SceneScriptableObject _currentScene;
        private Vector3 _lastCheckPointPosition;
        private int _lastCheckPointFacingDirection;
        private bool _isBackToCheckPoint = false;

        [SerializeField] private VoidEventChannelSO _showCheckPointMessageSO;
        [SerializeField] private LoadSceneEventChannelSO _loadSceneEvent;
        [SerializeField] private SceneScriptableObject _defaultCheckpoint;
        [SerializeField] private GameplayBus _gameplayBus;
        [SerializeField] private FadeConfigSO _fadeController;

        private void Awake()
        {
            ServiceProvider.Provide<ICheckPointController>(this);
            _loadSceneEvent.LoadingRequested += SaveNewLoadedSceneForCheckpoint;
        }
        private void OnDestroy()
        {
            _loadSceneEvent.LoadingRequested -= SaveNewLoadedSceneForCheckpoint;
        }

        private void Start()
        {
#if UNITY_EDITOR
            var editorColdBoot = FindObjectOfType<EditorColdBoot>();
            _currentScene = editorColdBoot.ThisScene;
#endif
            _lastCheckPointScene = _defaultCheckpoint;
            _lastCheckPointPosition = Vector3.zero;
            _lastCheckPointFacingDirection = 1;
        }

        private void SaveNewLoadedSceneForCheckpoint(SceneScriptableObject nextScene)
        {
            _currentScene = nextScene;
            Debug.Log("Checkpoint: Set current scene " + nextScene.name);
        }

        public void SaveCheckPoint(Vector3 position, int facingDirection)
        {
            _lastSceneCheckPointName = SceneManager.GetActiveScene().name;
            _lastCheckPointScene = _currentScene;
            _lastCheckPointPosition = position;
            _lastCheckPointFacingDirection = facingDirection;
            Debug.Log($"Checkpoint: Save checkpoint at {_lastCheckPointScene.name} at {position.ToString()}");
        }

        public void EnableCheckPoint()
        {
            _isBackToCheckPoint = true;
        }

        public void FinishBackToCheckPoint()
        {
            _isBackToCheckPoint = false;
        }

        public CharacterBehaviour.EFacingDirection GetFacingDirection()
        {
            return (CharacterBehaviour.EFacingDirection)_lastCheckPointFacingDirection;
        }

        public void BackToCheckPoint()
        {
            EnableCheckPoint();
            if (SceneManager.GetSceneByName(_lastSceneCheckPointName).isLoaded)
            {
                _gameplayBus.Hero.transform.position = _lastCheckPointPosition;
                _gameplayBus.Hero.SetFacingDirection(FacingDirection);
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(_lastSceneCheckPointName));
                _showCheckPointMessageSO.RaiseEvent();
                _fadeController.OnFadeOut();
            }
            else
            {
                _loadSceneEvent.RequestLoad(_lastCheckPointScene);
            }    
        }
    }
}
