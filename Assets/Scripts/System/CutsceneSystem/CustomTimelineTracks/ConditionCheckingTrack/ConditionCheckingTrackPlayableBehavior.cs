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

        private bool _played;
        private PlayableDirector _playableDirector;

        public enum EConditions
        {
            False = 0,
            True = 1
        }

        public override void OnGraphStart(Playable playable)
        {
            if (_playableDirector != null) return;
            _playableDirector = playable.GetGraph().GetResolver() as PlayableDirector;
        }

        public void SetDirector(PlayableDirector playableDirector)
        {
            _playableDirector = playableDirector;
        }

        public override void PrepareFrame(Playable playable, FrameData info)
        {
            if (_played) return;
            _played = true;

            bool condition = PlayNodeWhileConditionIs == EConditions.True;

            CutSceneChoiceInfo choiceInfo = new CutSceneChoiceInfo(ConditionToCheck);
            choiceInfo.ConfigureChoiceStatus();

            if (choiceInfo.HasMadeChoice == condition) return;
            _playableDirector.time += playable.GetDuration();
        }
    }
}