using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Battle.Commands;
using CryptoQuest.Battle.Events;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.System;
using IndiGames.Core.Common;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.Battle.Components.EnemyComponents
{
    /// <summary>
    /// Behaviour of enemy in action phase (select whether using normal attack or cast skill)
    /// </summary>
    [RequireComponent(typeof(EnemyBehaviour))]
    [RequireComponent(typeof(CommandExecutor))]
    public class EnemyActionBehaviour : CharacterComponentBase
    {
        [SerializeField] private BattleBus _battleBus;
        [SerializeField] private EnemyBehaviour _enemyBehaviour;

        [Tooltip("Since this is enemy behaviour you should drag AllEnemies/OneEnemy here because enemy of enemy is hero")]
        [SerializeField] private SkillTargetType _singleHeroChannel;
        [SerializeField] private SkillTargetType _allHeroChannel;
        [SerializeField] private SkillTargetType _allAllyChannel;
        [SerializeField] private SkillTargetType _targetSelfChannel;
        private CommandExecutor _commandExecutor;
        private IPartyController _heroParty;
        private TinyMessageSubscriptionToken _roundStartedEvent;
        private TinyMessageSubscriptionToken _roundEndedEvent;

        private void OnValidate()
        {
            _enemyBehaviour = GetComponent<EnemyBehaviour>();
        }

        protected override void OnInit()
        {
            _enemyBehaviour.TryGetComponent<CommandExecutor>(out _commandExecutor);
            _heroParty ??= ServiceProvider.GetService<IPartyController>();
            
            _roundStartedEvent = BattleEventBus.SubscribeEvent<RoundStartedEvent>(_ => RegistSkillEvents());
            _roundEndedEvent = BattleEventBus.SubscribeEvent<RoundEndedEvent>(_ => UnRegistSkillEvents());
        }

        protected override void OnReset()
        {
            UnRegistSkillEvents();
            BattleEventBus.UnsubscribeEvent(_roundStartedEvent);
            BattleEventBus.UnsubscribeEvent(_roundEndedEvent);
        }

        private void RegistSkillEvents()
        {
            _allHeroChannel.EventRaised += TargetAllHero;
            _singleHeroChannel.EventRaised += TargetSingleHero;
            _allAllyChannel.EventRaised += TargetAllEnemy;
            _targetSelfChannel.EventRaised += TargetSelf;
        }

        private void UnRegistSkillEvents()
        {
            _allHeroChannel.EventRaised -= TargetAllHero;
            _singleHeroChannel.EventRaised -= TargetSingleHero;
            _allAllyChannel.EventRaised -= TargetAllEnemy;
            _targetSelfChannel.EventRaised -= TargetSelf;
        }

        private void TargetAllEnemy(CastSkillAbility skill)
        {
            var battleContext = _battleBus.CurrentBattleContext;
            var enemies = battleContext.Enemies.ToArray();
            var castSkillCommand = new MultipleTargetCastSkillCommand(_enemyBehaviour, skill, enemies);
            _commandExecutor.SetCommand(castSkillCommand);
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

        private void TargetSelf(CastSkillAbility skill)
        {
            var castSkillCommand = new CastSkillCommand(_enemyBehaviour, skill, _enemyBehaviour);
            _commandExecutor.SetCommand(castSkillCommand);
        }
    }
}