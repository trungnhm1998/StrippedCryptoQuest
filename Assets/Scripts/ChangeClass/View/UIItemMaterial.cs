using TMPro;
using UnityEngine;
using UnityEngine.UI;
using CryptoQuest.ChangeClass.API;

namespace CryptoQuest
{
    public class UIItemMaterial : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _displayName;
        [SerializeField] private TextMeshProUGUI _materialNumber;
        [SerializeField] private Image _icon;
        [SerializeField] private GameObject _defaultBackground;

        public void ConfigureCell(WalletMaterialAPI material, int quantity, int index)
        {
            var data = material.Data[index];
            _displayName.text = data.nameEn;
            _materialNumber.text = $"{quantity}/{data.materialNum}";
            bool isEnable = material.Data[index].materialNum > 0;
            EnableDefaultBackground(isEnable);
        }

        private void EnableDefaultBackground(bool isEnable)
        {
            _defaultBackground.SetActive(isEnable);
        }
    }
}
