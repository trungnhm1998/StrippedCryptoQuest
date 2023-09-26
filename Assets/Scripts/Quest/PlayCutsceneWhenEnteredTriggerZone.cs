using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Components;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Quest
{
    public class PlayCutsceneWhenEnteredTriggerZone : MonoBehaviour
    {
        [SerializeField] private QuestSO _quest;

        public UnityEvent OnQuestCompleted;

        private void OnEnable()
        {
            _quest.OnQuestCompleted += OnCompleted;
        }

        private void OnDisable()
        {
            _quest.OnQuestCompleted -= OnCompleted;
        }

        private void OnCompleted()
        {
            OnQuestCompleted?.Invoke();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                QuestManager.OnTriggerQuest?.Invoke(_quest);
            }
        }
    }
}