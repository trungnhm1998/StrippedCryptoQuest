using System;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Quests
{
    public class ComponentEventRaiser : MonoBehaviour
    {
        [Serializable]
        public class QuestStatusChangedEvent : UnityEvent { }

        [SerializeField] private QuestStatusChangedEvent _questStatusChanged;

        public QuestStatusChangedEvent QuestStatusChanged
        {
            get => _questStatusChanged;
            set => _questStatusChanged = value;
        }

        [SerializeField] private bool _statusToRaise;
        [SerializeField] private Quest _questDefinition;

        private void OnEnable()
        {
            _questDefinition.StatusChanged += QuestStatusChangedHandler;
        }

        private void OnDisable()
        {
            _questDefinition.StatusChanged -= QuestStatusChangedHandler;
        }

        private void QuestStatusChangedHandler(bool hasCompleted)
        {
            if (hasCompleted == _statusToRaise)
            {
                _questStatusChanged.Invoke();
            }
        }
    }
}