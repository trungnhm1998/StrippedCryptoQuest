using System.Collections;
using CryptoQuest.Item.MagicStone;
using CryptoQuest.Sagas.MagicStone;
using IndiGames.Core.Common;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
using CryptoQuest.Menus.Status.UI.MagicStone;
using MagicStone = CryptoQuest.Sagas.Objects.MagicStone;

namespace CryptoQuest.Menus.DimensionalBox.UI.MagicStoneTransfer
{
    public class UIMagicStone : MonoBehaviour, ITooltipStoneProvider
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private LocalizeStringEvent _name;
        [SerializeField] private Button _button;
        [SerializeField] private GameObject _pendingTag;
        [SerializeField] private GameObject _equippedTag;
        [SerializeField] private GameObject _transferringTag;
        private IMagicStone _magicStone;
        public IMagicStone MagicStone => _magicStone;
        public MagicStone Respone { get; private set; }
        public int Id => Respone.id;

        private void OnDisable() => _magicStone = NullMagicStone.Instance;

        public bool MarkedForTransfer
        {
            get => _pendingTag.activeSelf;
            set => _pendingTag.SetActive(value);
        }

        public void Initialize(MagicStone magicStone)
        {
            bool isTransferring = magicStone.transferring == 1;
            bool isEquipped = magicStone.attachEquipment != 0;

            _magicStone = NullMagicStone.Instance;
            MarkedForTransfer = false;
            Respone = magicStone;
            _nameText.text = $"{Id}.Stone";
            _transferringTag.SetActive(isTransferring);
            _equippedTag.SetActive(isEquipped);
            _magicStone = ServiceProvider.GetService<IMagicStoneResponseConverter>().Convert(magicStone);
            StartCoroutine(SetLocalizedName());
        }

        private IEnumerator SetLocalizedName()
        {
            yield return new WaitUntil(() => _magicStone.Definition.DisplayName != null);
            _name.StringReference = _magicStone.Definition.DisplayName;
        }

        public void OnSelectToTransfer()
        {
            if (_equippedTag.activeSelf || _transferringTag.activeSelf) return;
            MarkedForTransfer = !MarkedForTransfer;
        }
    }
}