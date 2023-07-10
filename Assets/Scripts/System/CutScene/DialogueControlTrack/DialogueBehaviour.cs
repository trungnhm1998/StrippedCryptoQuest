using System;
using CryptoQuest.Events.UI;
using CryptoQuest.Gameplay.Quest.Dialogue.ScriptableObject;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.Playables;

namespace CryptoQuest.System.CutScene.DialogueControlTrack
{
    [Serializable]
    public class DialogueBehaviour : PlayableBehaviour
    {
        [SerializeField] private bool _isPauseWhenClipEnds;

        [Header("Dialogue Option")]
        [SerializeField] private DialogueScriptableObject _dialogueLine;

        [SerializeField] private VoidEventChannelSO _onLineEndedEvent;

        [Header("Raise event")]
        [HideInInspector] public DialogEventChannelSO PlayDialogueEvent;

        [HideInInspector] public VoidEventChannelSO PauseTimelineEvent;

        private bool _isDialoguePlayed;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            if (_isDialoguePlayed || !IsPlaying(playable)) return;

            if (_dialogueLine == null)
            {
                Debug.LogWarning("This clip contains no DialogueLine");
                return;
            }

            ShowDialog();
            _isDialoguePlayed = true;
        }

        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            if (!_isDialoguePlayed || !IsPlaying(playable)) return;

            if (IsClipDone(playable))
            {
                Debug.LogWarning("This clip is done");
                return;
            }

            if (_isPauseWhenClipEnds)
                PauseDialog();
            else
                EndDialog();
        }

        private void EndDialog() => _onLineEndedEvent.RaiseEvent();

        private void ShowDialog() => PlayDialogueEvent.Show(_dialogueLine);

        private void PauseDialog() => PauseTimelineEvent.RaiseEvent();

        private bool IsClipDone(Playable playable) => playable.GetGraph().GetRootPlayable(0).IsDone();
        private bool IsPlaying(Playable playable) => Application.isPlaying && playable.GetGraph().IsPlaying();
    }
}