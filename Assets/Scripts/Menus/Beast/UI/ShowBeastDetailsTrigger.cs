using CryptoQuest.Beast;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CryptoQuest.Menus.Beast.UI
{
    public class ShowBeastDetailsTrigger : MonoBehaviour, ISelectHandler
    {
        [SerializeField] private BeastEventChannel _showBeastDetailsEventChannel;
        private IBeastProvider _provider;

        public void Initialize(IBeastProvider beastProvider) => _provider = beastProvider;

        private void Awake() => _provider = GetComponent<IBeastProvider>();

        public void OnSelect(BaseEventData eventData) => _showBeastDetailsEventChannel.RaiseEvent(_provider.Beast);
    }
}