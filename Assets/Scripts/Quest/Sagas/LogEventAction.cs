using CryptoQuest.Quest.Authoring;
using IndiGames.Core.Events;

namespace CryptoQuest.Quest.Sagas
{
    public class LogEventAction : ActionBase
    {
        public string QuestName;

        public LogEventAction(string questName)
        {
            QuestName = questName;
        }
    }

    public class QuestTriggeredAction : ActionBase
    {
        public QuestSO QuestData;

        public QuestTriggeredAction(QuestSO quest)
        {
            QuestData = quest;
        }
    }
}