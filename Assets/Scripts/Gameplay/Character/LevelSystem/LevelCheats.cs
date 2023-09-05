using CryptoQuest.System;
using CommandTerminal;
using CryptoQuest.Events;
using CryptoQuest.System.Cheat;
using UnityEngine;

namespace CryptoQuest.Gameplay.Character.LevelSystem
{
    public class LevelCheats : MonoBehaviour, ICheatInitializer
    {
        [SerializeField] private ServiceProvider _provider;
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
            var party = _provider.PartyController.Party;
            var memberIndex = args[0].Int;
            var expToAdd = args[1].Int;

            if (0 < memberIndex || memberIndex >= party.Members.Length)
            {
                Debug.LogWarning($"Member index not valid");
                return;
            }
            var member = party.Members[memberIndex];
            LevelController.AddExpRequested?.Invoke(member, expToAdd);
        }
    }
}
