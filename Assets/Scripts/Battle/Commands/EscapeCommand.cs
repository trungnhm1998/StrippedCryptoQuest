using CryptoQuest.Battle.Components;
using UnityEngine;

namespace CryptoQuest.Battle.Commands
{
    public class EscapeCommand : ICommand
    {
        private readonly EscapeBehaviour _escapeBehaviour;
        private readonly float _highestEnemySpeed;

        public EscapeCommand(GameObject heroGo, float highestEnemySpeed)
        {
            _escapeBehaviour = heroGo.GetComponent<EscapeBehaviour>();
            _highestEnemySpeed = highestEnemySpeed;
        }

        public void Execute()
        {
            _escapeBehaviour.Escape(_highestEnemySpeed);
        }
    }
}