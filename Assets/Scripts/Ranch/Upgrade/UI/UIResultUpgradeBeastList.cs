using CryptoQuest.Beast;
using CryptoQuest.Beast.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Ranch.Upgrade.UI
{
    public class UIResultUpgradeBeastList : MonoBehaviour
    {
        [SerializeField] private GameObject _content;

        [SerializeField] private UIBeastUpgradeDetail _uiBeastDetail;
        [SerializeField] private CalculatorBeastStatsSO _calculatorBeastStatsSo;

        public void Show(IBeast beast)
        {
            _content.SetActive(true);
            _uiBeastDetail.SetupUI(beast);
            _calculatorBeastStatsSo.RaiseEvent(beast);
        }

        public void Hide()
        {
            _content.SetActive(false);
        }
    }
}