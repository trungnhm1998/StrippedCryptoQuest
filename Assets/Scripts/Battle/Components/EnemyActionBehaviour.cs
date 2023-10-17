using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Battle.Commands;
using CryptoQuest.Battle.Components.EnemyCommandSelector;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.System;
using UnityEngine;

namespace CryptoQuest.Battle.Components
{
    /// <summary>
    /// Behaviour of enemy in action phase (select whether using normal attack or cast skill)
    /// </summary>
    [RequireComponent(typeof(EnemyBehaviour))]
    [RequireComponent(typeof(CommandExecutor))]
    public class EnemyActionBehaviour : CharacterComponentBase
    {
        [SerializeField] private EnemyBehaviour _enemyBehaviour;

        [Tooltip("Since this is enemy behaviour you should drag AllEnemies/OneEnemy here because enemy of enemy is hero")]
        [SerializeField] private SkillTargetType _singleHeroChannel;
        [SerializeField] private SkillTargetType _allHeroChannel;
        private CommandExecutor _commandExecutor;
        private IPartyController _heroParty;

        private IEnemyCommandSelector _commandSelector
            = new EnemyCommandSelector.EnemyCommandSelector();

        private void OnValidate()
        {
            _enemyBehaviour = GetComponent<EnemyBehaviour>();
        }

        public override void Init()
        {
            _enemyBehaviour.TryGetComponent<CommandExecutor>(out _commandExecutor);
            _enemyBehaviour.PreTurnStarted += SelectCommand;
            _heroParty ??= ServiceProvider.GetService<IPartyController>();
        }

        protected override void OnReset()
        {
            _enemyBehaviour.PreTurnStarted -= SelectCommand;
            UnRegistSkillEvent();
        }

        private void SelectCommand()
        {
            // I have to setup event here to prevent listening to hero select skill event
            // Because it use the same event SO
            RegistSkillEvent();
            _commandSelector?.SelectCommand(_enemyBehaviour);
            UnRegistSkillEvent();
        }

        private void RegistSkillEvent()
        {
            // TODO: listening to target all enemy event if there's needed case
            // but I have to get enemy party behaviour somehow and the enemy target enemy case is really complicated 
            _allHeroChannel.EventRaised += TargetAllHero;
            _singleHeroChannel.EventRaised += TargetSingleHero;
        }

        private void UnRegistSkillEvent()
        {
            _allHeroChannel.EventRaised -= TargetAllHero;
            _singleHeroChannel.EventRaised -= TargetSingleHero;
        }

        private void TargetAllHero(CastSkillAbility skill)
        {
            var heroes = _heroParty.OrderedAliveMembers.ToArray();
            var castSkillCommand = new MultipleTargetCastSkillCommand(_enemyBehaviour, skill, heroes);
            _commandExecutor.SetCommand(castSkillCommand);
        }

        private void TargetSingleHero(CastSkillAbility skill)
        {
            var castSkillCommand = new CastSkillCommand(_enemyBehaviour, skill, _enemyBehaviour.Targeting.Target);
            _commandExecutor.SetCommand(castSkillCommand);
        }
    }
}