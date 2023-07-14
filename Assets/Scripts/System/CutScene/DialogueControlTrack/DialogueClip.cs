using CryptoQuest.Events.UI;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace CryptoQuest.System.CutScene.DialogueControlTrack
{
    public class DialogueClip : PlayableAsset, ITimelineClipAsset
    {
        [SerializeField] private DialogueBehaviour _dialogueBehaviour = default;

        [HideInInspector] public DialogueEventChannelSO playDialogueEvent;
        [HideInInspector] public VoidEventChannelSO pauseTimelineEvent;

        public ClipCaps clipCaps => ClipCaps.None;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            ScriptPlayable<DialogueBehaviour> playable =
                ScriptPlayable<DialogueBehaviour>.Create(graph, _dialogueBehaviour);

            _dialogueBehaviour.PlayDialogueEvent = playDialogueEvent;
            _dialogueBehaviour.PauseTimelineEvent = pauseTimelineEvent;

            return playable;
        }
    }
}