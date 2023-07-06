using CryptoQuest.Events.UI;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace CryptoQuest.System.CutScene.DialogueControlTrack
{
    public class DialogueClip : PlayableAsset, ITimelineClipAsset
    {
        [SerializeField] private DialogueBehaviour _template = default;

        [HideInInspector] public DialogEventChannelSO PlayDialogueEvent;
        [HideInInspector] public VoidEventChannelSO PauseTimelineEvent;

        public ClipCaps clipCaps
        {
            get { return ClipCaps.None; }
        }

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            ScriptPlayable<DialogueBehaviour> playable = ScriptPlayable<DialogueBehaviour>.Create(graph, _template);

            _template.PlayDialogueEvent = PlayDialogueEvent;
            _template.PauseTimelineEvent = PauseTimelineEvent;

            return playable;
        }
    }
}