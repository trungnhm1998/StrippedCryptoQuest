using System.Collections;
using CryptoQuest.Battle.Components;

namespace CryptoQuest.Battle.Commands
{
    public class GuardCommand : ICommand
    {
        private readonly HeroBehaviour _hero;

        public GuardCommand(HeroBehaviour hero)
        {
            _hero = hero;
        }

        public void Execute()
        {
            _hero.GetComponent<GuardBehaviour>().GuardUntilEndOfTurn();
        }
    }
}