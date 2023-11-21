using System.Collections.Generic;
using CryptoQuest.Battle.Commands;
using CryptoQuest.Character.Enemy;
using UnityEngine;

namespace CryptoQuest.Battle.Components.EnemyComponents
{
    public interface IEnemySkillHolder
    {
        List<Skills> Skills { get; }
    }

    public class EnemyCommandSelector : CharacterComponentBase, IEnemySkillHolder
    {
        [SerializeField] private EnemyBehaviour _enemyBehaviour;
        public List<Skills> Skills { get; private set; } = new();

        private EnemyDef _enemyDef;
        private float _normalAttackProbability = 1f;
        
        private void OnValidate()
        {
            _enemyBehaviour = GetComponent<EnemyBehaviour>();
        }

        public override void Init()
        {
            _enemyBehaviour.PreTurnStarted += SelectCommand;
            _enemyDef = _enemyBehaviour.Def;
            _normalAttackProbability = _enemyDef.NormalAttackProbability;
            Skills.AddRange(_enemyDef.Skills);
        }

        protected override void OnReset()
        {
            _enemyBehaviour.PreTurnStarted -= SelectCommand;
        }

        /// <summary>
        /// Check Enemy Move section in this link for details
        /// https://docs.google.com/spreadsheets/d/19vLix05OIiWNojw8Gr2Nr3w1wbz-Gc88_q7Y-BXyY8M/edit#gid=333504180&range=A39
        /// </summary>
        /// <param name="enemy"></param>
        /// <returns></returns>
        private void SelectCommand()
        {
            if (!_enemyBehaviour.IsValid()) return;

            var randomedValue = Random.value;
            
            if (TryNormalAttack(ref randomedValue)) return;

            foreach (var skill in Skills)
            {
                if (randomedValue <= skill.Probability)
                {
                    // Since enemy can multiple target I have to raise event here to create command
                    skill.SkillDef.TargetType.RaiseEvent(skill.SkillDef);
                    return;
                }
                randomedValue -= skill.Probability;
            }

            Debug.LogWarning("Enemy can't choose command." +
                "There's something wrong with enemy probability setup.");
            return;
        }

        private bool TryNormalAttack(ref float randomedValue)
        {
            if (!_enemyBehaviour.TryGetComponent<CommandExecutor>(out var commandExecutor))
                return false;

            if (randomedValue <= _normalAttackProbability)
            {
                commandExecutor.SetCommand(
                    new NormalAttackCommand(_enemyBehaviour, _enemyBehaviour.Targeting.Target));
                return true;
            }

            randomedValue -= _normalAttackProbability;
            return false;
        }

#if UNITY_EDITOR || DEVELOPMENT_BUILD
        public void Editor_SetNormalAttackProbability(float value)
        {
            _normalAttackProbability = value;
        }
#endif
    }
}