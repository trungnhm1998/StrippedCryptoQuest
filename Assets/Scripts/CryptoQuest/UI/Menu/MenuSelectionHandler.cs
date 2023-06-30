using System.Collections;
using Core.Runtime.EditorTools.Attributes.ReadOnlyAttribute;
using CryptoQuest.Input;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CryptoQuest.UI.Menu
{
    public class MenuSelectionHandler : MonoBehaviour
    {
        [SerializeField] private InputMediatorSO _inputReader;
        [SerializeField, ReadOnly] private GameObject _defaultSelection;
        [SerializeField, ReadOnly] private GameObject _currentSelection;
        [SerializeField, ReadOnly] private GameObject _mouseSelection;

        private void OnEnable()
        {
            _inputReader.MenuMouseMoveEvent += HandleMoveCursor;
            _inputReader.MenuMouseMoveEvent += HandleMenuMouseMove;

            StartCoroutine(SelectDefault());
        }

        private void OnDisable()
        {
            _inputReader.MenuMouseMoveEvent -= HandleMoveCursor;
            _inputReader.MenuMouseMoveEvent -= HandleMenuMouseMove;
        }

        public void UpdateDefault(GameObject newDefault)
        {
            _defaultSelection = newDefault;
        }

        private IEnumerator SelectDefault()
        {
            yield return new WaitForSeconds(.03f);

            if (_defaultSelection != null)
                UpdateSelection(_defaultSelection);
        }

        public void Unselect()
        {
            _currentSelection = null;
            if (EventSystem.current != null)
                EventSystem.current.SetSelectedGameObject(null);
        }

        private void HandleMenuMouseMove()
        {
            // Cursor.visible = false;

            if (EventSystem.current.currentSelectedGameObject == null)
                EventSystem.current.SetSelectedGameObject(_currentSelection);
        }

        private void HandleMoveCursor()
        {
            if (_mouseSelection != null)
            {
                EventSystem.current.SetSelectedGameObject(_mouseSelection);
            }

            // Cursor.visible = true;
        }

        public void HandleMouseEnter(GameObject UIElement)
        {
            _mouseSelection = UIElement;
            EventSystem.current.SetSelectedGameObject(UIElement);
        }

        public void HandleMouseExit(GameObject UIElement)
        {
            if (EventSystem.current.currentSelectedGameObject != UIElement)
            {
                return;
            }

            _mouseSelection = null;
            EventSystem.current.SetSelectedGameObject(_currentSelection);
        }

        public bool AllowsSubmit()
        {
            return !_inputReader.LeftMouseDown()
                   || _mouseSelection != null && _mouseSelection == _currentSelection;
        }

        public void UpdateSelection(GameObject UIElement)
        {
            if ((UIElement.GetComponent<MultiInputSelectableElement>() != null) ||
                (UIElement.GetComponent<MultiInputButton>() != null))
            {
                _mouseSelection = UIElement;
                _currentSelection = UIElement;
            }
        }

        private void OnGUI()
        {
            GUILayout.Box($"_currentSelection: {(_currentSelection != null ? _currentSelection.name : "null")}");
            GUILayout.Box($"_mouseSelection: {(_mouseSelection != null ? _mouseSelection.name : "null")}");
        }

        private void Update()
        {
            if ((EventSystem.current != null) && (EventSystem.current.currentSelectedGameObject == null) &&
                (_currentSelection != null))
            {
                EventSystem.current.SetSelectedGameObject(_currentSelection);
            }
        }
    }
}