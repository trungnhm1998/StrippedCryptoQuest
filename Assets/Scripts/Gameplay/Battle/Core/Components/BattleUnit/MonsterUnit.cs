using System.Collections;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit.AbilitySelectStrategies;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit.TargetPlayerStrategies;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit
{
    public class MonsterUnit : CharacterUnit
    {
        [field: SerializeField]
        public MonsterTargetPlayerWeightConfig TargetWeightConfig { get; private set; }

        private IAbilitySelector _abilitySelector;
        private IPlayerHeroSelector _playerHeroSelector;

        protected void Awake()
        {
            _abilitySelector = new RandomAbilitySelector();
            _playerHeroSelector = new WeightPlayerSelector();
        }

        public override IEnumerator Prepare()
        {
            // Select single target first
            // then if ability target all ally or all enemy the target will be overrided
            // ? Is there any case that enemy target only one enemy?
            UnitLogic.SelectTargets(_playerHeroSelector.GetTarget(this));
            SelectAbility(_abilitySelector.GetAbility(this));
            yield return base.Prepare();
        }
        
        public override void OnDeath()
        {
            base.OnDeath();
            OwnerTeam.Members.Remove(Owner);
            Destroy(gameObject);
        }
    }
}