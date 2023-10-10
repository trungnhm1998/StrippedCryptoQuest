using CryptoQuest.Quest.Components;
using UnityEngine;

namespace CryptoQuest.Quest.Controller
{
    public abstract class BaseQuestController : MonoBehaviour
    {
        public QuestManager QuestManager { get; set; }

        protected abstract void OnQuestFinish();
    }
}