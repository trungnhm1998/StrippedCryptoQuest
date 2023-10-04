using CryptoQuest.Menu;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.BlackSmith.EvolveStates.UI
{
    public class UIEvolveEquipmentList : MonoBehaviour
    {
        [SerializeField] private MultiInputButton _defaultSelection;

        private void OnEnable()
        {
            _defaultSelection.Select();
        }
    }
}