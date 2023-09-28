using CryptoQuest.Character.Attributes;
using CryptoQuest.Character.Hero;
using CryptoQuest.Gameplay;
using CryptoQuest.Gameplay.Character;
using UnityEngine;

namespace CryptoQuest.Battle.Components
{
    /// <summary>
    /// To interact with Gameplay Ability System
    /// </summary>
    public class HeroBehaviour : MonoBehaviour, IEquipmentsProvider
    {
        public int Level => 1;
        public Origin.Information DetailsInfo => Spec.Unit.Origin.DetailInformation;
        public StatsDef Stats => Spec.Unit.Stats;
        public Elemental Element => Spec.Unit.Element;
        public GameObject GameObject => gameObject;
        public CharacterClass Class => Spec.Unit.Class;
        public Sprite Avatar { get; set; }
        public Sprite BattleAvatar { get; set; }

        private Character _characterComponent;
        [SerializeField] private HeroSpec _spec;
        public HeroSpec Spec { get => _spec; set => _spec = value; }
        private LevelSystem _levelSystem;

        private void Awake()
        {
            _characterComponent = GetComponent<Character>();
            _levelSystem = GetComponent<LevelSystem>();
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
            // Need level before I can init the character
            _characterComponent.Init(Element);
        }

        public bool IsValid() => Spec.IsValid();
        public Equipments GetEquipments() => Spec.Equipments;
    }
}