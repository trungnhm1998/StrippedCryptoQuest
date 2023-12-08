using System;
using CryptoQuest.Item.MagicStone;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.BlackSmith.UpgradeStoneUI.UI
{
    public class UIUpgradableStone : MonoBehaviour
    {
        public event Action<UIUpgradableStone> Pressed;
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private LocalizeStringEvent _name;
        [SerializeField] private GameObject _pendingTag;
        [SerializeField] private GameObject _equippedTag;
        public IMagicStone MagicStone { get; private set; }

        public int Id { get; private set; }

        public void Initialize(IMagicStone magicStone)
        {
            MagicStone = magicStone;
            Id = magicStone.ID;
            _nameText.text = "Item " + magicStone.ID;
        }

        public void OnPressed()
        {
            if (_equippedTag.activeSelf) return;
            Pressed?.Invoke(this);
        }

        public void EnablePendingTag(bool enabling) => _pendingTag.SetActive(enabling);
    }
}