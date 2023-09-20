using CryptoQuest.Quest.Authoring;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Quest.Components
{
    public class ObjectiveTrack : MonoBehaviour
    {
        [field: SerializeReference] private AbstractObjective _objective;

        public UnityEvent OnObjectiveCompleted;

        private void Awake()
        {
            if (_objective.IsCompleted)
                OnCompleted();

            _objective.OnCompleteObjective += OnCompleted;
        }

        private void OnDisable()
        {
            _objective.OnCompleteObjective -= OnCompleted;
        }

        private void OnCompleted()
        {
            OnObjectiveCompleted?.Invoke();
        }
    }
}