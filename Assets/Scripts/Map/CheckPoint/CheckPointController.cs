using CryptoQuest.Character.MonoBehaviours;
using CryptoQuest.Gameplay;
using CryptoQuest.System;
using IndiGames.Core.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using IndiGames.Core.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using IndiGames.Core.SaveSystem;
using System;
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

    public class CheckPointController : MonoBehaviour, ICheckPointController, ISaveObject
    {
        public bool IsBackToCheckPoint => _isBackToCheckPoint;
        public Vector3 CheckPointPosition => _lastCheckPointPosition;
        public CharacterBehaviour.EFacingDirection FacingDirection => GetFacingDirection();

        [SerializeField] private SceneScriptableObject _lastCheckPointScene;
        [SerializeField] private string _lastSceneCheckPointName;
        [SerializeField] private SceneScriptableObject _currentScene;
        [SerializeField] private Vector3 _lastCheckPointPosition;
        [SerializeField] private int _lastCheckPointFacingDirection;
        [SerializeField] private bool _isBackToCheckPoint = false;

        [SerializeField] private VoidEventChannelSO _showCheckPointMessageSO;
        [SerializeField] private LoadSceneEventChannelSO _loadSceneEvent;
        [SerializeField] private SceneScriptableObject _defaultCheckpoint;
        [SerializeField] private GameplayBus _gameplayBus;
        [SerializeField] private FadeConfigSO _fadeController;

        private ISaveSystem _saveSystem;

        private void Awake()
        {
            ServiceProvider.Provide<ICheckPointController>(this);
            _loadSceneEvent.LoadingRequested += SaveNewLoadedSceneForCheckpoint;
            _saveSystem = ServiceProvider.GetService<ISaveSystem>();
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
            _saveSystem?.LoadObject(this);
        }

        private void SaveNewLoadedSceneForCheckpoint(SceneScriptableObject nextScene)
        {
            _currentScene = nextScene;
            _saveSystem.SaveObject(this);
            Debug.Log("Checkpoint: Set current scene " + nextScene.name);
        }

        public void SaveCheckPoint(Vector3 position, int facingDirection)
        {
            _lastSceneCheckPointName = SceneManager.GetActiveScene().name;
            _lastCheckPointScene = _currentScene;
            _lastCheckPointPosition = position;
            _lastCheckPointFacingDirection = facingDirection;
            _saveSystem.SaveObject(this);
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
                EnableCheckPoint();
                _loadSceneEvent.RequestLoad(_lastCheckPointScene);
            }    
        }

        #region SaveLoad
        public string Key => "CheckPoint";

        public string ToJson()
        {
            return JsonUtility.ToJson(this);
        }

        public bool FromJson(string json)
        {
            try
            {
                JsonUtility.FromJsonOverwrite(json, this);
                return true;
            }
            catch (Exception ex) { Debug.LogException(ex); }
            return false;
        }
        #endregion
    }
}
