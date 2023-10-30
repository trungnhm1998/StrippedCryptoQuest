using CryptoQuest.Battle.Events;
using UnityEngine;

namespace CryptoQuest.Battle.Components
{
    public class HeroAttackBehaviour : MonoBehaviour
    {
        private NormalAttack _attackBehaviour;

        private void Awake() => _attackBehaviour = GetComponent<NormalAttack>();
        private void OnEnable() => _attackBehaviour.Attacking += PlayVfx;
        private void OnDisable() => _attackBehaviour.Attacking -= PlayVfx;

        private void PlayVfx(Character character, Character target, float damage) =>
            BattleEventBus.RaiseEvent(new HeroNormalAttackEvent { Target = target });
    }
}