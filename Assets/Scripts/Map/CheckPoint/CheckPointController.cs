using CryptoQuest.Character.MonoBehaviours;
using CryptoQuest.System;
using IndiGames.Core.EditorTools;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        private SceneScriptableObject _currentScene;
        private Vector3 _lastCheckPointPosition;
        private int _lastCheckPointFacingDirection;
        private bool _isBackToCheckPoint = false;

        [SerializeField] private LoadSceneEventChannelSO _loadSceneEvent;
        [SerializeField] private SceneScriptableObject _defaultCheckpoint;

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
        }

        private void SaveNewLoadedSceneForCheckpoint(SceneScriptableObject nextScene)
        {
            _currentScene = nextScene;
            Debug.Log("Checkpoint: Set current scene " + nextScene.name);
        }

        public void SaveCheckPoint(Vector3 position, int facingDirection)
        {
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
            _loadSceneEvent.RequestLoad(_lastCheckPointScene);
        }
    }
}
