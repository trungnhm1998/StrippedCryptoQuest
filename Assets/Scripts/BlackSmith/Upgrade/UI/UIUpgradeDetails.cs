using TMPro;
using UnityEngine;

namespace CryptoQuest.BlackSmith.Upgrade.UI
{
    public class UIUpgradeDetails : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _costText;
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