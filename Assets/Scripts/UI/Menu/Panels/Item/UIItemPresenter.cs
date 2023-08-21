using CryptoQuest.Gameplay;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.ActionTypes;
using CryptoQuest.UI.Menu.MenuStates.ItemStates;
using CryptoQuest.UI.Menu.Panels.Item.States;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Panels.Item
{
    public class UIItemPresenter : MonoBehaviour, IActionPresenter
    {
        [SerializeField] private PresenterBinder _binder;
        [SerializeField] private UIConsumableMenuPanel _uiConsumableMenuPanel;
        [SerializeField] private PartySO _partySo;

        [SerializeField] private UsableSO _item;

        [SerializeField] private UIItemCharacterSelection _uiItemCharacterSelection;


        private void Awake()
        {
            _uiConsumableMenuPanel.StateMachine.AddState(ItemState.UseItemForSingleAlly, new ItemState(this));
            _binder.Bind(this);
        }

        public void Show()
        {
            _uiConsumableMenuPanel.Interactable = false;
            _uiItemCharacterSelection.Init();

            _uiItemCharacterSelection.Clicked += UseItem;
        }

        private void UseItem(int index)
        {
            AbilitySystemBehaviour owner = _partySo.PlayerTeam.Members[index];
            AbstractAbility ability = _item.Ability.GetAbilitySpec(owner);
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
            _uiConsumableMenuPanel.StateMachine.RequestStateChange(ItemState.UseItemForSingleAlly);
        }
    }
}