using System.Collections;
using CryptoQuest.Quest.Categories;
using CryptoQuest.Quest.Events;
using CryptoQuest.System.CutsceneSystem.Events;
using UnityEngine;

namespace CryptoQuest.Quest.Actions
{
    [CreateAssetMenu(menuName = "Quest/Actions/CutsceneAction")]
    public class CutsceneAction : NextAction
    {
        public QuestEventChannelSO GiveQuestEventChannel;
        public CutsceneQuestSO CutsceneQuestToGive;
        public QuestCutsceneDef CutsceneDef;
        public float Delay = 0.5f;

        public override IEnumerator Execute()
        {
            yield return new WaitForSeconds(Delay);
            if (CutsceneQuestToGive != null)
                GiveQuestEventChannel.RaiseEvent(CutsceneQuestToGive);

            CutsceneDef.RaiseEvent();
        }
    }
}