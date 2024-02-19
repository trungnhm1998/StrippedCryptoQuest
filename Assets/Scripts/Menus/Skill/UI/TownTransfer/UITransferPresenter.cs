using System.Collections;
using CryptoQuest.Actions;
using CryptoQuest.Input;
using CryptoQuest.Map;
using CryptoQuest.Menus.Item.States;
using CryptoQuest.Menus.TownTransfer;
using CryptoQuest.Ocarina;
using CryptoQuest.System.SaveSystem.Loaders;
using IndiGames.Core.Events;
using IndiGames.Core.Events.ScriptableObjects;
using TinyMessenger;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace CryptoQuest.Menus.Skill.UI.TownTransfer
{
    public class UITransferPresenter : MonoBehaviour
    {
        [SerializeField] private TownTransferLocations _transferLocations;
        [SerializeField] private UITownTransferOptionButton _townButtonPrefab;
        [SerializeField] private GameObject _content;
        [SerializeField] private Transform _townButtonContainer;
        [SerializeField] private UISkillMenu _skillMenu;
        [SerializeField] private InputMediatorSO _inputMediatorSO;

        [Header("Listen on")] [SerializeField] private VoidEventChannelSO _showTownMenuEvent;


        [Header("Raise event on")] [SerializeField]
        private VoidEventChannelSO _forceCloseMenuEvent;

        [SerializeField] private UnityEvent<MapPathSO> _teleportEvent;
        [SerializeField] private UnityEvent _teleportCancel;
        [SerializeField] private RegisterTownToTransferEventChannelSO _registerTown;
        private TinyMessageSubscriptionToken _triggerTransferAbilityEventToken;
        private bool _isActivateFromAbility = false;
        private TinyMessageSubscriptionToken _loadLocations;

        private void Awake()
        {
            _showTownMenuEvent.EventRaised += Show;
            _registerTown.EventRaised += RegisterTown;
            _loadLocations = ActionDispatcher.Bind<DestinationsLoaded>(HandleLoadedDestinations);
            foreach (var town in _transferLocations.Locations)
            {
                RegisterTown(town);
            }
        }

        private void OnDestroy()
        {
            _showTownMenuEvent.EventRaised -= Show;
            _registerTown.EventRaised -= RegisterTown;
            ActionDispatcher.Unbind(_loadLocations);
            DestroyAllChildren();
        }

        private void HandleLoadedDestinations(DestinationsLoaded _)
        {
            for (var index = 0; index < _transferLocations.ConditionalLocations.Count; index++)
            {
                var town = _transferLocations.ConditionalLocations[index];
                RegisterTown(town);
            }
        }

        private void RegisterTown(TownTransferPath town)
        {
            var townButton = Instantiate(_townButtonPrefab, _townButtonContainer);
            townButton.SetTownName(town);
            townButton.Clicked += UseTransfer;
        }

        private void Show()
        {
            if (_isActivateFromAbility)
            {
                _skillMenu.SkillMenuStateMachine.OnExit();
                _inputMediatorSO.MenuCancelEvent += CancelConsuming;
            }
            else
                ItemConsumeState.Cancelled += CancelConsuming;

            _content.SetActive(true);
            StartCoroutine(CoSelectFirstButton());
        }

        private IEnumerator CoSelectFirstButton()
        {
            yield return null;
            EventSystem.current.SetSelectedGameObject(_townButtonContainer.GetChild(0).gameObject);
        }

        private void UseTransfer(TownTransferPath location)
        {
            _forceCloseMenuEvent.RaiseEvent();
            _teleportEvent.Invoke(location);
            Hide();
        }

        private void DestroyAllChildren()
        {
            while (_townButtonContainer.childCount > 0)
            {
                var go = _townButtonContainer.GetChild(0).gameObject;
                go.GetComponent<UITownTransferOptionButton>().Clicked -= UseTransfer;
                DestroyImmediate(go);
            }
        }

        private void CancelConsuming()
        {
            Hide();
        }

        private void Hide()
        {
            if (_isActivateFromAbility)
            {
                _skillMenu.SkillMenuStateMachine.OnEnter();
                _isActivateFromAbility = false;
                _inputMediatorSO.MenuCancelEvent -= CancelConsuming;
                _teleportCancel?.Invoke();
            }
            else
                ItemConsumeState.Cancelled -= CancelConsuming;

            ActionDispatcher.Dispatch(new TownTransferCancelEvent());
            _content.SetActive(false);
        }

        private void OnEnable()
        {
            _triggerTransferAbilityEventToken =
                ActionDispatcher.Bind<TriggerTownTransferAbilityEvent>(HandleTriggerTownTransferAbilityEvent);
        }

        private void OnDisable()
        {
            ActionDispatcher.Unbind(_triggerTransferAbilityEventToken);
        }

        private void HandleTriggerTownTransferAbilityEvent(TriggerTownTransferAbilityEvent _)
        {
            _isActivateFromAbility = true;
            Show();
        }
    }
}