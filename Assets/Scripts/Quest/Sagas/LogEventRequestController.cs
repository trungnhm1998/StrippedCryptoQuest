using IndiGames.Core.Events;
using UnityEngine;

namespace CryptoQuest.Quest.Sagas
{
    public class LogEventRequestController : SagaBase<QuestTriggeredAction>
    {
        [SerializeField] private QuestSaveSO _questSaveSO;

        protected override void HandleAction(QuestTriggeredAction ctx)
        {
            var questData = ctx.QuestData;
            if (!_questSaveSO.CompletedQuests.Contains(questData.Guid))
                return;

            ActionDispatcher.Dispatch(new LogEventAction(questData.QuestName));
        }
    }
}