using CryptoQuest.Character.Ability;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Panels.Item
{
    public class ConsumeItemPresenter : MonoBehaviour
    {
        [SerializeField] private ConsumableEventChannel _heroConsumeItem;
        [SerializeField] private UIConsumableMenuPanel _uiConsumableMenuPanel;
        [SerializeField] private UIItemCharacterSelection _uiItemCharacterSelection;

        private ConsumableSO _item;

        private void OnEnable()
        {
            SingleHeroConsumeAbility.ShowHeroSelection += Show;
        }

        /// <summary>
        /// I don't want to show this panel when the main menu is disabled
        /// </summary>
        private void OnDisable()
        {
            SingleHeroConsumeAbility.ShowHeroSelection -= Show;
        }

        private void Show()
        {
            _uiConsumableMenuPanel.Interactable = false;
            _uiItemCharacterSelection.Init();

            _uiItemCharacterSelection.Clicked += ConsumeOnCharacterIndex;
        }

        private void GetItem(UIConsumableItem currentItem)
        {
            _item = currentItem.Consumable.Data;
        }

        private void ConsumeOnCharacterIndex(int index)
        {
            ConsumableController.HeroConsumingItem?.Invoke(index, _uiConsumableMenuPanel.ConsumingItem);
            // TODO: RÈACTOR GÁ
            // AbilitySystemBehaviour owner = _partySo.Members[index].CharacterComponent.GameplayAbilitySystem;
            //
            // CryptoQuestGameplayEffectSpec ability =
            //     (CryptoQuestGameplayEffectSpec)owner.MakeOutgoingSpec(_item.Skill.Effect);
            //
            // ability.SetParameters(_item.ItemAbilityInfo.SkillParameters);
            // owner.ApplyEffectSpecToSelf(ability);

            Hide();
        }

        private void Hide()
        {
            _uiConsumableMenuPanel.Interactable = true;
            _uiItemCharacterSelection.DeInit();

            _uiItemCharacterSelection.Clicked -= ConsumeOnCharacterIndex;
        }
    }
}