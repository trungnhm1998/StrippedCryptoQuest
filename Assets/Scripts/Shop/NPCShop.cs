using CryptoQuest.Shop.UI;
using CryptoQuest.Shop.UI.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Shop
{
    public class NPCShop : MonoBehaviour
    {
        [Header("Raise Event")]
        [SerializeField] private ShowShopEventChannelSO _showShopEvent;

        [Header("NPC Shop Config")]
        [SerializeField] private ShopInfo _shopInfo;

        public void Interact()
        {
            if (_shopInfo == null)
            {
                Debug.Log("NPCShop::Interact Show shop fail due to shop info was null!");
                return;
            }
            _showShopEvent?.RaiseEvent(_shopInfo);
        }
    }
}
