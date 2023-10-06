using System.Collections;
using CryptoQuest.Battle.Commands;
using UnityEngine;

namespace CryptoQuest.Battle.Components
{
    public class EnemyCommandExecutor : CommandExecutor
    {
        [SerializeField] private EnemyBehaviour _enemyBehaviour;

        protected override IEnumerator OnPreExecuteCommand()
        {
            Command = new NormalAttackCommand(Character, Character.Targeting.Target);
            _enemyBehaviour.SetAlpha(1);
            yield return base.OnPreExecuteCommand();
        }

        protected override IEnumerator OnPostExecuteCommand()
        {
            _enemyBehaviour.SetAlpha(0.5f);
            yield return base.OnPostExecuteCommand();
        }
    }
}