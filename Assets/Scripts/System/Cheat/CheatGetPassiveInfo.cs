using CommandTerminal;
using CryptoQuest.AbilitySystem.Abilities;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.EffectSystem.Components;
using UnityEngine;

namespace CryptoQuest.System.Cheat
{
    public class CheatGetPassiveInfo : MonoBehaviour, ICheatInitializer
    {
        public void InitCheats()
        {
            Terminal.Shell.AddCommand("get.passives", GetCharacterPassives, 1, 1,
                "get.passive <character_index>, get character's all passive with character id ");

            Terminal.Shell.AddCommand("get.effects", GetCharacterAppliedEffects, 1, 1,
                "get.effects <character_index>, get character's all applied effects with character id ");

            Terminal.Shell.AddCommand("get.states", GetCharacterCurrentStates, 1, 1,
                "get.states <character_index>, get character's all state tags with character id ");
        }

        private void GetCharacterPassives(CommandArg[] args)
        {
            int characterId = args[0].Int;
            if (!TryGetCharacter(characterId, out var character)) return;

            AbilitySystemBehaviour abilitySystem = character.AbilitySystem;
            var grantedAbilities = abilitySystem.GrantedAbilities;
            Debug.Log($"Character: {character.DisplayName}");
            foreach (var grantedAbility in grantedAbilities)
            {
                if (grantedAbility.AbilitySO is not PassiveAbility) continue;
                Debug.Log($"Passive: {grantedAbility.AbilitySO.name}");
            }
        }

        private void GetCharacterAppliedEffects(CommandArg[] args)
        {
            int characterId = args[0].Int;
            if (!TryGetCharacter(characterId, out var character)) return;

            EffectSystemBehaviour effectSystem = character.GameplayEffectSystem;
            var appliedEffects = effectSystem.AppliedEffects;
            Debug.Log($"Character: {character.DisplayName}");
            foreach (var effect in appliedEffects)
            {
                Debug.Log(
                    $"Effect: {effect.Spec.Def.name}  from {effect.Spec.Source.name}  to {effect.Spec.Target.name}");
            }
        }

        private void GetCharacterCurrentStates(CommandArg[] args)
        {
            int characterId = args[0].Int;
            if (!TryGetCharacter(characterId, out var character)) return;

            TagSystemBehaviour tagSystem = character.AbilitySystem.TagSystem;
            var grantedTags = tagSystem.GrantedTags;
            Debug.Log($"Character: {character.DisplayName}");
            foreach (var grantedTag in grantedTags)
            {
                Debug.Log($"Tag: {grantedTag.name}");
            }
        }

        private bool TryGetCharacter(int characterId, out Battle.Components.Character character)
        {
            character = CharacterCheats.Instance.GetCharacter(characterId);
            if (character == null)
            {
                Debug.LogWarning($"Cannot find character with instance id [{characterId}].");
                return false;
            }

            return true;
        }
    }
}