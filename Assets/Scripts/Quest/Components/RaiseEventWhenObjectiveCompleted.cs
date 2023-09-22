using CryptoQuest.Quest.Authoring;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Quest.Components
{
    public class RaiseEventWhenObjectiveCompleted : MonoBehaviour
    {
        [field: SerializeReference] private AbstractObjective _quest;

        public UnityEvent OnQuestCompleted;

        private void Awake()
        {
            if (_quest.IsCompleted)
                OnCompleted();

            _quest.OnCompleteObjective += OnCompleted;
        }

        private void OnDisable()
        {
            _quest.OnCompleteObjective -= OnCompleted;
        }

        private void OnCompleted()
        {
            OnQuestCompleted?.Invoke();
        }
    }
}