using System;
using CryptoQuest.System.Dialogue.Managers;
using UnityEngine;
using UnityEngine.Playables;

namespace CryptoQuest.System.CutsceneSystem.CustomTimelineTracks.YarnSpinnerNodeControlTrack
{
    [Serializable]
    public class YarnSpinnerNodePlayableBehaviour : PlayableBehaviour
    {
        [HideInInspector]
        public string YarnNodeName = "Start";

        /// <summary>
        /// We need to wait for the player to actually finish reading all the dialogues before we can continue the timeline.
        /// </summary>
        public bool PauseTimelineOnClipEnds = true;

        protected bool _played = false;
        private bool _hasCompleted = false;

        /// <summary>
        /// Show dialogue using YarnSpinner.
        /// </summary>
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            if (_played)
                return;
            _played = true;
            _hasCompleted = false;

            if (Application.isPlaying)
            {
                if (!playable.GetGraph().IsPlaying()) return;
                if (string.IsNullOrEmpty(YarnNodeName)) return;
                YarnSpinnerDialogueManager.DialogueCompletedEvent += OnDialogueCompleted;
                YarnSpinnerDialogueManager.PlayDialogueRequested?.Invoke(YarnNodeName);
            }
#if UNITY_EDITOR
            else
            {
                Debug.Log($"YarnSpinnerNodePlayableBehaviour: ProcessFrame: {YarnNodeName}");
            }
#endif
        }

        private void OnDialogueCompleted(string yarnNode)
        {
            YarnSpinnerDialogueManager.DialogueCompletedEvent -= OnDialogueCompleted;
            if (yarnNode != YarnNodeName) return;
            _hasCompleted = true;
        }


        /// <summary>
        /// Try to pause the timeline when the player reading the dialogue
        /// </summary>
        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            if (!Application.isPlaying
                || !playable.GetGraph().IsPlaying()
                || playable.GetGraph().GetRootPlayable(0).IsDone()
                || !_played
                || _hasCompleted)
            {
                return;
            }

            if (!PauseTimelineOnClipEnds) return;

            // pause the timeline until the player finishes reading through all the dialogue (When the dialogue closes)
            Debug.Log("OnBehaviourPause::Pause cutscene");
            YarnSpinnerDialogueManager.PauseTimelineRequested?.Invoke(true);
        }
    }
}