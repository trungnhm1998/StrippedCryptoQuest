using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Events;
using UnityEngine;

namespace CryptoQuest.Quest.Components
{
    public class QuestActor : MonoBehaviour
    {
        [SerializeField] private QuestSO _quest;
        [SerializeField] private QuestTriggerEventChannelSO questTriggerEventChannel;

        public void Interact()
        {
            if (_quest == null) return;

            questTriggerEventChannel.RaiseEvent(_quest);
        }
    }
}