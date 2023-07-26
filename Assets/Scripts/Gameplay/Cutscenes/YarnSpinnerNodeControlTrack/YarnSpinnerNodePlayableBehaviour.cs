using System;
using CryptoQuest.Gameplay.Cutscenes.Events;
using CryptoQuest.System.Dialogue.Events;
using UnityEngine;
using UnityEngine.Playables;

namespace CryptoQuest.Gameplay.Cutscenes.YarnSpinnerNodeControlTrack
{
    [Serializable]
    public class YarnSpinnerNodePlayableBehaviour : PlayableBehaviour
    {
        public PlayDialogueEvent PlayDialogue;
        public PauseCutsceneEvent PauseTimelineEvent;
        [HideInInspector] public string YarnNodeName = "Start";

        private bool _dialoguePlayed = false;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            if (_dialoguePlayed)
                return;

            _dialoguePlayed = true;
            if (Application.isPlaying)
            {
                if (!playable.GetGraph().IsPlaying()) return;
                if (string.IsNullOrEmpty(YarnNodeName)) return;
                PlayDialogue.RaiseEvent(YarnNodeName);
            }
#if UNITY_EDITOR
            else
            {
                Debug.Log($"YarnSpinnerNodePlayableBehaviour: ProcessFrame: {YarnNodeName}");
            }
#endif
        }

        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            Debug.Log("OnBehaviourPause");
            if (!Application.isPlaying) return;
            if (!playable.GetGraph().IsPlaying()
                || playable.GetGraph().GetRootPlayable(0).IsDone()) return;
            // pause the timeline until the player finishes reading through all the dialogue (When the dialogue closes)
            PauseTimelineEvent.RaiseEvent(true);
        }
    }
}