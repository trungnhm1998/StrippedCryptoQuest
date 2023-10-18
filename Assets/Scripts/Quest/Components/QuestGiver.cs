using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Events;
using UnityEngine;

namespace CryptoQuest.Quest.Components
{
    public class QuestGiver : MonoBehaviour
    {
        [field: Header("Quest Configs"), SerializeField] public QuestSO Quest { get; private set; }

        [SerializeField] private QuestEventChannelSO _giveQuestEventChannel;

        public void GiveQuest() => _giveQuestEventChannel.RaiseEvent(Quest);

        public void Init(QuestSO dataQuest) => Quest = dataQuest;
    }
}