using System;
using UnityEngine.Timeline;
using Yarn.Unity;

namespace CryptoQuest.Quest.Components.YarnQuestNode
{
    [Serializable]
    [TrackClipType(typeof(YarnQuestNodePlayableAsset))]
    [TrackColor(238, 238, 238)]
    public class YarnQuestNodeTrackAsset : PlayableTrack { }
}