using CryptoQuest.Battle.Commands;
using UnityEngine;

namespace CryptoQuest.Battle.Components.EnemyCommandSelector
{
    public interface IEnemyCommandSelector
    {
        void SelectCommand(EnemyBehaviour enemy);
    }

    public class EnemyCommandSelector : IEnemyCommandSelector
    {
        /// <summary>
        /// Check Enemy Move section in this link for details
        /// https://docs.google.com/spreadsheets/d/19vLix05OIiWNojw8Gr2Nr3w1wbz-Gc88_q7Y-BXyY8M/edit#gid=333504180&range=A39
        /// </summary>
        /// <param name="enemy"></param>
        /// <returns></returns>
        public void SelectCommand(EnemyBehaviour enemy)
        {
            if (!enemy.IsValid()) return;
            if (!enemy.TryGetComponent<CommandExecutor>(out var commandExecutor))
                return;

            var _enemyDef = enemy.Def;
            var randomedValue = Random.Range(0f, 1f);
            var normalAttackProbability = _enemyDef.NormalAttackProbability;
            if (randomedValue < normalAttackProbability)
            {
                commandExecutor.SetCommand(
                    new NormalAttackCommand(enemy, enemy.Targeting.Target));
                return;
            }
            randomedValue -= normalAttackProbability;

            foreach (var skill in _enemyDef.Skills)
            {
                if (randomedValue < skill.Probability)
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
    }
}