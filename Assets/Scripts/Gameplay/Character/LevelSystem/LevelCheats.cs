using CryptoQuest.System;
using CommandTerminal;
using CryptoQuest.Battle.Components;
using CryptoQuest.Events;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.System.Cheat;
using UnityEngine;

namespace CryptoQuest.Gameplay.Character.LevelSystem
{
    public class LevelCheats : MonoBehaviour, ICheatInitializer
    {
        [SerializeField] private CharacterSpecEventChannelSO _characterLevelUpEventChannel;

        public void InitCheats()
        {
            Debug.Log("LevelCheats::InitCheats()");
            Terminal.Shell.AddCommand("addexp", AddExpToCharacter, 2, 2, "Add exp to a character");
        }

        private void OnEnable()
        {
            _characterLevelUpEventChannel.EventRaised += CharacterLevelUp;
        }

        private void OnDisable()
        {
            _characterLevelUpEventChannel.EventRaised -= CharacterLevelUp;
        }

        private void CharacterLevelUp(CharacterSpec character)
        {
            Debug.Log($"Character {character.BackgroundInfo.name} leveled up to {character.Level}!");
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
            if (!hero.GameObject.TryGetComponent<CryptoQuest.Battle.Components.LevelSystem>(out var levelComponent)) return;
            levelComponent.AddExp(expToAdd);
        }
    }
}
