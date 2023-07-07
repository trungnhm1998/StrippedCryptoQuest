using System;
using System.Collections;
using CryptoQuest.UI.CutScene;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;

namespace CryptoQuest.System.CutScene
{
    public class CutSceneTrigger : MonoBehaviour
    {
        [Header("Option")]
        [SerializeField] private bool _isTriggerOnce;

        [SerializeField] private bool _playOnAwake;
        [SerializeField] private PlayableDirector _playableDirector;

        [Header("Listen to")]
        [SerializeField] private VoidEventChannelSO _onSpecificCutSceneEvent;

        [SerializeField] private VoidEventChannelSO _sceneLoadedEventChannelSO;

        [Header("Raise event")]
        [SerializeField] private PlayableDirectorChannelSO _onPlayCutsceneEvent;

        private void OnEnable()
        {
            _sceneLoadedEventChannelSO.EventRaised += PlayCutSceneOnAwake;
            // _onSpecificCutSceneEvent.EventRaised += OnSpecificCutSceneEventRaised;
        }

        private void OnDisable()
        {
            _sceneLoadedEventChannelSO.EventRaised -= PlayCutSceneOnAwake;
            // _onSpecificCutSceneEvent.EventRaised -= OnSpecificCutSceneEventRaised;
        }

        private void Update()
        {
            // click T in new input system
        }

        private void PlayCutSceneOnAwake()
        {
            if (!_playOnAwake) return;
            if (_onPlayCutsceneEvent == null) return;

            StartCoroutine(PlayCutScene());
        }

        private IEnumerator PlayCutScene()
        {
            yield return new WaitForSeconds(1f);
            _onPlayCutsceneEvent.RaiseEvent(_playableDirector);
        }

        private void OnSpecificCutSceneEventRaised()
        {
            if (_onPlayCutsceneEvent == null) return;
            _onPlayCutsceneEvent.RaiseEvent(_playableDirector);

            if (_isTriggerOnce) Destroy(this);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_onSpecificCutSceneEvent != null) _onSpecificCutSceneEvent.RaiseEvent();
        }
    }
}