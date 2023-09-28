using System;
using System.Collections.Generic;
using CryptoQuest.Battle.Commands;
using CryptoQuest.Character.Attributes;
using CryptoQuest.Character.Hero;
using CryptoQuest.Gameplay;
using CryptoQuest.Gameplay.Character;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Battle.Components
{
    /// <summary>
    /// To interact with Gameplay Ability System
    /// </summary>
    public class HeroBehaviour : MonoBehaviour, IEquipmentsProvider
    {
        public int Level { get; set; } = 1;
        public Origin.Information DetailsInfo => Spec.Unit.Origin.DetailInformation;
        public StatsDef Stats => Spec.Unit.Stats;
        public Elemental Element => Spec.Unit.Element;
        public GameObject GameObject => gameObject;
        public CharacterClass Class => Spec.Unit.Class;
        public Sprite Avatar { get; set; }
        public Sprite BattleAvatar { get; set; }

        private Character _characterComponent;
        public AttributeSystemBehaviour AttributeSystem => _characterComponent.AttributeSystem;

        [SerializeField] private HeroSpec _spec;

        public HeroSpec Spec
        {
            get => _spec;
            set => _spec = value;
        }

        private LevelSystem _levelSystem;
        private TagSystemBehaviour _tagSystem;
        private readonly Dictionary<Type, Component> _cachedComponents = new();

        private void Awake()
        {
            _characterComponent = GetComponent<Character>();
            _levelSystem = GetComponent<LevelSystem>();
            _tagSystem = GetComponent<TagSystemBehaviour>();
        }

        /// <summary>
        /// 1. get lvl first
        /// 2. get stats from current lvl using UnitSO
        /// 3. apply stats to the character
        /// 4. equip items (weapons, armors, accessories) to get stats
        /// 5. active passive skill from items
        /// 6. equip gems to get stats
        /// 7. apply traits/personality to get stats
        /// </summary>
        /// <param name="character"></param>
        public void Init(HeroSpec character)
        {
            Spec = character;
            _levelSystem.Init(this);
            // Need level before I can init the character
            _characterComponent.Init(Element);
        }

        public bool IsValid() => Spec.IsValid();
        public Equipments GetEquipments() => Spec.Equipments;

        public bool HasTag(TagScriptableObject tagSO) => _characterComponent.HasTag(tagSO);

        public void SetCommand(ICommand command) => _characterComponent.SetCommand(command);
        
        /// <summary>
        /// Same as Unity's <see cref="GameObject.TryGetComponent{T}(out T)"/> but with a cache
        /// </summary>
        public new bool TryGetComponent<T>(out T component) where T : Component
        {
            var type = typeof(T);
            if (!_cachedComponents.TryGetValue(type, out var value))
            {
                if (base.TryGetComponent(out component))
                    _cachedComponents.Add(type, component);

                return component != null;
            }

            component = (T)value;
            return true;
        }

        public void RequestAddExp(float exp)
        {
            // TODO: Request to server here
            _spec.Experience += exp;
        }
    }
}