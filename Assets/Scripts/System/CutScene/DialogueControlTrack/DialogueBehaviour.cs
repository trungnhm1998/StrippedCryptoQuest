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
            if (_isDialoguePlayed) return;

            if (!Application.isPlaying) return;

            if (!playable.GetGraph().IsPlaying()) return;

            if (_dialogueLine != null)
            {
                _isDialoguePlayed = true;
                if (PlayDialogueEvent == null) return;
                PlayDialogueEvent.Show(_dialogueLine);
            }
            else
            {
                Debug.LogWarning("This clip contains no DialogueLine");
            }
        }

        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            if (!Application.isPlaying) return;

            if (!playable.GetGraph().IsPlaying()) return;

            if (!_isDialoguePlayed) return;

            if (!playable.GetGraph().GetRootPlayable(0).IsDone())
            {
                if (_isPauseWhenClipEnds)
                {
                    if (PauseTimelineEvent == null) return;
                    PauseTimelineEvent.RaiseEvent();
                }
                else
                {
                    _onLineEndedEvent?.RaiseEvent();
                }
            }
        }
    }
}