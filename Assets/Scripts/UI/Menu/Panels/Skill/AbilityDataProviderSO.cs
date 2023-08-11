using System.Collections.Generic;
using CryptoQuest.Gameplay.Skill;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Panels.Skill
{
    [CreateAssetMenu(menuName = "Create AbilityDataProviderSO", fileName = "AbilityDataProviderSO", order = 0)]
    public class AbilityDataProviderSO : ScriptableObject, IAbilityDataProvider
    {
        public IAbilityDataProvider RealDataProvider;
        public List<SkillInformation> GetAllAbility()
        {
            return RealDataProvider.GetAllAbility();
        }
    }
}