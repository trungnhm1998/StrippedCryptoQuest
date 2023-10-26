using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Events;
using UnityEngine;

namespace CryptoQuest.Quest.Components
{
    public class QuestTrigger : MonoBehaviour
    {
        [field: Header("Quest Configs"), SerializeField]
        public QuestSO Quest { get; private set; }

        [SerializeField] private QuestEventChannelSO _triggerQuestEventChannel;

        public void Init(QuestSO dataQuest) => Quest = dataQuest;
        public void TriggerQuest() => _triggerQuestEventChannel.RaiseEvent(Quest);
    }
}