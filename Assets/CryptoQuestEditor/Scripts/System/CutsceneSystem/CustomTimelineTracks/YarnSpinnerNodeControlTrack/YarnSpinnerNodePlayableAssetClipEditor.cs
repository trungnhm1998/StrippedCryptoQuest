using CryptoQuest.System.CutsceneSystem.CustomTimelineTracks.YarnSpinnerNodeControlTrack;
using UnityEditor.Timeline;
using UnityEngine.Timeline;

namespace CryptoQuestEditor.System.CutsceneSystem.CustomTimelineTracks.YarnSpinnerNodeControlTrack
{
    [CustomTimelineEditor(typeof(YarnSpinnerNodePlayableAsset))]
    public class YarnSpinnerNodePlayableAssetClipEditor : ClipEditor
    {
        public override void OnClipChanged(TimelineClip clip)
        {
            var yarnSpinnerNodePlayableAsset = clip.asset as YarnSpinnerNodePlayableAsset;
            if (yarnSpinnerNodePlayableAsset == null) return;

            clip.displayName = string.IsNullOrEmpty(yarnSpinnerNodePlayableAsset.YarnNodeName)
                ? "Start"
                : yarnSpinnerNodePlayableAsset.YarnNodeName;
        }
    }
}