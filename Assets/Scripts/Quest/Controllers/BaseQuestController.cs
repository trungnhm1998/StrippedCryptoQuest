using CryptoQuest.Quest.Components;
using UnityEngine;
using static CryptoQuest.System.ServiceProvider;

namespace CryptoQuest.Quest.Controllers
{
    public abstract class BaseQuestController : MonoBehaviour
    {
        protected QuestManager _questManager;
        private void Start() => _questManager = GetService<QuestManager>();

        protected abstract void OnQuestFinish();
    }
}