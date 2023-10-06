using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Components;
using CryptoQuest.Quest.Controllers;
using IndiGames.Core.SceneManagementSystem.ScriptableObjects;

namespace CryptoQuest.Quest.Categories
{
    public class TeleportQuestSO : QuestSO
    {
        public SceneScriptableObject Destination;

        public override QuestInfo CreateQuest(QuestManager questManager)
            => new TeleportQuestInfo(questManager, this);
    }

    public class TeleportQuestInfo : QuestInfo<TeleportQuestSO>
    {
        private QuestManager _questManager;
        private QuestTeleportController _questTeleportController;

        public TeleportQuestInfo(QuestManager questManager, TeleportQuestSO teleportQuestSo) : base(teleportQuestSo)
        {
            _questManager = questManager;
        }

        public override void TriggerQuest()
        {
            FinishQuest();
        }

        public override void GiveQuest()
        {
            base.GiveQuest();
            _questTeleportController = _questManager.GetComponent<QuestTeleportController>();
            _questTeleportController.GiveQuest(this);
        }
    }
}