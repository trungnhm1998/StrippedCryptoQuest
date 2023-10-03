using System.Collections;
using CryptoQuest.Battle.Components;
using UnityEngine;

namespace CryptoQuest.Battle.Commands
{
    public class RetreatCommand : ICommand
    {
        private readonly RetreatBehaviour _retreatBehaviour;
        private readonly float _highestEnemySpeed;

        public RetreatCommand(HeroBehaviour hero, float highestEnemySpeed)
        {
            _retreatBehaviour = hero.GetComponent<RetreatBehaviour>();
            _highestEnemySpeed = highestEnemySpeed;
        }

        public IEnumerator Execute()
        {
            _retreatBehaviour.Retreat(_highestEnemySpeed);
            yield break;
        }
    }
}