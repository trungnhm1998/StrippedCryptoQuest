using System;
using CryptoQuest.Quest.Authoring;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace CryptoQuest.Quest.Components.YarnQuestNode
{
    public class YarnQuestNodePlayableAsset : PlayableAsset, ITimelineClipAsset
    {
        [
            SerializeField,
            Obsolete("Use YarnDialog in template instead"),
            Tooltip("Do not use this field, use YarnDialog in template instead")
        ]
        private YarnDialogWithQuestSo _yarnDialogWithQuest;

        [SerializeField] private YarnQuestNodePlayableBehaviour _template;
        public ClipCaps clipCaps => ClipCaps.None;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            if (_yarnDialogWithQuest != null)
            {
                _template.YarnDialog = _yarnDialogWithQuest;
            }

            var playable = ScriptPlayable<YarnQuestNodePlayableBehaviour>.Create(graph, _template);

            return playable;
        }
    }
}