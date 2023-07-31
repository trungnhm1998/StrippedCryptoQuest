using System;
using UnityEngine;
using UnityEngine.Playables;

namespace CryptoQuest.System.CutsceneSystem.CustomTimelineTracks.YarnSpinnerNodeControlTrack
{
    [Serializable]
    public class YarnSpinnerNodePlayableBehaviour : PlayableBehaviour
    {
        public static event Action<string> PlayDialogueRequested;
        public static event Action<bool> PauseTimelineRequested;

        [HideInInspector]
        public string YarnNodeName = "Start";

        /// <summary>
        /// We need to wait for the player to actually finish reading all the dialogues before we can continue the timeline.
        /// </summary>
        public bool PauseTimelineOnClipEnds = true;


        private bool _played = false;

        /// <summary>
        /// Show dialogue using YarnSpinner.
        /// </summary>
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            if (_played)
                return;
            _played = true;

            if (Application.isPlaying)
            {
                if (!playable.GetGraph().IsPlaying()) return;
                if (string.IsNullOrEmpty(YarnNodeName)) return;
                PlayDialogueRequested?.Invoke(YarnNodeName);
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
            if (!Application.isPlaying
                || !playable.GetGraph().IsPlaying()
                || playable.GetGraph().GetRootPlayable(0).IsDone()
                || !_played)
            {
                return;
            }

            if (!PauseTimelineOnClipEnds) return;

            // pause the timeline until the player finishes reading through all the dialogue (When the dialogue closes)
            Debug.Log("OnBehaviourPause::Pause cutscene");
            PauseTimelineRequested?.Invoke(true);
        }
    }
}