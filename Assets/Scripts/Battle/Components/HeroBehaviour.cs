using CryptoQuest.Character;
using CryptoQuest.Character.Hero;
using CryptoQuest.Gameplay;
using CryptoQuest.Gameplay.PlayerParty;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Battle.Components
{
    public interface IStatsConfigProvider
    {
        public StatsDef Stats { get; }
    }

    public interface IExpProvider
    {
        public float Exp { get; set; }
    }

    /// <summary>
    /// To interact with Gameplay Ability System
    /// </summary>
    [DisallowMultipleComponent]
    public class HeroBehaviour :
        Character,
        IEquipmentsProvider, 
        IExpProvider, 
        IStatsConfigProvider
    {
        public Origin.Information DetailsInfo => Spec.Origin.DetailInformation;
        public StatsDef Stats => Spec.Stats;
        public override string DisplayName => DetailsInfo.LocalizedName.GetLocalizedString();
        public override LocalizedString LocalizedName => DetailsInfo.LocalizedName;
        public GameObject GameObject => gameObject;
        public CharacterClass Class => Spec.Class;
        public Sprite Avatar { get; set; }
        public Sprite BattleAvatar { get; set; }

        public float Exp
        {
            get
            {
                if (_spec.IsValid() == false) return 0;
                return _spec.Experience;
            }
            set
            {
                if (_spec.IsValid() == false) return;
                _spec.Experience = value;
            }
        }

        [SerializeField] private HeroSpec _spec;

        public HeroSpec Spec
        {
            get => _spec;
            set => _spec = value;
        }

        private PartySlotSpec _partySlotSpec = new();

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
            Init(Spec.Elemental);
        }

        public override bool IsValid() => _spec.IsValid();

        public Equipments GetEquipments() => _partySlotSpec.EquippingItems;
    }
}