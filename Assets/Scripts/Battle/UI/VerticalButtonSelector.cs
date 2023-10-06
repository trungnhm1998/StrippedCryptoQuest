using CryptoQuest.Input;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CryptoQuest.Battle.UI
{
    public class VerticalButtonSelector : MonoBehaviour
    {
        [SerializeField] private GameObject _firstSelectedButton;
        [SerializeField] private bool _wrapAround;
        [SerializeField] private bool _isSelectFirstButtonOnEnable;
        [SerializeField] private bool _isSelectLastManagedButtonOnEnable;
        [SerializeField] private Transform _buttonsContainer;

        private GameObject _lastSelectedButton;
        public GameObject LastSelectedButton => _lastSelectedButton;

        private bool _interactable = true;

        public bool Interactable
        {
            get => _interactable;
            set
            {
                _interactable = value;
                CacheLastSelectedButton();
                SelectLastManagedSelectedButton();

                foreach (Transform button in _buttonsContainer)
                    button.GetComponent<Selectable>().interactable = _interactable;
            }
        }

        private void CacheLastSelectedButton()
        {
            var currentSelectedGameObject = EventSystem.current.currentSelectedGameObject;
            if (currentSelectedGameObject == null) return;
            if (currentSelectedGameObject.transform.IsChildOf(_buttonsContainer) == false) return;
            _lastSelectedButton = currentSelectedGameObject;
        }

        private bool SelectLastManagedSelectedButton()
        {
            if (!_isSelectLastManagedButtonOnEnable || !_interactable) return false;
            if (_lastSelectedButton == null || !_lastSelectedButton.transform.IsChildOf(_buttonsContainer))
                return false;
            DOVirtual.DelayedCall(SELECT_DELAY, () =>
            {
                EventSystem.current.SetSelectedGameObject(_lastSelectedButton);
                _lastSelectedButton = null;
            });
            return true;
        }

        private int _currentIndex;
        private const float SELECT_DELAY = 0.1f;

        private int CurrentIndex
        {
            get => _currentIndex;
            set
            {
                _currentIndex = value;

                if (_currentIndex >= 0 && _currentIndex < _buttonsContainer.childCount) return;
                var childCount = _buttonsContainer.childCount;
                _currentIndex = _wrapAround
                    ? (_currentIndex + childCount) % childCount
                    : Mathf.Clamp(_currentIndex, 0, _buttonsContainer.childCount - 1);
            }
        }

        private void OnValidate()
        {
            if (_buttonsContainer == null)
                _buttonsContainer = transform;
        }

        private void OnEnable()
        {
            BattleInput.instance.NavigateEvent += NavigateSelectCommand;
            if (SelectLastManagedSelectedButton()) return;
            if (_isSelectFirstButtonOnEnable)
            {
                SelectFirstButton();
            }
        }

        private void OnDisable()
        {
            BattleInput.instance.NavigateEvent -= NavigateSelectCommand;
            CacheLastSelectedButton();
        }

        private void NavigateSelectCommand(Vector2 dir)
        {
            if (!Interactable) return;
            CurrentIndex += (int)dir.y * -1;
            EventSystem.current.SetSelectedGameObject(_buttonsContainer.GetChild(CurrentIndex).gameObject);
        }

        public void SelectFirstButton()
        {
            CurrentIndex = 0;
            DOVirtual.DelayedCall(SELECT_DELAY, () =>
            {
                var buttonToSelect = _firstSelectedButton == null
                    ? _buttonsContainer.GetChild(0).gameObject
                    : _firstSelectedButton;
                EventSystem.current.SetSelectedGameObject(buttonToSelect);
            });
        }
    }
}