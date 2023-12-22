using CommandTerminal;
using CryptoQuest.Character.Behaviours;
using CryptoQuest.System.Cheat;
using UnityEngine;

namespace CryptoQuest.Character.MonoBehaviours
{
    public class EncounterRateCheat : MonoBehaviour, ICheatInitializer
    {
        public void InitCheats()
        {
            Terminal.Shell.AddCommand("enr", SetEncounterRate, 1, 1,
                "Set encounter rate as distance per step, the higher the number the less likely to encounter");
        }

        private void SetEncounterRate(CommandArg[] obj)
        {
            var rate = obj[0].Float;
            var hero = FindObjectOfType<HeroController>(); // it is a cheat I don't care about performance
            if (hero.TryGetComponent(out StepBehaviour stepBehaviourComponent))
                stepBehaviourComponent.SetDistanceTilNextStep(rate);
        }
    }
}