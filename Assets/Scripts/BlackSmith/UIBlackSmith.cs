using System.Collections;
using CryptoQuest.Menu;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.BlackSmith
{
    public class UIBlackSmith : MonoBehaviour
    {
        [SerializeField] private MultiInputButton _defaultSelection;
        [SerializeField] private GameObject _upgradePanel;
        [SerializeField] private GameObject _blackSmithOverview;
        [SerializeField] private GameObject _selectActionPanel;

        [Header("Unity Events")]
        [SerializeField] private UnityEvent _upgradeButtonPressedEvent;
        [SerializeField] private UnityEvent _evolveButtonPressedEvent;

        public void BlackSmithOpened()
        {
            _blackSmithOverview.SetActive(true);
            StartCoroutine(CoSelectDefault());
        }

        public void BlackSmithClosed()
        {
            _blackSmithOverview.SetActive(false);
            StopCoroutine(CoSelectDefault());
        }

        private IEnumerator CoSelectDefault()
        {
            yield return null;
            _defaultSelection.Select();
        }

        public void OnUpgradeButtonPressed()
        {
            _upgradePanel.SetActive(true);
            _upgradeButtonPressedEvent.Invoke();
        }

        public void OnEvolveButtonPressed()
        {
            _selectActionPanel.SetActive(false);
            _evolveButtonPressedEvent.Invoke();
        }
    }
}