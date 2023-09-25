namespace CryptoQuest.Quest.Authoring
{
    public interface IObjective
    {
        public void OnComplete();
        public void OnProgressChange();
        public void SubscribeObjective();
        public void UnsubscribeObjective();
    }
}