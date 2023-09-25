using CryptoQuest.Gameplay.Battle;
using CryptoQuest.Gameplay.Encounter;
using CryptoQuest.Quest.Authoring;
using UnityEngine;

namespace CryptoQuest.Quest
{
    public class QuestTriggerBattle : MonoBehaviour
    {
        [field: SerializeReference] private AbstractObjective _quest;
        [field: SerializeField] private Battlefield _battlefieldToLoad;

        private void OnEnable()
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
            BattleLoader.RequestLoadBattle(_battlefieldToLoad.Id);
        }
    }
}