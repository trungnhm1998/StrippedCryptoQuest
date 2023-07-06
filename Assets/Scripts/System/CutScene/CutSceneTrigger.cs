using CryptoQuest.Gameplay.Quest.Dialogue.ScriptableObject;
using CryptoQuest.UI.CutScene;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.Playables;

namespace CryptoQuest.System.CutScene
{
    public class CutSceneTrigger : MonoBehaviour
    {
        [SerializeField] private DialogueScriptableObject _dialogue;

        [Header("Option")]
        [SerializeField] private bool _isTriggerOnce;

        [SerializeField] private bool _playOnAwake;
        [SerializeField] private PlayableDirector _playableDirector;

        [Header("Listen to")]
        [SerializeField] private VoidEventChannelSO _onSpecificCutSceneEvent;

        [Header("Raise event")]
        [SerializeField] private PlayableDirectorChannelSO _onPlayCutsceneEvent;

        private void OnEnable()
        {
            if (_onSpecificCutSceneEvent != null) _onSpecificCutSceneEvent.EventRaised += OnSpecificCutSceneEventRaised;

            if (!_playOnAwake) return;
            if (_onPlayCutsceneEvent == null) return;

            _onPlayCutsceneEvent.RaiseEvent(_playableDirector);
        }

        private void OnDisable()
        {
            if (_onSpecificCutSceneEvent != null) _onSpecificCutSceneEvent.EventRaised -= OnSpecificCutSceneEventRaised;
        }

        private void OnSpecificCutSceneEventRaised()
        {
            if (_onPlayCutsceneEvent == null) return;
            _onPlayCutsceneEvent.RaiseEvent(_playableDirector);

            if (_isTriggerOnce) Destroy(this);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // if (_onSpecificCutSceneEvent != null) _onSpecificCutSceneEvent.RaiseEvent();
            // TODO: Demo only
            _onPlayCutsceneEvent.RaiseEvent(_playableDirector);
        }
    }
}