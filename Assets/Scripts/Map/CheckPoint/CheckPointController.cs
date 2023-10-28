using CryptoQuest.Character.MonoBehaviours;
using CryptoQuest.Gameplay;
using CryptoQuest.System;
using IndiGames.Core.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using IndiGames.Core.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
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

    [Serializable]
    class CheckPointData
    {
        public string LastSceneGuid;
        public string LastSceneName;
        public Vector3 LastPosition;
        public int LastDirection;
        public string CurrentSceneGuid;
    }

    public class CheckPointController : SaveObject, ICheckPointController
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

        private void Awake()
        {
            ServiceProvider.Provide<ICheckPointController>(this);
            _lastCheckPointScene = _defaultCheckpoint;
            _lastCheckPointPosition = Vector3.zero;
            _lastCheckPointFacingDirection = 1;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _loadSceneEvent.LoadingRequested += SaveNewLoadedSceneForCheckpoint;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _loadSceneEvent.LoadingRequested -= SaveNewLoadedSceneForCheckpoint;
        }

        private void Start()
        {
#if UNITY_EDITOR
            var editorColdBoot = FindObjectOfType<EditorColdBoot>();
            _currentScene = editorColdBoot.ThisScene;
#endif
        }

        private void SaveNewLoadedSceneForCheckpoint(SceneScriptableObject nextScene)
        {
            _currentScene = nextScene;
            SaveSystem?.SaveObject(this);
            Debug.Log("Checkpoint: Set current scene " + nextScene.name);
        }

        public void SaveCheckPoint(Vector3 position, int facingDirection)
        {
            var scene = SceneManager.GetActiveScene();
            _lastSceneCheckPointName = scene != null ? scene.name : "NULL";
            _lastCheckPointScene = _currentScene;
            _lastCheckPointPosition = position;
            _lastCheckPointFacingDirection = facingDirection;
            SaveSystem?.SaveObject(this);
            Debug.Log($"Checkpoint: Save checkpoint at {_lastSceneCheckPointName} at {position.ToString()}");
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
        public override string Key => "CheckPoint";

        public override string ToJson()
        {
            var checkPointData = new CheckPointData()
            {
                LastSceneGuid = _lastCheckPointScene.Guid,
                LastSceneName = _lastSceneCheckPointName,
                LastPosition = _lastCheckPointPosition,
                LastDirection = _lastCheckPointFacingDirection,
                CurrentSceneGuid = _currentScene.Guid
            };
            return JsonUtility.ToJson(checkPointData);
        }

        public override IEnumerator CoFromJson(string json)
        {
            if (!string.IsNullOrEmpty(json))
            {
                var checkPointData = new CheckPointData();
                JsonUtility.FromJsonOverwrite(json, checkPointData);
                var lastSoHandle = Addressables.LoadAssetAsync<SceneScriptableObject>(checkPointData.LastSceneGuid);
                yield return lastSoHandle;
                if (lastSoHandle.Status == AsyncOperationStatus.Succeeded)
                {
                   _lastCheckPointScene = lastSoHandle.Result;
                }
                var currentSoHandle = Addressables.LoadAssetAsync<SceneScriptableObject>(checkPointData.CurrentSceneGuid);
                yield return currentSoHandle;
                if (currentSoHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    _currentScene = currentSoHandle.Result;
                }
                _lastSceneCheckPointName = checkPointData.LastSceneName;
                _lastCheckPointPosition = checkPointData.LastPosition;
                _lastCheckPointFacingDirection = checkPointData.LastDirection;
            }
        }
        #endregion
    }
}
