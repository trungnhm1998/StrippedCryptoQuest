using System;
using CryptoQuest.Events;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.ActionTypes;
using CryptoQuest.Item.Ocarinas.Data;
using CryptoQuest.UI.Menu.Panels.Item.Ocarina.States;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CryptoQuest.UI.Menu.Panels.Item.Ocarina
{
    public class UIOcarinaPresenter : MonoBehaviour, IActionPresenter
    {
        [SerializeField] private PresenterBinder _binder;

        [SerializeField] private OcarinaDefinition _towns;

        [SerializeField] private UIConsumableMenuPanel _consumableMenuPanel;
        [SerializeField] private OcarinaTownButton _townButtonPrefab;
        [SerializeField] private GameObject _content;
        [SerializeField] private Transform _townButtonContainer;

        [Header("Raise event on")]
        [SerializeField] private MapPathEventChannelSO _destinationSelectedEvent;

        [SerializeField] private VoidEventChannelSO _destinationConfirmEvent;
        [SerializeField] private VoidEventChannelSO _forceCloseMenuEvent; // TODO: Also bad code

        private void Awake()
        {
            _consumableMenuPanel.StateMachine.AddState(OcarinaState.Ocarina, new OcarinaState(this));
            _binder.Bind(this);
        }

        private void OnEnable()
        {
            Hide();
        }

        public void Show()
        {
            _consumableMenuPanel.Interactable = false;
            _content.SetActive(true);
            DestroyAllChildren();

            for (var index = 0; index < _towns.Locations.Count; index++)
            {
                var town = _towns.Locations[index];
                var townButton = Instantiate(_townButtonPrefab, _townButtonContainer);
                townButton.SetTownName(town);

                townButton.Clicked += UseOcarina;

                if (index == 0)
                {
                    EventSystem.current.SetSelectedGameObject(townButton.gameObject);
                }
            }
        }

        private void UseOcarina(OcarinaDefinition.Location location)
        {
            _forceCloseMenuEvent.RaiseEvent();
            _destinationSelectedEvent.RaiseEvent(location.Path);
            _destinationConfirmEvent.RaiseEvent();
            Hide();
        }

        /// <summary>
        /// TODO: Reuse the buttons instead of destroying them, BAD CODE
        /// </summary>
        private void DestroyAllChildren()
        {
            while (_townButtonContainer.childCount > 0)
            {
                var go = _townButtonContainer.GetChild(0).gameObject;
                go.GetComponent<OcarinaTownButton>().Clicked -= UseOcarina;
                DestroyImmediate(go);
            }
        }

        public void Hide()
        {
            _consumableMenuPanel.Interactable = true;
            _content.SetActive(false);
        }

        public void Execute()
        {
            _consumableMenuPanel.StateMachine.RequestStateChange(OcarinaState.Ocarina);
        }
    }
}