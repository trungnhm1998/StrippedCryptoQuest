using CryptoQuest.Battle.Events;
using UnityEngine;

namespace CryptoQuest.Battle.Components
{
    class EnemyAttackBehaviour : MonoBehaviour
    {
        private NormalAttack _attackBehaviour;

        private void Awake() => _attackBehaviour = GetComponent<NormalAttack>();
        private void OnEnable() => _attackBehaviour.Attacking += ShakeUI;
        private void OnDisable() => _attackBehaviour.Attacking -= ShakeUI;
        private void ShakeUI(Character target, Character character, float damage) => BattleEventBus.RaiseEvent(new ShakeUIEvent());
    }
}