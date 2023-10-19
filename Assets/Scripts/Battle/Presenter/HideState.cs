namespace CryptoQuest.Battle.Presenter
{
    public class HideState : StateBase
    {
        protected override void OnEnter() => LogPresenter.HideAndClear();
    }
}