using UnityEngine;

namespace CryptoQuest.ShopSystem.Buy.Consumable
{
    public class BuyConsumablePresenter : MonoBehaviour
    {
        [SerializeField] private BuyConsumableModel _model;
        [SerializeField] private UIBuyConsumableList _uiBuyableList;

        private void OnEnable()
        {
            _uiBuyableList.BuyingConsumable += BuyConsumable;
            _uiBuyableList.Render(_model.SellingConsumables);
        }

        private void OnDisable()
        {
            _uiBuyableList.BuyingConsumable -= BuyConsumable;
        }

        private void BuyConsumable(UIConsumableShopItem item) { }
    }
}