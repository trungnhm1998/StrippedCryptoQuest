using CryptoQuest.Timeline.Position;
using UnityEditor.Timeline;
using UnityEngine.Timeline;

namespace CryptoQuestEditor.Timeline.Position
{
    [CustomTimelineEditor(typeof(PositionClip))]
    public class PositionPlayableAssetClipEditor : ClipEditor
    {
        public override void OnClipChanged(TimelineClip clip)
        {
            var positionPlayableAsset = clip.asset as PositionClip;
            if (positionPlayableAsset != null)
                clip.displayName = positionPlayableAsset.Position.ToString();
        }
    }
}