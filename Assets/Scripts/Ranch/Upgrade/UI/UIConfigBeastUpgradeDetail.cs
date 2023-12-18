using CryptoQuest.Beast;
using TMPro;
using UnityEngine;

namespace CryptoQuest.Ranch.Upgrade.UI
{
    public class UIConfigBeastUpgradeDetail : MonoBehaviour
    {
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private TMP_Text _costText;
        [SerializeField] private Color _validColor;
        [SerializeField] private Color _inValidColor;

        public void SetupUI(int levelToUpgrade, float cost, bool isValid)
        {
            _levelText.text = levelToUpgrade.ToString();
            _costText.text = $"{cost} G";
            _costText.color = isValid ? _validColor : _inValidColor;
        }
    }
}