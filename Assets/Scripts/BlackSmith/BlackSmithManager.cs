using CryptoQuest.BlackSmith.EvolveStates;
using UnityEngine;

namespace CryptoQuest.BlackSmith
{
    public class BlackSmithManager : MonoBehaviour
    {
        [SerializeField] private GameObject _evolveStateController;
        [SerializeField] private GameObject _upgradeStateController;

        public void EvolveButtonPressed()
        {
            _evolveStateController.SetActive(true);
        }

        public void UpgradeButtonPressed()
        {
            _upgradeStateController.SetActive(true);
        }
    }
}
