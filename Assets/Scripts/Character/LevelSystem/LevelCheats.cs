using CommandTerminal;
using CryptoQuest.Battle.Components;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.System;
using CryptoQuest.System.Cheat;
using IndiGames.Core.Events;
using TinyMessenger;
using UnityEngine;
using LevelSystemBehavior = CryptoQuest.Battle.Components.LevelSystem;

namespace CryptoQuest.Character.LevelSystem
{
    public class LevelCheats : MonoBehaviour, ICheatInitializer
    {
        private TinyMessageSubscriptionToken _leveledUpEvent;

        public void InitCheats()
        {
            Debug.Log("LevelCheats::InitCheats()");
            Terminal.Shell.AddCommand("addexp", AddExpToCharacter, 2, 2, "Add exp to a character");
        }

        private void OnEnable() => _leveledUpEvent =
            ActionDispatcher.Bind<HeroLeveledUpAction>(ctx => CharacterLevelUp(ctx.Hero));

        private void OnDisable() => ActionDispatcher.Unbind(_leveledUpEvent);

        private void CharacterLevelUp(HeroBehaviour character)
        {
            character.TryGetComponent(out LevelSystemBehavior levelSystem);
            Debug.Log($"Character {character.DetailsInfo} leveled up to {levelSystem.Level}!");
        }

        public void AddExpToCharacter(CommandArg[] args)
        {
            var partyController = ServiceProvider.GetService<IPartyController>();
            var memberIndex = args[0].Int;
            var expToAdd = args[1].Int;

            if (!partyController.GetHero(memberIndex, out var hero))
            {
                Debug.LogWarning($"Member index not valid");
                return;
            }

            if (!hero.TryGetComponent<LevelSystemBehavior>(out var levelComponent)) return;
            levelComponent.AddExp(expToAdd);
        }
    }
}