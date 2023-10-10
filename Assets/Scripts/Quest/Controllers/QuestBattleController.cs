using System.Collections.Generic;
using CryptoQuest.Battle;
using CryptoQuest.Battle.Events;
using CryptoQuest.Gameplay.Encounter;
using CryptoQuest.Quest.Categories;
using CryptoQuest.Quest.Components;
using CryptoQuest.Quest.Events;
using UnityEngine;

namespace CryptoQuest.Quest.Controllers
{
    public class QuestBattleController : MonoBehaviour
    {
        private readonly List<BattleQuestInfo> _currentlyProcessQuests = new();
        [SerializeField] private BattleResultEventSO _battleCompletedEvent;
        [SerializeField] private QuestEventChannelSO _triggerQuestEventChannel;
        [SerializeField] private QuestEventChannelSO _giveQuestEventChannel;

        private void OnEnable()
        {
            _battleCompletedEvent.EventRaised += OnBattleCompleted;
        }

        private void OnDisable()
        {
            _battleCompletedEvent.EventRaised -= OnBattleCompleted;
        }


        public void GiveQuest(BattleQuestInfo questInfo)
        {
            _currentlyProcessQuests.Add(questInfo);
            _giveQuestEventChannel.RaiseEvent(questInfo.Data.WinQuest);
            _giveQuestEventChannel.RaiseEvent(questInfo.Data.LoseQuest);
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
            if (isWinBattle)
            {
                _triggerQuestEventChannel.RaiseEvent(winQuest);
                _triggerQuestEventChannel.RaiseEvent(info.Data);
                QuestManager.OnRemoveProgressingQuest?.Invoke(loseQuest);
                _currentlyProcessQuests.Remove(info);
            }
            else
            {
                _triggerQuestEventChannel.RaiseEvent(loseQuest);
            }
        }
    }
}