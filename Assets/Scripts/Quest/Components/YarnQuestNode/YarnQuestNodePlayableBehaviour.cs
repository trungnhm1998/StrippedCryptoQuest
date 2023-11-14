using System;
using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Controllers;
using CryptoQuest.System.CutsceneSystem.CustomTimelineTracks.YarnSpinnerNodeControlTrack;
using CryptoQuest.System.Dialogue.Managers;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;

namespace CryptoQuest.Quest.Components.YarnQuestNode
{
    [Serializable]
    public class YarnQuestNodePlayableBehaviour : YarnSpinnerNodePlayableBehaviour
    {
        [field: SerializeField] public YarnDialogWithQuestSo YarnDialog { get; set; }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            if (_played)
                return;
            _played = true;

            if (Application.isPlaying)
            {
                if (!playable.GetGraph().IsPlaying()) return;
                string yarnNode = YarnDialog.YarnQuestDef.YarnNode;

                QuestCutsceneController.RegisterYarnQuestDef?.Invoke(YarnDialog.YarnQuestDef);
                YarnSpinnerDialogueManager.PlayDialogueRequested?.Invoke(yarnNode);
            }
#if UNITY_EDITOR
            else
            {
                Debug.Log($"YarnQuestNodePlayableBehaviour: ProcessFrame: {YarnNodeName}");
            }
#endif
        }
    }
}