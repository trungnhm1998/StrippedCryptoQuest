using CryptoQuest.Core;
using CryptoQuest.UI.Actions;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.UI.Core
{
    public class LoadingController : MonoBehaviour
    {
        [SerializeField] private GameObject _loadingPanel;
        private TinyMessageSubscriptionToken _showLoadingEvent;

        private void OnEnable() => _showLoadingEvent = ActionDispatcher.Bind<ShowLoading>(EnableLoadingUI);

        private void OnDisable() => ActionDispatcher.Unbind(_showLoadingEvent);

        
        private void EnableLoadingUI(ShowLoading ctx)
        {
            _loadingPanel.gameObject.SetActive(ctx.IsShown);
        }
    }
}