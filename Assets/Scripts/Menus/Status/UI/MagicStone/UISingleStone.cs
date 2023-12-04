using CryptoQuest.Item.MagicStone;
using CryptoQuest.UI.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.Menus.Status.UI.MagicStone
{
    public class UISingleStone : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private LocalizeStringEvent _localizedName;
        [SerializeField] private TMP_Text _level;

        public void SetInfo(IMagicStone stoneData)
        {
            _icon.LoadSpriteAndSet(stoneData.Definition.Image);
            _localizedName.StringReference = stoneData.Definition.DisplayName;
            _localizedName.RefreshString();
            _level.text = $"Lv{stoneData.Level.ToString()}";
        }
    }
}