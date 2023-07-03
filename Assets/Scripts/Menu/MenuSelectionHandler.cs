using System.Collections;
using CryptoQuest.Input;
using IndiGames.Core.EditorTools.Attributes.ReadOnlyAttribute;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CryptoQuest.Menu
{
    public class MenuSelectionHandler : MonoBehaviour
    {
        [SerializeField] private InputMediatorSO _inputMediator;
        [SerializeField][ReadOnly] private GameObject _defaultSelection;
        [SerializeField][ReadOnly] private GameObject _currentSelection;
        [SerializeField][ReadOnly] private GameObject _mouseSelection;

        private void OnEnable()
        {
            _inputMediator.MenuNavigateEvent += HandleMoveCursor;
            _inputMediator.MoveSelectionEvent += HandleMoveSelection;

            StartCoroutine(SelectDefault());
        }

        private void OnDisable()
        {
            _inputMediator.MenuNavigateEvent -= HandleMoveCursor;
            _inputMediator.MoveSelectionEvent -= HandleMoveSelection;
        }

        public void UpdateDefault(GameObject newDefault)
        {
            _defaultSelection = newDefault;
        }

        /// <summary>
        /// Highlights the default element
        /// </summary>
        private IEnumerator SelectDefault()
        {
            yield return new WaitForSeconds(.03f); // Necessary wait otherwise the highlight won't show up

            if (_defaultSelection != null)
                UpdateSelection(_defaultSelection);
        }

        public void Unselect()
        {
            _currentSelection = null;
            if (EventSystem.current != null)
                EventSystem.current.SetSelectedGameObject(null);
        }

        /// <summary>
        /// Fired by keyboard and gamepad inputs. Current selected UI element will be the ui Element that was selected
        /// when the event was fired. The _currentSelection is updated later on, after the EventSystem moves to the
        /// desired UI element, the UI element will call into UpdateSelection()
        /// </summary>
        private void HandleMoveSelection()
        {
            Cursor.visible = false;

            // Handle case where no UI element is selected because mouse left selectable bounds
            if (EventSystem.current.currentSelectedGameObject == null)
                EventSystem.current.SetSelectedGameObject(_currentSelection);
        }

        private void HandleMoveCursor()
        {
            if (_mouseSelection != null)
            {
                EventSystem.current.SetSelectedGameObject(_mouseSelection);
            }

            Cursor.visible = true;
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

            // keep selecting the last thing the mouse has selected 
            _mouseSelection = null;
            EventSystem.current.SetSelectedGameObject(_currentSelection);
        }

        /// <summary>
        /// Method interactable UI elements should call on Submit interaction to determine whether to continue or not.
        /// </summary>
        /// <returns></returns>
        public bool AllowsSubmit()
        {
            // if LMB is not down, there is no edge case to handle, allow the event to continue
            return !_inputMediator.LeftMouseDown()
                   // if we know mouse & keyboard are on different elements, do not allow interaction to continue
                   || _mouseSelection != null && _mouseSelection == _currentSelection;
        }

        /// <summary>
        /// Fired by gamepad or keyboard navigation inputs
        /// </summary>
        /// <param name="UIElement"></param>
        public void UpdateSelection(GameObject UIElement)
        {
            if ((UIElement.GetComponent<MultiInputSelectableElement>() != null) || (UIElement.GetComponent<MultiInputButton>() != null))
            {
                _mouseSelection = UIElement;
                _currentSelection = UIElement;
            }
        }

        // Debug
         private void OnGUI()
         {
        	 	GUILayout.Box($"_currentSelection: {(_currentSelection != null ? _currentSelection.name : "null")}");
        	 	GUILayout.Box($"_mouseSelection: {(_mouseSelection != null ? _mouseSelection.name : "null")}");
         }
        private void Update()
        {
            if ((EventSystem.current != null) && (EventSystem.current.currentSelectedGameObject == null) && (_currentSelection != null))
            {

                EventSystem.current.SetSelectedGameObject(_currentSelection);
            }
        }
    }
}
