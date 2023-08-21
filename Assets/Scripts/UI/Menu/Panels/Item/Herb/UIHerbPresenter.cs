using CryptoQuest.Gameplay;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.ActionTypes;
using CryptoQuest.UI.Menu.MenuStates.ItemStates;
using CryptoQuest.UI.Menu.Panels.Item.States;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Panels.Item
{
    public class UIHerbPresenter : MonoBehaviour, IActionPresenter
    {
        [SerializeField] private PresenterBinder _binder;
        [SerializeField] private UIConsumableMenuPanel _uiConsumableMenuPanel;
        [SerializeField] private PartySO _partySo;

        [SerializeField] private UsableSO _item;

        [SerializeField] private UIItemCharacterSelection _uiItemCharacterSelection;


        private void Awake()
        {
            _uiConsumableMenuPanel.StateMachine.AddState(HerbState.Herb, new HerbState(this));
            _binder.Bind(this);
        }

        public void Show()
        {
            _uiConsumableMenuPanel.Interactable = false;
            _uiItemCharacterSelection.Init();

            _uiItemCharacterSelection.Clicked += UseHerb;
        }

        private void UseHerb(int index)
        {
            var owner = _partySo.PlayerTeam.Members[index];
            AbstractAbility ability = owner.GiveAbility(_item.Ability);
            ability.ActivateAbility();
        }

        public void Hide()
        {
            _uiConsumableMenuPanel.Interactable = true;
            _uiItemCharacterSelection.DeInit();
            _uiConsumableMenuPanel.StateMachine.RequestStateChange(ItemMenuStateMachine.InventorySelection);
        }

        public void Execute()
        {
            _uiConsumableMenuPanel.StateMachine.RequestStateChange(HerbState.Herb);
        }
    }
}