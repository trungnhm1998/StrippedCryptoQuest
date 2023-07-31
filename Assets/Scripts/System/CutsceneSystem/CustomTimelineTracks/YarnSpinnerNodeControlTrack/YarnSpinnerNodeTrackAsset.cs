using System;
using CryptoQuest.System.CutsceneSystem.Events;
using CryptoQuest.System.Dialogue.Events;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Yarn.Unity;

namespace CryptoQuest.System.CutsceneSystem.CustomTimelineTracks.YarnSpinnerNodeControlTrack
{
    [Serializable]
    [TrackClipType(typeof(YarnSpinnerNodePlayableAsset))]
    [TrackBindingType(typeof(YarnProject))]
    [TrackColor(238, 238, 238)]
    public class YarnSpinnerNodeTrackAsset : PlayableTrack
    {
        public PlayDialogueEvent PlayDialogueEvent;
        public PauseCutsceneEvent PauseTimelineEvent;

#if UNITY_EDITOR
        private void OnValidate()
        {
            foreach (var clip in GetClips())
            {
                var asset = clip.asset as YarnSpinnerNodePlayableAsset;
                if (asset == null) continue;

                asset.PlayDialogueEvent = PlayDialogueEvent;
                asset.PauseTimelineEvent = PauseTimelineEvent;
            }
        }
#endif

        public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
        {
            var yarnProject = director.GetGenericBinding(this) as YarnProject;
            if (yarnProject == null)
            {
                Debug.LogWarning("Yarn Spinner Node Track has no Yarn Project bound to it.");
                return;
            }

            foreach (var clip in GetClips())
            {
                var asset = clip.asset as YarnSpinnerNodePlayableAsset;
                if (asset == null) continue;

                asset.PlayDialogueEvent = PlayDialogueEvent;
                asset.PauseTimelineEvent = PauseTimelineEvent;
            }

            base.GatherProperties(director, driver);
        }
    }
}