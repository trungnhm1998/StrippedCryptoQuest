using System.Collections;
using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Events;
using UnityEngine;

namespace CryptoQuest.Quest.Actions
{
    [CreateAssetMenu(menuName = "QuestSystem/Actions/GiveQuestAction")]
    public class GiveQuestAction : NextAction
    {
        public QuestEventChannelSO GiveQuestEventChannel;
        public QuestSO QuestToGive;
        public float Delay = 0f;

        public override IEnumerator Execute()
        {
            yield return new WaitForSeconds(Delay);
            GiveQuestEventChannel.RaiseEvent(QuestToGive);
        }
    }
}