using CryptoQuest.Battle;
using CryptoQuest.Gameplay.Battle;
using CryptoQuest.Gameplay.Encounter;
using CryptoQuest.Quest.Authoring;
using UnityEngine;

namespace CryptoQuest.Quest.Components
{
    public class QuestTriggerBattle : MonoBehaviour
    {
        [field: SerializeReference] private AbstractObjective _objective;

        [SerializeField] private Battlefield _battlefieldToLoad;

        private void OnEnable()
        {
            _objective.OnCompleteObjective += OnCompleted;
        }

        private void OnDisable()
        {
            _objective.OnCompleteObjective -= OnCompleted;
        }

        private void OnCompleted()
        {
            BattleLoader.RequestLoadBattle(_battlefieldToLoad.Id);
        }
    }
}