using CryptoQuest.ChangeClass.API;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.ChangeClass.View
{
    public class UIItemMaterial : MonoBehaviour
    {
        [SerializeField] private LocalizeStringEvent _displayName;
        [SerializeField] private TextMeshProUGUI _materialNumber;
        [SerializeField] private Image _icon;
        [SerializeField] private GameObject _defaultBackground;
        public bool IsValid { get; private set; }

        public void ConfigureCell(WalletMaterialAPI material, int quantity, int index)
        {
            var data = material.Data[index];
            _materialNumber.text = $"{quantity}/{data.materialNum}";
            IsValid = material.Data[index].materialNum > 0;
            EnableDefaultBackground(IsValid);
        }

        public void SetLocalization(LocalizedString localized)
        {
            _displayName.StringReference = localized;
        }

        private void EnableDefaultBackground(bool isEnable)
        {
            _defaultBackground.SetActive(isEnable);
        }
    }
}
