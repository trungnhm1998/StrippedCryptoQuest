using System.Collections.Generic;
using CryptoQuest.Battle.Events;
using CryptoQuest.Gameplay.Encounter;
using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Categories;
using CryptoQuest.Quest.Components;
using CryptoQuest.Quest.Events;
using IndiGames.Core.Events;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CryptoQuest.Quest.Controllers
{
    public class QuestBattleController : MonoBehaviour
    {
        private List<BattleQuestInfo> _currentlyProcessQuests = new();
        [SerializeField] private BattleResultEventSO _battleWonEvent;
        [SerializeField] private BattleResultEventSO _battleLostEvent;
        [SerializeField] private QuestEventChannelSO _triggerQuestEventChannel;
        [SerializeField] private QuestEventChannelSO _giveQuestEventChannel;


        private void OnEnable()
        {
            _battleWonEvent.EventRaised += OnBattleWon;
            _battleLostEvent.EventRaised += OnBattleLost;
        }

        private void OnDisable()
        {
            _battleWonEvent.EventRaised -= OnBattleWon;
            _battleLostEvent.EventRaised -= OnBattleLost;
        }

        public void GiveQuest(BattleQuestInfo questInfo)
        {
            _currentlyProcessQuests.Add(questInfo);
        }

        private void OnBattleLost(Battlefield battlefield)
        {
            foreach (var processingQuest in _currentlyProcessQuests)
            {
                if (processingQuest.Data.BattlefieldToConquer != battlefield) continue;
                HandleBattleLost(processingQuest);
                break;
            }
        }

        private void OnBattleWon(Battlefield battlefield)
        {
            for (var index = _currentlyProcessQuests.Count - 1; index > -1; index--)
            {
                var processingQuest = _currentlyProcessQuests[index];
                if (processingQuest.Data.BattlefieldToConquer != battlefield) continue;
                HandleBattleWon(processingQuest);
            }
        }

        private void HandleBattleLost(BattleQuestInfo info)
        {
            var loseQuest = info.Data.FirstTimeLoseQuest;

            if (info.Data.FirstTimeLoseQuest != null)
            {
                _giveQuestEventChannel.RaiseEvent(loseQuest);
                _triggerQuestEventChannel.RaiseEvent(loseQuest);
            }

            if (info.Data.GiveRepeatBattleQuest != null)
                _giveQuestEventChannel.RaiseEvent(info.Data.GiveRepeatBattleQuest);
        }

        private void HandleBattleWon(BattleQuestInfo battleQuest)
        {
            QuestSO loseQuest = battleQuest.Data.FirstTimeLoseQuest;
            _triggerQuestEventChannel.RaiseEvent(battleQuest.Data);

            if (battleQuest.Data.FirstTimeLoseQuest != null)
            {
                IQuestManager.OnRemoveProgressingQuest?.Invoke(loseQuest);
            }

            _currentlyProcessQuests.Remove(battleQuest);
        }
    }

    public class PauseCutsceneAction : ActionBase { }
    public class ResumeCutsceneAction : ActionBase { }
}