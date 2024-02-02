using System;
using System.Collections;
using CryptoQuest.Character.Hero;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Quest;
using CryptoQuest.Quest.Authoring;
using UnityEngine;

namespace CryptoQuest.System.SaveSystem.Loaders
{
    [Serializable]
    public class MainHeroClassLoader : Loader
    {
        [SerializeField] private QuestSaveSO _questSaveSO;
        [SerializeField] private QuestSO _questToCheck;
        [SerializeField] private PartySO _party;

        [SerializeField, Space] private UnitSO _defaultMainClass;
        [SerializeField] private UnitSO _trueBraveHeartUnit;

        public override IEnumerator LoadAsync()
        {
            UpdateMainCharacterClass(_defaultMainClass);
            foreach (var completedQuest in _questSaveSO.CompletedQuests)
            {
                if (completedQuest.Equals(_questToCheck.Guid) == false) continue;
                UpdateMainCharacterClass(_trueBraveHeartUnit);
                yield break;
            }
        }

        private void UpdateMainCharacterClass(UnitSO unit)
        {
            var partySlotSpecs = _party.GetParty();
            for (var index = 0; index < partySlotSpecs.Length; index++)
            {
                var partySlotSpec = partySlotSpecs[index];
                var heroOrigin = partySlotSpec.Hero.Origin;
                if (heroOrigin != null && heroOrigin.DetailInformation.Id != 0) continue;
                var heroSpec = partySlotSpec.Hero;
                SyncStats(heroSpec, unit);
                heroSpec.Class = unit.Class;
                heroSpec.Elemental = unit.Element;
                heroSpec.Origin = unit.Origin;
                _party.GetParty()[index].Hero = heroSpec;
                return;
            }
        }

        private void SyncStats(HeroSpec hero, UnitSO unit)
        {
            // Only sync min max and perseve the random and modify value
            for (var index = 0; index < hero.Stats.Attributes.Length; index++)
            {
                var unitAttribute = unit.Stats.Attributes;
                var attribute = hero.Stats.Attributes[index];
                attribute.MinValue = unitAttribute[index].MinValue;
                attribute.MaxValue = unitAttribute[index].MaxValue;
                attribute.RandomValue = unitAttribute[index].RandomValue;
            }
        }
    }
}