using CryptoQuest.Character.Ability;
using CryptoQuest.Gameplay;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.EffectSystem.Components;
using UnityEngine;

namespace CryptoQuest.Battle
{
    [RequireComponent(typeof(AbilitySystemBehaviour))]
    public class Character : MonoBehaviour, ICharacter
    {
        [SerializeField] private NormalAttackAbility _normalAttackAbility;

        private NormalAttackAbilitySpec _normalAttackAbilitySpec;
        private AbilitySystemBehaviour _gas;
        public AttributeSystemBehaviour Attributes => _gas.AttributeSystem;
        public EffectSystemBehaviour GameplayEffectSystem => _gas.EffectSystem;
        public AbilitySystemBehaviour AbilitySystem => _gas;
        public Elemental Element { get; set; }

        private void Awake()
        {
            _gas = GetComponent<AbilitySystemBehaviour>();
            _normalAttackAbilitySpec = _gas.GiveAbility<NormalAttackAbilitySpec>(_normalAttackAbility);
        }

        public void Init()
        {
            var components = GetComponents<IComponent>();
            foreach (var comp in components)
                comp.Init(this);
        }

        public void Attack(ICharacter enemy)
        {
            _normalAttackAbilitySpec.Execute(enemy);
        }
    }
}