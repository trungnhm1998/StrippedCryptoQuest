using UnityEngine;

namespace CryptoQuest.Quest
{
    public class GameObjectActivator : MonoBehaviour
    {
        [SerializeField] private bool _isActivated;
        [SerializeField] private bool _statusToActivate;
        [SerializeField] private Quest _questDefinition;
        [SerializeField] private GameObject _target;

        private void OnEnable()
        {
            _questDefinition.StatusChanged += QuestStatusChanged;
        }

        private void OnDisable()
        {
            _questDefinition.StatusChanged -= QuestStatusChanged;
        }

        public void CheckQuestStatus()
        {
        }

        private void QuestStatusChanged(bool hasCompleted)
        {
            if (hasCompleted == _statusToActivate)
            {
                _target.SetActive(_isActivated);
            }
        }
    }
}