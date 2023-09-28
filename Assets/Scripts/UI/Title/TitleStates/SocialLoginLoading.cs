
namespace CryptoQuest.UI.Title.TitleStates
{
    public class SocialLoginLoading : IState
    {
        public void OnEnter()
        {
            LoadingController.OnEnableLoadingPanel?.Invoke();
        }

        public void OnExit()
        {
            LoadingController.OnDisableLoadingPanel?.Invoke();
        }
    }
}