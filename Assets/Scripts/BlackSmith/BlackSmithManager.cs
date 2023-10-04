using CryptoQuest.BlackSmith.EvolveStates;
using UnityEngine;

namespace CryptoQuest.BlackSmith
{
    public class BlackSmithManager : MonoBehaviour
    {
        [SerializeField] private GameObject _evolveStateController;

        public void EvolveButtonPressed()
        {
            _evolveStateController.SetActive(true);
        }
    }
}
