namespace CryptoQuest.Battle.Components
{
    public interface IEvadable
    {
        bool TryEvade();
    }
        
    public class EvadeBehaviour : CharacterComponentBase, IEvadable
    {
        public override void Init()
        {
        }

        public bool TryEvade()
        {
            return false;
        }
    }
}