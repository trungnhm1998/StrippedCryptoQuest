using CryptoQuest.Character.Attributes;
using CryptoQuest.Character.Hero;
using CryptoQuest.Gameplay;
using CryptoQuest.Gameplay.Character;
using CryptoQuest.UI.Menu.Panels.Home;
using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using UnityEngine;

namespace CryptoQuest.Battle.Components
{
    /// <summary>
    /// To interact with Gameplay Ability System
    /// </summary>
    public class HeroBehaviour : MonoBehaviour
    {
        public int Level => 1;
        public Origin.Information DetailsInfo => _spec.Unit.Origin.DetailInformation;
        public StatsDef Stats => _spec.Unit.Stats;
        public Elemental Element => _spec.Unit.Element;
        public GameObject GameObject => gameObject;
        public CharacterClass Class => _spec.Unit.Class;

        private ICharacter _characterComponent;
        private HeroSpec _spec;
        public HeroSpec Spec => _spec;
        private LevelSystem _levelSystem;

        private void Awake()
        {
            _characterComponent = GetComponent<ICharacter>();
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
            _spec = character;
            // Need level before I can init the character
            _characterComponent.Init(Element);
        }


        public void SetupUI(ICharacterInfo uiCharacterInfo)
        {
            uiCharacterInfo.SetElement(Element.Icon);
            uiCharacterInfo.SetLevel(Level);
            // uiCharacterInfo.SetClass(Class.Name);
            // uiCharacterInfo.SetAvatar(Avatar);
            // BackgroundInfo.SetupUI(uiCharacterInfo);
            SetupExpUI(uiCharacterInfo);
        }

        private void SetupExpUI(ICharacterInfo uiCharacterInfo)
        {
            var levelCalculator = LevelSystem.LevelCalculator;
            if (levelCalculator == null) return;

            // var cachedLevel = Level;
            // var isMaxedLevel = cachedLevel == StatsDef.MaxLevel;
            // var nextLevelRequireExp = levelCalculator.RequiredExps[isMaxedLevel ? cachedLevel - 1 : cachedLevel];
            // uiCharacterInfo.SetMaxExp(nextLevelRequireExp);
            //
            // var currentLevelAccumulateExp = levelCalculator.AccumulatedExps[cachedLevel - 1];
            // var currentExp = isMaxedLevel ? nextLevelRequireExp : Experience - currentLevelAccumulateExp;
            // uiCharacterInfo.SetExp(currentExp);
        }

        public bool IsValid()
        {
            return _spec != null;
        }
    }
}