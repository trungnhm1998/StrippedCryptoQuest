using System;
using UnityEngine;
using UnityEngine.Playables;

namespace CryptoQuest.System.CutsceneSystem.CustomTimelineTracks.ConditionCheckingTrack
{
    [Serializable]
    public class ConditionCheckingTrackPlayableBehavior : PlayableBehaviour
    {
        public string ConditionToCheck = "";
        public EConditions PlayNodeWhileConditionIs = EConditions.False;
        private bool _played = false;


        public enum EConditions
        {
            False = 0,
            True = 1
        }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            if (_played)
                return;
            _played = true;

            if (Application.isPlaying)
            {
                if (!playable.GetGraph().IsPlaying()) return;
                PlayableDirector director = playable.GetGraph().GetResolver() as PlayableDirector;
                if (director == null) return;
                CutSceneChoiceInfo choiceInfo = new CutSceneChoiceInfo(ConditionToCheck);
                choiceInfo.ConfigureChoiceStatus();
                bool condition = PlayNodeWhileConditionIs == EConditions.True;
                if (choiceInfo.HasMadeChoice != condition)
                {
                    director.time += playable.GetDuration();
                }
            }
        }
    }
}