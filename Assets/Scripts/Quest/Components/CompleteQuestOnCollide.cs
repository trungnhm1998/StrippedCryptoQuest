using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Events;
using UnityEngine;
using UnityEngine.Serialization;

namespace CryptoQuest.Quest.Components
{
    public class CompleteQuestOnCollide : MonoBehaviour
    {
        [SerializeField] private QuestSO _questToComplete;
        [SerializeField] private QuestEventChannelSO _questEventChannelSo;

        [SerializeField] private ECollideActionType _actionType;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            if (_actionType != ECollideActionType.OnEnter) return;

            _questEventChannelSo.RaiseEvent(_questToComplete);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            if (_actionType != ECollideActionType.OnExit) return;

            _questEventChannelSo.RaiseEvent(_questToComplete);
        }

        private enum ECollideActionType
        {
            OnEnter = 0,
            OnExit = 1
        }
    }
}