namespace CryptoQuest.Gameplay.Battle.Core.Components.Logger
{
    public interface ILogger
    {
        void Log(string message);
        void ClearLogs();
    }
}
