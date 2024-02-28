﻿using System.Collections;
using IndiGames.Core.Common;
using IndiGames.Core.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.Events.ScriptableObjects;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IndiGames.Core.EditorTools
{
    public class EditorColdBoot : MonoBehaviour
    {
#if UNITY_EDITOR
        public SceneScriptableObject ThisScene => _thisSceneSO;
        [SerializeField] private SceneScriptableObject _thisSceneSO;
        [SerializeField] private SceneScriptableObject _globalManagersSO;
        [SerializeField] private SceneAssetReference[] _additionalScenes;

        [SerializeField]
        private ScriptableObjectAssetReference<LoadSceneEventChannelSO> _editorColdBootEventChannelSO;

        [Header("Raise on")]
        [SerializeField] private VoidEventChannelSO _sceneLoadedEventChannelSO;

        private bool _isStartFromEditor;

        private void Awake()
        {
            _isStartFromEditor =
                !SceneManager.GetSceneByName(_globalManagersSO.SceneReference.editorAsset.name).isLoaded
                && !_globalManagersSO.SceneReference.OperationHandle.IsValid();
        }

        private IEnumerator Start()
        {
            if (_isStartFromEditor == false) yield break;
            yield return _globalManagersSO.SceneReference.LoadSceneAsync(LoadSceneMode.Additive);
            var coldBootEventAssetHandle = _editorColdBootEventChannelSO.LoadAssetAsync<LoadSceneEventChannelSO>();
            yield return coldBootEventAssetHandle;

            var coldBootEvent = coldBootEventAssetHandle.Result;
            foreach (var sceneAssetReference in _additionalScenes)
            {
                yield return sceneAssetReference.LoadSceneAsync(LoadSceneMode.Additive);
            }

            SetupGameplayManagerSceneOrNotifySceneLoaded(coldBootEvent);
        }

        private void SetupGameplayManagerSceneOrNotifySceneLoaded(LoadSceneEventChannelSO coldBootEvent)
        {
            if (_thisSceneSO != null)
                coldBootEvent.RequestLoad(_thisSceneSO);
            else
                _sceneLoadedEventChannelSO.RaiseEvent();
        }
#endif
    }
}