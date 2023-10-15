using System;
using UnityEngine;
using UnityEngine.Playables;

namespace CryptoQuest.System.CutsceneSystem.CustomTimelineTracks.FadeTransitionTrack
{
    [Serializable]
    public class FadeTransitionPlayableBehavior : PlayableBehaviour
    {
        [HideInInspector]
        public float BlackScreenDuration;

        // public bool PauseTimelineOnClipEnds = true;

        private bool _played = false;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            if (_played)
                return;
            _played = true;

            if (Application.isPlaying)
            {
                if (!playable.GetGraph().IsPlaying()) return;
                if (BlackScreenDuration <= 0) return;
            }
        }
    }
}