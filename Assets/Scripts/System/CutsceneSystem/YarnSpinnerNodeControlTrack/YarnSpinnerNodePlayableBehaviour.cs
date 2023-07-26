using System;
using CryptoQuest.System.CutsceneSystem.Events;
using CryptoQuest.System.Dialogue.Events;
using UnityEngine;
using UnityEngine.Playables;

namespace CryptoQuest.System.CutsceneSystem.YarnSpinnerNodeControlTrack
{
    [Serializable]
    public class YarnSpinnerNodePlayableBehaviour : PlayableBehaviour
    {
        public PlayDialogueEvent PlayDialogue;
        public PauseCutsceneEvent PauseTimelineEvent;

        /// <summary>
        /// We need to wait for the player to actually finish reading all the dialogues before we can continue the timeline.
        /// </summary>
        public bool PauseTimelineOnClipEnds = true;

        [HideInInspector] public string YarnNodeName = "Start";

        private bool _played = false;

        /// <summary>
        /// Show dialogue using YarnSpinner.
        /// </summary>
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            if (_played)
                return;

            if (Application.isPlaying)
            {
                if (!playable.GetGraph().IsPlaying()) return;
                if (string.IsNullOrEmpty(YarnNodeName)) return;
                PlayDialogue.RaiseEvent(YarnNodeName);
                _played = true;
            }
#if UNITY_EDITOR
            else
            {
                Debug.Log($"YarnSpinnerNodePlayableBehaviour: ProcessFrame: {YarnNodeName}");
            }
#endif
        }

        /// <summary>
        /// Try to pause the timeline when the player reading the dialogue
        /// </summary>
        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            Debug.Log("OnBehaviourPause");
            if (!Application.isPlaying
                || !playable.GetGraph().IsPlaying()
                || playable.GetGraph().GetRootPlayable(0).IsDone()
                || !_played)
            {
                Debug.Log("OnBehaviourPause::Skip");
                return;
            }

            // pause the timeline until the player finishes reading through all the dialogue (When the dialogue closes)
            if (PauseTimelineOnClipEnds)
            {
                Debug.Log("OnBehaviourPause::Pause cutscene");
                PauseTimelineEvent.RaiseEvent(true);
            }
        }
    }
}