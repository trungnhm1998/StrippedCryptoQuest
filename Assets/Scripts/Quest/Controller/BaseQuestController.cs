using CryptoQuest.Quest.Events;
using UnityEngine;

namespace CryptoQuest.Quest
{
    public abstract class BaseQuestController : MonoBehaviour
    {
        [field: SerializeField] public QuestEventChannelSO TriggerQuestEventChannel { get; private set; }

        protected abstract void OnQuestFinish();
    }
}