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

        [SerializeField, Space] private Origin _mainHeroOrigin;
        [SerializeField] private UnitSO _defaultMainClass;
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
                if (!(partySlotSpec.Hero.Origin == _mainHeroOrigin || partySlotSpec.Hero.Id == 0)) continue;
                var heroSpec = partySlotSpec.Hero;
                heroSpec.Stats = unit.Stats;
                heroSpec.Class = unit.Class;
                heroSpec.Elemental = unit.Element;
                heroSpec.Origin = unit.Origin;
                _party.GetParty()[index].Hero = heroSpec;
                return;
            }
        }
    }
}