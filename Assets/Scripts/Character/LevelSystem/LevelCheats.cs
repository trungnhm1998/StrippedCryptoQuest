using CommandTerminal;
using CryptoQuest.Battle.Components;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.System;
using CryptoQuest.System.Cheat;
using UnityEngine;
using LevelSystemBehavior = CryptoQuest.Battle.Components.LevelSystem;

namespace CryptoQuest.Character.LevelSystem
{
    public class LevelCheats : MonoBehaviour, ICheatInitializer
    {
        public void InitCheats()
        {
            Debug.Log("LevelCheats::InitCheats()");
            Terminal.Shell.AddCommand("addexp", AddExpToCharacter, 2, 2, "Add exp to a character");
        }

        private void OnEnable()
        {
            LevelSystemBehavior.HeroLeveledUp += CharacterLevelUp;
        }

        private void OnDisable()
        {
            LevelSystemBehavior.HeroLeveledUp -= CharacterLevelUp;
        }

        private void CharacterLevelUp(HeroBehaviour character)
        {
            Debug.Log($"Character {character.DetailsInfo} leveled up to {character.Level}!");
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
            if (!hero.GameObject.TryGetComponent<LevelSystemBehavior>(out var levelComponent)) return;
            levelComponent.AddExp(expToAdd);
        }
    }
}
