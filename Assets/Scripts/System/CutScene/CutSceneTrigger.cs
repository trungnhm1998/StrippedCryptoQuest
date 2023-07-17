using System.Collections;
using CryptoQuest.Gameplay;
using CryptoQuest.System.Cutscene.Events;
using UnityEngine;
using UnityEngine.Playables;

namespace CryptoQuest.System.Cutscene
{
    public class CutsceneTrigger : MonoBehaviour
    {
        [Header("Option")]
        [SerializeField, Tooltip("Check if you want to destroy after cut scene done.")]
        private bool _playOneTimeOnly;

        [SerializeField] private PlayableDirector _playableDirector;

        [Header("Raise event")]
        [SerializeField] private PlayableDirectorChannelSO _onPlayCutsceneEvent;

        public void Awake()
        {
            StartCoroutine(PlayCutscene());
        }

        private void OnEnable()
        {
            _playableDirector.stopped += HandleDirectorStopped;
        }


        private void OnDisable()
        {
            _playableDirector.stopped -= HandleDirectorStopped;
        }

        private void HandleDirectorStopped(PlayableDirector obj)
        {
            // _gameplayBus.Hero.transform.position = _cutscenePlayer.transform.position;
            if (!_playOneTimeOnly) return;
            Destroy(this);
        }

        private IEnumerator PlayCutscene()
        {
            yield return new WaitForSeconds(1f); // wait for scene to load
            _onPlayCutsceneEvent.RaiseEvent(_playableDirector);
        }
    }
}