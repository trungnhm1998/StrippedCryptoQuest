using System.Collections.Generic;
using CryptoQuest.Gameplay.Skill;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Panels.Skill
{
    public class MockAbilityDataProvider : MonoBehaviour, IAbilityDataProvider
    {
        [SerializeField] private AbilityDataProviderSO _providerBus;
        [SerializeField] private List<SkillInformation> _mockData;

        private void Awake()
        {
            _providerBus.RealDataProvider = this;
        }

        public List<SkillInformation> GetAllAbility()
        {
            return _mockData;
        }
    }
}