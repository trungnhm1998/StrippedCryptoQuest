using System;
using CryptoQuest.System.TransitionSystem;
using UnityEngine;
using UnityEngine.Playables;

namespace CryptoQuest.System.CutsceneSystem.CustomTimelineTracks.FadeTransitionTrack
{
    [Serializable]
    public class TransitionPlayableBehavior : PlayableBehaviour
    {
        [HideInInspector] public AbstractTransition Transition;
        [HideInInspector] public TransitionEventChannelSO TransitionEvent;
        private bool _played = false;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            if (_played)
                return;
            _played = true;

            if (Application.isPlaying)
            {
                if (!playable.GetGraph().IsPlaying()) return;
                TransitionEvent.RaiseEvent(Transition);
            }
        }
    }
}