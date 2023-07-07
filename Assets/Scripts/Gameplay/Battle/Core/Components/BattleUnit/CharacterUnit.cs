using UnityEngine;
using System.Collections;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;

namespace CryptoQuest.Gameplay.Battle
{
    public class CharacterUnit : BattleUnitBase
    {
        protected string _originalName;

        protected virtual void Start()
        {
            var statsInitializer = GetComponent<StatsInitializer>();
            var data = statsInitializer.DefaultStats as CharacterDataSO;
            if (data == null) return;
            _originalName = data.Name;
        }

        public override string GetOriginalName()
        {
            return _originalName;
        }
    }
}