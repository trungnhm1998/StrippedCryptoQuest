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
        [SerializeField] private bool _isPauseWhenClipEnds = true;

        [Header("Dialogue Option")]
        [SerializeField] private DialogueScriptableObject _dialogueLine;

        [SerializeField] private VoidEventChannelSO _onLineEndedEvent;
        [SerializeField] private VoidEventChannelSO _onShowEmotionEvent;

        [Header("Raise event")]
        [HideInInspector] public DialogueEventChannelSO PlayDialogueEvent;

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

            if (_isPauseWhenClipEnds) PauseDialog();

            ShowDialog();
            ShowEmotion();
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

            if (!_isPauseWhenClipEnds) EndDialog();

            _isDialoguePlayed = false;
        }

        private void ShowEmotion()
        {
            if (_onShowEmotionEvent == null) return;
            _onShowEmotionEvent.RaiseEvent();
        }

        private void EndDialog()
        {
            if (_onLineEndedEvent == null) return;
            _onLineEndedEvent.RaiseEvent();
        }

        private void ShowDialog()
        {
            if (_dialogueLine == null) return;
            PlayDialogueEvent.Show(_dialogueLine);
        }

        private void PauseDialog() => PauseTimelineEvent.RaiseEvent();

        private bool IsClipDone(Playable playable) => playable.GetGraph().GetRootPlayable(0).IsDone();
        private bool IsPlaying(Playable playable) => Application.isPlaying && playable.GetGraph().IsPlaying();
    }
}