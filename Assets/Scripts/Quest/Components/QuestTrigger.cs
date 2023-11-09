using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Events;
using CryptoQuest.System.Dialogue.Managers;
using CryptoQuest.System.Dialogue.YarnManager;
using UnityEngine;

namespace CryptoQuest.Quest.Components
{
    public class QuestTrigger : MonoBehaviour
    {
        [field: Header("Quest Configs"), SerializeField]
        public QuestSO Quest { get; private set; }

        [SerializeField] private QuestEventChannelSO _triggerQuestEventChannel;
        [SerializeField] private YarnProjectConfigSO _yarnProjectConfig;
        private GiverActionCollider _actionCollider;

        private void Awake()
        {
            if (_yarnProjectConfig)
                YarnSpinnerDialogueManager.YarnProjectRequested?.Invoke(_yarnProjectConfig);
        }

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