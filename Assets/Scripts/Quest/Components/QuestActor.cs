using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Events;
using UnityEngine;
using UnityEngine.Serialization;

namespace CryptoQuest.Quest.Components
{
    public class QuestActor : MonoBehaviour
    {
        [SerializeField] private QuestSO _quest;
        [SerializeField] private QuestEventChannelSO _questEventChannelSo;

        public void Interact()
        {
            if (_quest == null) return;

            _questEventChannelSo.RaiseEvent(_quest);
        }
    }
}