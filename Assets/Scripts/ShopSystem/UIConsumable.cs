using CryptoQuest.Item.Consumable;
using CryptoQuest.UI.Extensions;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.ShopSystem
{
    public class UIConsumable : MonoBehaviour
    {
        [SerializeField] private Image _imgIcon;
        [SerializeField] private LocalizeStringEvent _txtName;
        [SerializeField] private Text _txtQuantity;
        [SerializeField] private string _quantityFormant = "x{0}";

        public void Init(ConsumableInfo consumable)
        {
            _imgIcon.LoadSpriteAndSet(consumable.Icon);
            _txtName.StringReference = consumable.DisplayName;
            _txtQuantity.text = string.Format(_quantityFormant, consumable.Quantity);
        }
    }
}