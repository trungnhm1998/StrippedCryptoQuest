using CryptoQuest.Character.Attributes;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.EffectSystem.Components;
using UnityEngine;

namespace CryptoQuest.Battle.Components
{
    [RequireComponent(typeof(AbilitySystemBehaviour))]
    public class Character : MonoBehaviour, ICharacter
    {
        #region GAS

        private AbilitySystemBehaviour _gas;
        public AttributeSystemBehaviour AttributeSystem => _gas.AttributeSystem;
        public EffectSystemBehaviour GameplayEffectSystem => _gas.EffectSystem;
        public AbilitySystemBehaviour AbilitySystem => _gas;

        #endregion

        private Elemental _element;
        public Elemental Element => _element;

        private void Awake()
        {
            _gas = GetComponent<AbilitySystemBehaviour>();
        }

        public void Init(Elemental element)
        {
            _element = element;
            AttributeSystem.Init();

            var components = GetComponents<CharacterComponentBase>();
            foreach (var comp in components)
                comp.Init();
        }
    }
}