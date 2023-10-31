using System;
using CryptoQuest.System.Dialogue.Events;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Playables;
using UnityEngine.Timeline;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CryptoQuest.System.CutsceneSystem.CustomTimelineTracks.YarnSpinnerNodeControlTrack
{
    [Serializable]
    [DisplayName("Yarn Node Clip")]
    public class YarnSpinnerNodePlayableAsset : PlayableAsset, ITimelineClipAsset
    {
        public string YarnNodeName = "Start";
        public YarnProjectConfigEvent OnYarnProjectConfigEvent;

        [SerializeField] private YarnSpinnerNodePlayableBehaviour _template;

        public ClipCaps clipCaps => ClipCaps.None;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<YarnSpinnerNodePlayableBehaviour>.Create(graph, _template);

            _template.YarnNodeName = YarnNodeName;
            _template.OnYarnProjectConfigEvent = OnYarnProjectConfigEvent;

            return playable;
        }

#if UNITY_EDITOR
        public void Editor_SetConfigEvent()
        {
            string[] guids = AssetDatabase.FindAssets("t:YarnProjectConfigEvent");

            if (guids.Length == 0) return;

            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            OnYarnProjectConfigEvent = AssetDatabase.LoadAssetAtPath<YarnProjectConfigEvent>(path);
        }
#endif
    }
}