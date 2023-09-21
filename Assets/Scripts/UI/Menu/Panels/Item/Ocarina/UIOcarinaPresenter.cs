﻿using System.Collections;
using CryptoQuest.Item.Ocarina;
using CryptoQuest.UI.Menu.MenuStates.ItemStates;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CryptoQuest.UI.Menu.Panels.Item.Ocarina
{
    public class UIOcarinaPresenter : MonoBehaviour
    {
        [SerializeField] private UIOcarinaTownButton _townButtonPrefab;
        [SerializeField] private GameObject _content;
        [SerializeField] private Transform _townButtonContainer;
        [SerializeField] private OcarinaAbility _ocarinaAbility;

        [Header("Listen on")]
        [SerializeField] private VoidEventChannelSO _showOcarinaMenuEvent;

        [SerializeField] private RegisterTownToOcarinaEventChannelSO _registerTownEvent;

        [Header("Raise event on")]
        [SerializeField] private VoidEventChannelSO _forceCloseMenuEvent; // TODO: Also bad code

        private void Awake()
        {
            _registerTownEvent.EventRaised += RegisterTown;
            _showOcarinaMenuEvent.EventRaised += Show;
            var locations = _ocarinaAbility.Locations;
            for (var index = 0; index < locations.Count; index++)
            {
                var town = locations[index];
                RegisterTown(town);
            }
        }

        private void OnDestroy()
        {
            _registerTownEvent.EventRaised -= RegisterTown;
            _showOcarinaMenuEvent.EventRaised -= Show;
            DestroyAllChildren();
        }

        private void Show()
        {
            ConsumingItemState.Cancelled += CancelConsuming;
            _content.SetActive(true);
            StartCoroutine(CoSelectFirstButton());
        }

        private IEnumerator CoSelectFirstButton()
        {
            yield return null;
            EventSystem.current.SetSelectedGameObject(_townButtonContainer.GetChild(0).gameObject);
        }

        private void RegisterTown(OcarinaEntrance town)
        {
            var townButton = Instantiate(_townButtonPrefab, _townButtonContainer);
            townButton.SetTownName(town);
            townButton.Clicked += UseOcarina;
            _ocarinaAbility.RegisterTown(town);
        }

        private void UseOcarina(OcarinaEntrance location)
        {
            _forceCloseMenuEvent.RaiseEvent();
            Hide();
            var spec = _ocarinaAbility.GetAbilitySpec(null);
            ((OcarinaAbility.OcarinaAbilitySpec)spec).TeleportToTown(location);
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
            ConsumingItemState.Cancelled -= CancelConsuming;
            Hide();
        }

        private void Hide()
        {
            _content.SetActive(false);
        }
    }
}