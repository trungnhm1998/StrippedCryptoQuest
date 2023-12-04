namespace CryptoQuest.Beast.Interface
{
    public interface IBeastPassiveController
    {
        public void ApplyPassive(IBeast beast);
        public void RemovePassive(IBeast beast);
    }
}