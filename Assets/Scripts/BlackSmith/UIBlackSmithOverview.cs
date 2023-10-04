using CryptoQuest.Menu;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.BlackSmith
{
    public class UIBlackSmithOverview : MonoBehaviour
    {
        [SerializeField] private MultiInputButton _defaultSelection;
        [SerializeField] private GameObject _upgradePanel;
        [SerializeField] private GameObject _evolvePanel;
        [SerializeField] private UnityEvent _upgradeButtonPressedEvent;
        [SerializeField] private UnityEvent _evolveButtonPressedEvent;

        public void InputEnabled()
        {
            _defaultSelection.Select();
        }

        public void OnUpgradeButtonPressed()
        {
            _upgradePanel.SetActive(true);
            _upgradeButtonPressedEvent.Invoke();
        }

        public void OnEvolveButtonPressed()
        {
            _evolveButtonPressedEvent.Invoke();
        }
    }
}