using System.Collections;
using CryptoQuest.UI.CutScene;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.Playables;
using NotImplementedException = System.NotImplementedException;

namespace CryptoQuest.System.CutScene
{
    public class CutSceneTrigger : MonoBehaviour
    {
        [Header("Option")]
        [SerializeField, Tooltip("Check here if you want cut scene play immediately")]
        private bool _playOnAwake;

        [SerializeField, Tooltip("Check if you want to destroy after cut scene done.")]
        private bool _playOneTimeOnly;

        [SerializeField] private PlayableDirector _playableDirector;

        [Header("Listen to")]
        [SerializeField] private VoidEventChannelSO _sceneLoadedEventChannelSO;

        [Header("Raise event")]
        [SerializeField] private PlayableDirectorChannelSO _onPlayCutsceneEvent;

        private void OnEnable()
        {
            _sceneLoadedEventChannelSO.EventRaised += PlayCutSceneOnAwake;
            _playableDirector.stopped += HandleDirectorStopped;
        }

        private void OnDisable()
        {
            _sceneLoadedEventChannelSO.EventRaised -= PlayCutSceneOnAwake;
            _playableDirector.stopped -= HandleDirectorStopped;
        }

        private void HandleDirectorStopped(PlayableDirector obj)
        {
            if (!_playOneTimeOnly) return;
            Destroy(this);
        }

        private void PlayCutSceneOnAwake()
        {
            if (!_playOnAwake) return;
            if (_onPlayCutsceneEvent == null) return;

            StartCoroutine(PlayCutScene());
        }

        private IEnumerator PlayCutScene()
        {
            yield return new WaitForSeconds(1f); // wait for scene to load
            _onPlayCutsceneEvent.RaiseEvent(_playableDirector);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            _onPlayCutsceneEvent.RaiseEvent(_playableDirector);
        }
    }
}