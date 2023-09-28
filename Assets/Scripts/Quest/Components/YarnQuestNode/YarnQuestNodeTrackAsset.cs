using System;
using CryptoQuest.System.CutsceneSystem.CustomTimelineTracks.YarnSpinnerNodeControlTrack;
using UnityEngine.Timeline;
using Yarn.Unity;

namespace CryptoQuest.Quest.Components.YarnQuestNode
{
    [Serializable]
    [TrackClipType(typeof(YarnQuestNodePlayableAsset))]
    [TrackBindingType(typeof(YarnProject))]
    [TrackColor(238, 238, 238)]
    public class YarnQuestNodeTrackAsset : PlayableTrack { }
}