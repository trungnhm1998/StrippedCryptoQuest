using CryptoQuest.Quest.Components;
using UnityEngine;

namespace CryptoQuest.Quest
{
    public abstract class BaseQuestController : MonoBehaviour
    {
        public QuestManager QuestManager { get; set; }

        protected abstract void OnQuestFinish();
    }
}