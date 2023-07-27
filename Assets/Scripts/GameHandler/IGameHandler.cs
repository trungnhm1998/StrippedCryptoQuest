namespace CryptoQuest.GameHandler
{
    public interface IGameHandler<T>
    {
        IGameHandler<T> NextHandler { get; }
        void Handle(T request);
        void Handle();
        IGameHandler<T> SetNext(IGameHandler<T> nextHandler);
    }
}