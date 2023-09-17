namespace CryptoQuest.Battle.Commands
{
    public class NormalAttackCommand : ICommand
    {
        private readonly ICharacter _attacker;
        private readonly ICharacter _target;
        
        public NormalAttackCommand(ICharacter attacker, ICharacter target)
        {
            _attacker = attacker;
            _target = target;
        }

        public void Execute()
        {
            _attacker.Attack(_target);
        }
    }
}