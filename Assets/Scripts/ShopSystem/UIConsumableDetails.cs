using CryptoQuest.Item.Consumable;
using CryptoQuest.UI.Extensions;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.ShopSystem
{
    public class UIConsumableDetails : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private LocalizeStringEvent _name;
        [SerializeField] private LocalizeStringEvent _description;

        public void SetupUI(ConsumableInfo consumable)
        {
            _icon.LoadSpriteAndSet(consumable.Icon);
            _name.StringReference = consumable.DisplayName;
            _description.StringReference = consumable.Description;
        }
    }
}