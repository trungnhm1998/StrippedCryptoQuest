namespace CryptoQuest.Battle.ResultStates
{
    public interface IResultState : IState
    {
        EBattleResult Result { get; }
        void RaiseSetEvent();
    }
}