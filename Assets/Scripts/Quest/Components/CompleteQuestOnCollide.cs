using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Events;
using UnityEngine;

namespace CryptoQuest.Quest.Components
{
    public class CompleteQuestOnCollide : MonoBehaviour
    {
        [SerializeField] private QuestSO _questToComplete;
        [SerializeField] private QuestTriggerEventChannelSO questTriggerEventChannel;

        [SerializeField] private ECollideActionType _actionType;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            if (_actionType != ECollideActionType.OnEnter) return;

            questTriggerEventChannel.RaiseEvent(_questToComplete);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            if (_actionType != ECollideActionType.OnExit) return;

            questTriggerEventChannel.RaiseEvent(_questToComplete);
        }

        private enum ECollideActionType
        {
            OnEnter = 0,
            OnExit = 1
        }
    }
}