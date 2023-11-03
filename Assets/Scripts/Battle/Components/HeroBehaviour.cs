using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.Character;
using CryptoQuest.Character.Hero;
using CryptoQuest.Gameplay;
using CryptoQuest.Gameplay.PlayerParty;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Battle.Components
{
    /// <summary>
    /// To interact with Gameplay Ability System
    /// </summary>
    [DisallowMultipleComponent]
    public class HeroBehaviour : Character, IEquipmentsProvider
    {
        public int Level { get; set; } = 1;
        public Origin.Information DetailsInfo => Spec.Unit.Origin.DetailInformation;
        public StatsDef Stats => Spec.Unit.Stats;
        public Elemental Element => Spec.Unit.Element;
        public override string DisplayName => DetailsInfo.LocalizedName.GetLocalizedString();
        public override LocalizedString LocalizedName => DetailsInfo.LocalizedName;
        public GameObject GameObject => gameObject;
        public CharacterClass Class => Spec.Unit.Class;
        public Sprite Avatar { get; set; }
        public Sprite BattleAvatar { get; set; }

        [SerializeField] private HeroSpec _spec;

        public HeroSpec Spec
        {
            get => _spec;
            set => _spec = value;
        }

        private LevelSystem _levelSystem;
        private PartySlotSpec _partySlotSpec;

        protected override void Awake()
        {
            base.Awake();
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
        /// <param name="slotSpec"></param>
        public void Init(PartySlotSpec slotSpec)
        {
            _partySlotSpec = slotSpec;
            Spec = _partySlotSpec.Hero;
            _levelSystem.Init(this);
            // Need level before I can init the character
            Init(Element);
        }

        public override bool IsValid() => _spec.IsValid();

        public Equipments GetEquipments() => _partySlotSpec.EquippingItems;

        public void RequestAddExp(float exp)
        {
            // TODO: Request to server here
            _spec.Experience += exp;
        }
    }
}