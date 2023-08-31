using System;
using CommandTerminal;
using CryptoQuest.Events;
using CryptoQuest.System.Cheat;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle
{
    public class EncounterCheats : MonoBehaviour, ICheatInitializer
    {
        [SerializeField] private StringEventChannelSO _triggerEncounterEventChannel;
        public void InitCheats()
        {
            Debug.Log("EncounterCheats::InitCheats()");
            Terminal.Shell.AddCommand("battle", Battle, 1, 1, "Start a battle");
        }

        private void Battle(CommandArg[] args)
        {
            var encounterId = args[0].String;
            _triggerEncounterEventChannel.RaiseEvent(encounterId);
        }
    }
}