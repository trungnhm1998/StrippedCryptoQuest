using System;
using CryptoQuest.Quest.Authoring;
using CryptoQuest.System.CutsceneSystem.CustomTimelineTracks.YarnSpinnerNodeControlTrack;
using CryptoQuest.System.Dialogue.Managers;
using UnityEngine;
using UnityEngine.Playables;

namespace CryptoQuest.Quest.Components.YarnQuestNode
{
    [Serializable]
    public class YarnQuestNodePlayableBehaviour : YarnSpinnerNodePlayableBehaviour
    {
        private YarnDialogWithQuestSo _yarnDialogWithQuestSo;

        public void SetYarnQuestSo(YarnDialogWithQuestSo yarnDialogWithQuestSo)
        {
            _yarnDialogWithQuestSo = yarnDialogWithQuestSo;
        }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            if (_played)
                return;
            _played = true;

            if (Application.isPlaying)
            {
                if (!playable.GetGraph().IsPlaying()) return;
                if (string.IsNullOrEmpty(YarnNodeName)) return;
                YarnQuestManager.OnUpdateCurrentNode?.Invoke(_yarnDialogWithQuestSo.YarnQuestDef);
                string yarnNode = _yarnDialogWithQuestSo.YarnQuestDef.YarnNode;
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