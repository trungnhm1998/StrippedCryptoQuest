using System.Collections;
using CryptoQuest.Battle;
using CryptoQuest.Gameplay.Encounter;
using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Events;
using UnityEngine;

namespace CryptoQuest.Quest.Actions
{
    [CreateAssetMenu(menuName = "QuestSystem/Actions/BattleAction")]
    public class BattleAction : NextAction
    {
        public QuestEventChannelSO GiveQuestEventChannel;
        public QuestSO QuestToGive;
        public Battlefield BattlefieldToLoad;
        public float Delay = 0.5f;

        public override IEnumerator Execute()
        {
            yield return new WaitForSeconds(Delay);
            if (QuestToGive != null)
                GiveQuestEventChannel.RaiseEvent(QuestToGive);
            BattleLoader.RequestLoadBattle(BattlefieldToLoad.Id);
        }
    }
}