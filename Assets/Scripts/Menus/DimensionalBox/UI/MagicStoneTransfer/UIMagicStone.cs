using System;
using CryptoQuest.Sagas.Objects;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.Menus.DimensionalBox.UI.MagicStoneTransfer
{
    public class UIMagicStone : MonoBehaviour
    {
        public event Action<UIMagicStone> Pressed;
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private LocalizeStringEvent _name;
        [SerializeField] private GameObject _pendingTag;
        [SerializeField] private GameObject _equippedTag;
        public MagicStone MagicStone { get; private set; }
        public GameObject EquippedTag => _equippedTag;

        public int Id { get; private set; }

        public void Initialize(MagicStone magicStone)
        {
            MagicStone = magicStone;
            Id = magicStone.id;
            _nameText.text = "Item " + magicStone.id;
        }

        public void OnPressed()
        {
            if (_equippedTag.activeSelf) return;
            Pressed?.Invoke(this);
        }

        public void EnablePendingTag(bool enabling) => _pendingTag.SetActive(enabling);
    }
}