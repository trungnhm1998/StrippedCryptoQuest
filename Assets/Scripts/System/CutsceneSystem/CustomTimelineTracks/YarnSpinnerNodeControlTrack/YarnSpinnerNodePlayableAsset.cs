using System;
using CryptoQuest.System.CutsceneSystem.Events;
using CryptoQuest.System.Dialogue.Events;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace CryptoQuest.System.CutsceneSystem.CustomTimelineTracks.YarnSpinnerNodeControlTrack
{
    [Serializable]
    [DisplayName("Yarn Node Clip")]
    public class YarnSpinnerNodePlayableAsset : PlayableAsset, ITimelineClipAsset
    {
        public string YarnNodeName = "Start";

        [HideInInspector] public PlayDialogueEvent PlayDialogueEvent;
        [HideInInspector] public PauseCutsceneEvent PauseTimelineEvent;

        [SerializeField] private YarnSpinnerNodePlayableBehaviour _template;

        public ClipCaps clipCaps => ClipCaps.None;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<YarnSpinnerNodePlayableBehaviour>.Create(graph, _template);

            _template.YarnNodeName = YarnNodeName;
            _template.PlayDialogueEvent = PlayDialogueEvent;
            _template.PauseTimelineEvent = PauseTimelineEvent;

            return playable;
        }
    }
}