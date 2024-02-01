using CryptoQuest.Item.MagicStone;
using CryptoQuest.UI.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.Menus.Status.UI.MagicStone
{
    public class UISingleStone : MonoBehaviour
    {
        public event UnityAction<IMagicStone> Pressed;
        [SerializeField] private Image _icon;
        [SerializeField] private LocalizeStringEvent _localizedName;
        [SerializeField] private TMP_Text _level;

        private IMagicStone _stoneData;
        public IMagicStone Data => _stoneData;

        public void SetInfo(IMagicStone stoneData)
        {
            _stoneData = stoneData;
            _icon.LoadSpriteAndSet(stoneData.Definition.Image);
            _localizedName.StringReference = stoneData.Definition.DisplayName;
            _localizedName.RefreshString();
            _level.text = $"Lv{stoneData.Level.ToString()}";
        }

        public void Reset()
        {
            _stoneData = null;
        }


        public void OnPressed() => Pressed?.Invoke(_stoneData);
    }
}