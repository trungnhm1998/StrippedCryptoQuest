using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.BlackSmith.UpgradeStone.UI
{
    public class UIPassiveInfoDetail : MonoBehaviour
    {
        [SerializeField] private LocalizeStringEvent _passiveDescription;
        [SerializeField] public Text _passiveValue;

        public void Initialize(LocalizedString passiveName, string passiveValue, Color color)
        {
            _passiveDescription.StringReference = passiveName;
            _passiveValue.text = passiveValue;
            _passiveValue.color = color;
        }
    }
}