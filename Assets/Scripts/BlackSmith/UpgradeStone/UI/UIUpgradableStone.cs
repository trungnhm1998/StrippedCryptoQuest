using System;
using CryptoQuest.Item.MagicStone;
using CryptoQuest.UI.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.BlackSmith.UpgradeStone.UI
{
    public class UIUpgradableStone : MonoBehaviour
    {
        public event Action<UIUpgradableStone> Pressed;
        [field: SerializeField] public Button Button { get; private set; }
        [SerializeField] private Image _icon;
        [SerializeField] private LocalizeStringEvent _name;
        [SerializeField] private TMP_Text _lvlText;
        [SerializeField] private GameObject _materialTag;
        public IMagicStone MagicStone { get; private set; }

        public int Id { get; private set; }

        public void Initialize(IMagicStone magicStone)
        {
            ResetItemStates();
            MagicStone = magicStone;
            Id = magicStone.ID;
            _icon.LoadSpriteAndSet(magicStone.Definition.Image);
            _name.StringReference = magicStone.Definition.DisplayName;
            _lvlText.text = $"Lv.{magicStone.Level}";
        }

        public void OnPressed()
        {
            if (_materialTag.activeSelf) return;
            Pressed?.Invoke(this);
        }

        public void ResetItemStates()
        {
            _materialTag.SetActive(false);
            Button.interactable = true;
        }
    }
}