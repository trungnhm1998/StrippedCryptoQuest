using CryptoQuest.Menu;
using UnityEngine;

namespace CryptoQuest.BlackSmith.EvolveStates.UI
{
    public class UIBlackSmithOverview : MonoBehaviour
    {
        [SerializeField] private MultiInputButton _defaultSelection;
        [SerializeField] private GameObject _upgradePanel;
        [SerializeField] private GameObject _evolvePanel;

        private void Awake()
        {
            _defaultSelection.Select();
        }

        public void UpgradeButtonPressed()
        {
            _upgradePanel.SetActive(true);
        }

        public void EvolveButtonPressed()
        {
            _evolvePanel.SetActive(true);
        }
    }
}