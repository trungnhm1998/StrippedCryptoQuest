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
        private PlayableDirector _playableDirector;

        public enum EConditions
        {
            False = 0,
            True = 1
        }

        public override void PrepareFrame(Playable playable, FrameData info)
        {
            if (_played)
                return;
            _played = true;

            CutSceneChoiceInfo choiceInfo = new CutSceneChoiceInfo(ConditionToCheck);
            choiceInfo.ConfigureChoiceStatus();
            bool condition = PlayNodeWhileConditionIs == EConditions.True;
            if (choiceInfo.HasMadeChoice != condition)
            {
                _playableDirector.time += playable.GetDuration();
            }
        }

        public void SetDirector(PlayableDirector playableDirector)
        {
            _playableDirector = playableDirector;
        }
    }
}