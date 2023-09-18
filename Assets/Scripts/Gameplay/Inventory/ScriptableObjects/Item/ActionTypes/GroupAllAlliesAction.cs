using CryptoQuest.Gameplay.Character;
using CryptoQuest.Gameplay.PlayerParty;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.ActionTypes
{
    public class GroupAllAlliesAction : ActionDefinitionBase
    {
        [SerializeField] private PartySO _party;

        protected override ActionSpecificationBase CreateInternal()
        {
            return new GroupActionSpec(_party);
        }
    }

    public class GroupActionSpec : ActionSpecificationBase
    {
        private PartySO _party;

        public GroupActionSpec(PartySO party)
        {
            _party = party;
        }

        protected override void OnExecute()
        {
            // TODO: REFACTOR GAS
            // UsableSO item = ActionContext.Item.Data;
            // CharacterSpec[] members = _party.Members;
            //
            // foreach (CharacterSpec characterSpec in members)
            // {
            //     AbilitySystemBehaviour owner = characterSpec.CharacterComponent.GameplayAbilitySystem;
            //
            //     CryptoQuestGameplayEffectSpec ability =
            //         (CryptoQuestGameplayEffectSpec)owner.MakeOutgoingSpec(item.Skill.Effect);
            //
            //     ability.SetParameters(item.ItemAbilityInfo.SkillParameters);
            //     owner.ApplyEffectSpecToSelf(ability);
            // }
        }
    }
}