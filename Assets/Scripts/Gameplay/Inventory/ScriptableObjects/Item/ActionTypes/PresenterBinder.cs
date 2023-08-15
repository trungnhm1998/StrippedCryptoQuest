using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.ActionTypes
{
    /// <summary>
    /// Act as a bridge between the UI and the action.
    /// I followed the MVVP pattern here, but with a little twist. this class decorated the real UI.
    /// Now we can "interact" with the UI through this class.
    /// </summary>
    [CreateAssetMenu(menuName = "Create UIActionProviderSO", fileName = "UIActionProviderSO", order = 0)]
    public class PresenterBinder : ScriptableObject, IActionPresenter
    {
        private IActionPresenter _presenter;
        public IActionPresenter Presenter => _presenter;

        public void Bind(IActionPresenter presenter)
        {
            _presenter = presenter;
        }

        public void Show()
        {
            _presenter.Show();
        }

        public void Hide()
        {
            _presenter.Hide();
        }

        public void Execute()
        {
            _presenter.Execute();
        }
    }
}