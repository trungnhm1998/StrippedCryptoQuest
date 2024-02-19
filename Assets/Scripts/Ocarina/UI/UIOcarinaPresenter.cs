using System.Collections;
using CryptoQuest.Map;
using CryptoQuest.Menus.Item.States;
using CryptoQuest.System.SaveSystem.Loaders;
using IndiGames.Core.Events;
using IndiGames.Core.Events.ScriptableObjects;
using TinyMessenger;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace CryptoQuest.Ocarina.UI
{
    public class UIOcarinaPresenter : MonoBehaviour
    {
        [SerializeField] private OcarinaLocations _ocarinaData;
        [SerializeField] private UIOcarinaTownButton _townButtonPrefab;
        [SerializeField] private GameObject _content;
        [SerializeField] private Transform _townButtonContainer;

        [Header("Listen on")] [SerializeField] private VoidEventChannelSO _showOcarinaMenuEvent;

        [SerializeField] private RegisterTownToOcarinaEventChannelSO _registerTownEvent;

        [Header("Raise event on")] [SerializeField]
        private VoidEventChannelSO _forceCloseMenuEvent; // TODO: Also bad code

        [SerializeField] private UnityEvent<MapPathSO> _teleportEvent;
        private TinyMessageSubscriptionToken _loadLocations;

        private void Awake()
        {
            _registerTownEvent.EventRaised += RegisterTown;
            _showOcarinaMenuEvent.EventRaised += Show;
            _loadLocations = ActionDispatcher.Bind<DestinationsLoaded>(HandleLoadedDestinations);
            for (var index = 0; index < _ocarinaData.Locations.Count; index++)
            {
                var town = _ocarinaData.Locations[index];
                RegisterTown(town);
            }
        }

        private void OnDestroy()
        {
            _registerTownEvent.EventRaised -= RegisterTown;
            _showOcarinaMenuEvent.EventRaised -= Show;
            ActionDispatcher.Unbind(_loadLocations);
            DestroyAllChildren();
        }

        private void HandleLoadedDestinations(DestinationsLoaded _)
        {
            for (var index = 0; index < _ocarinaData.ConditionalLocations.Count; index++)
            {
                var town = _ocarinaData.ConditionalLocations[index];
                RegisterTown(town);
            }
        }

        private void RegisterTown(OcarinaEntrance town)
        {
            var townButton = Instantiate(_townButtonPrefab, _townButtonContainer);
            townButton.SetTownName(town);
            townButton.Clicked += UseOcarina;
        }

        private void Show()
        {
            ItemConsumeState.Cancelled += CancelConsuming;
            _content.SetActive(true);
            StartCoroutine(CoSelectFirstButton());
        }

        private IEnumerator CoSelectFirstButton()
        {
            yield return null;
            EventSystem.current.SetSelectedGameObject(_townButtonContainer.GetChild(0).gameObject);
        }

        private void UseOcarina(OcarinaEntrance location)
        {
            _forceCloseMenuEvent.RaiseEvent();
            Hide();
            _teleportEvent.Invoke(location);
        }

        // TODO: Reuse buttons instead of destroying them, BAD CODE
        private void DestroyAllChildren()
        {
            while (_townButtonContainer.childCount > 0)
            {
                var go = _townButtonContainer.GetChild(0).gameObject;
                go.GetComponent<UIOcarinaTownButton>().Clicked -= UseOcarina;
                DestroyImmediate(go);
            }
        }

        private void CancelConsuming()
        {
            ItemConsumeState.Cancelled -= CancelConsuming;
            Hide();
        }

        private void Hide()
        {
            _content.SetActive(false);
        }
    }
}