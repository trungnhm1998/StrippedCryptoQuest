using System.Collections.Generic;
using CryptoQuest.Battle;
using CryptoQuest.Battle.Events;
using CryptoQuest.Gameplay.Encounter;
using CryptoQuest.Quest.Events;
using UnityEngine;

namespace CryptoQuest.Quest
{
    public class QuestBattleController : MonoBehaviour
    {
        private List<BattleQuestInfo> _currentlyProcessQuests = new();
        [SerializeField] private BattleResultEventSO _battleCompletedEvent;
        [SerializeField] private QuestEventChannelSO _triggerQuestEventChannel;

        private void OnEnable()
        {
            _battleCompletedEvent.EventRaised += OnBattleCompleted;
        }

        private void OnDisable()
        {
            _battleCompletedEvent.EventRaised -= OnBattleCompleted;
        }

        public void TriggerBattle(Battlefield battlefield)
        {
            BattleLoader.RequestLoadBattle(battlefield);
        }

        public void GiveQuest(BattleQuestInfo questInfo)
        {
            _currentlyProcessQuests.Add(questInfo);
        }

        private void OnBattleCompleted(BattleResultInfo result)
        {
            var battleField = result.Battlefield;
            foreach (var processingQuest in _currentlyProcessQuests)
            {
                if (processingQuest.Data.BattlefieldToLoad != battleField) continue;
                HandleBattleResult(processingQuest, result);
                break;
            }
        }

        private void HandleBattleResult(BattleQuestInfo info, BattleResultInfo result)
        {
            bool isWinBattle = result.IsWin;
            var winQuest = info.Data.WinQuest;
            var loseQuest = info.Data.LoseQuest;
            _triggerQuestEventChannel.RaiseEvent(isWinBattle ? winQuest : loseQuest);
            _currentlyProcessQuests.Remove(info);
        }
    }
}