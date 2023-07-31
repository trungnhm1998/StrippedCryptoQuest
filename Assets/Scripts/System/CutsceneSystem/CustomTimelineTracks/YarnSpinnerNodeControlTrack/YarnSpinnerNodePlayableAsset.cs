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
    public class YarnSpinnerNodePlayableAsset : PlayableAsset, ITimelineClipAsset
    {
        [HideInInspector] public YarnProject Project;
        [HideInInspector] public string YarnNodeName = "Start";

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