using System.Collections;
using CryptoQuest.UI.CutScene;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.Playables;

namespace CryptoQuest.System.CutScene
{
    public class CutSceneTrigger : MonoBehaviour
    {
        [Header("Option")]
        [SerializeField, Tooltip("Check if you want cut scene play immediately")]
        private bool _playOnAwake;

        [SerializeField, Tooltip("Check if you want to destroy after cut scene done.")]
        private bool _playOneTimeOnly;

        [SerializeField] private PlayableDirector _playableDirector;

        [Header("Listen to")]
        [SerializeField] private CutsceneEventChannelSO _sceneLoadedEventChannelSO;

        [Header("Raise event")]
        [SerializeField] private PlayableDirectorChannelSO _onPlayCutsceneEvent;

        private void Awake()
        {
            if (!_playOnAwake) return;
            StartCoroutine(PlayCutscene());
        }

        private void OnEnable()
        {
            _sceneLoadedEventChannelSO.EventRaised += HandleCutscenePlaying;
            _playableDirector.stopped += HandleDirectorStopped;
        }

        private void OnDisable()
        {
            _sceneLoadedEventChannelSO.EventRaised -= HandleCutscenePlaying;
            _playableDirector.stopped -= HandleDirectorStopped;
        }

        private void HandleDirectorStopped(PlayableDirector obj)
        {
            if (!_playOneTimeOnly) return;
            Destroy(this);
        }

        private void HandleCutscenePlaying()
        {
            if (_onPlayCutsceneEvent == null) return;
            StartCoroutine(PlayCutscene());
        }

        private IEnumerator PlayCutscene()
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