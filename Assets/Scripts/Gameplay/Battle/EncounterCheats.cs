using CommandTerminal;
using CryptoQuest.Battle;
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
            Terminal.Shell.AddCommand("enc", TriggerEncounter, 1, 1, "Trigger encounter with id");
            Terminal.Shell.AddCommand("bat", TriggerBattle, 1, 1, "Trigger battle with id");
        }

        private void TriggerBattle(CommandArg[] args)
        {
            var partyId = args[0].Int;
            BattleLoader.RequestLoadBattle(partyId);
        }

        private void TriggerEncounter(CommandArg[] args)
        {
            var encounterId = args[0].String;
            _triggerEncounterEventChannel.RaiseEvent(encounterId);
        }
    }
}