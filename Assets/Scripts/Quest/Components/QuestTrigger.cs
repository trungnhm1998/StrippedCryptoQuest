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
        private GiverActionCollider _actionCollider;

        public void Init(QuestSO dataQuest, GiverActionCollider actionCollider)
        {
            Quest = dataQuest;
            _actionCollider = actionCollider;
        }

        public void TriggerQuest()
        {
            _triggerQuestEventChannel.RaiseEvent(Quest);
            _actionCollider.BoxCollider2D.enabled = false;
        }
    }
}