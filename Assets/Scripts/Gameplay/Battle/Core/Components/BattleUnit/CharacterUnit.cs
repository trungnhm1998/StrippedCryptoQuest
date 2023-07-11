using UnityEngine;
using System.Collections;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;

namespace CryptoQuest.Gameplay.Battle
{
    public class CharacterUnit : BattleUnitBase
    {
        protected CharacterDataSO _characterData;
        
        public override CharacterDataSO GetUnitData()
        {
            _characterData = (_characterData != null) ? _characterData : GetCharacterData();
            return _characterData;
        }

        private CharacterDataSO GetCharacterData()
        {
            var statsInitializer = GetComponent<StatsInitializer>();
            if (statsInitializer.DefaultStats is CharacterDataSO data)
            {
                return data;
            }
            return null;
        }
    }
}