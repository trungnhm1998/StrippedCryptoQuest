using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace CryptoQuest.Timeline.Position
{
    /// <summary>
    /// Track to control the position of a GameObject.
    /// </summary>
    [TrackColor(0.1394896f, 0.4411765f, 0.3413077f)]
    [Serializable]
    [TrackClipType(typeof(PositionClip))]
    [TrackBindingType(typeof(Transform))]
    [ExcludeFromPreset]
    public class PositionPlayableTrack : PlayableTrack
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<PositionMixerPlayable>.Create(graph, inputCount);
        }

        /// <summary>
        /// Revert the position of the GameObject to the original value, when the timeline is stopped/done previewing.
        /// <a href="https://forum.unity.com/threads/solved-how-to-restore-state-of-the-game-object-modified-by-script-playable.888682/#post-5837596">Source</a>
        /// </summary>
        /// <param name="director"></param>
        /// <param name="driver"></param>
        public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
        {
            var boundTransform = GetTransformBinding(director);
            if (boundTransform == null) return;

            driver.AddFromName<Transform>(boundTransform.gameObject, "m_LocalPosition.x");
            driver.AddFromName<Transform>(boundTransform.gameObject, "m_LocalPosition.y");
            driver.AddFromName<Transform>(boundTransform.gameObject, "m_LocalPosition.z");

            base.GatherProperties(director, driver);
        }

        private Transform GetTransformBinding(PlayableDirector director)
        {
            if (director == null)
                return null;

            var binding = director.GetGenericBinding(this);

            var transform = binding as Transform;
            if (transform != null)
                return transform;

            var comp = binding as Component;
            return comp != null ? comp.transform : null;
        }
    }
}