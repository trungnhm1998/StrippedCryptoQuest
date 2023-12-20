using CommandTerminal;
using CryptoQuest.Battle;
using CryptoQuest.Battle.Events;
using UnityEngine;

namespace CryptoQuest.System.Cheat
{
    public class BattleCheats : MonoBehaviour, ICheatInitializer
    {
        public void InitCheats()
        {
            Debug.Log($"BattleCheats.InitCheats()");
            // Terminal.Shell.AddCommand("bat.killall", TriggerKillAll, 0, 0, "Kill all enemies");
            // Terminal.Shell.AddCommand("bat.kill", TriggerKillCharacter, 1, 1, "Kill character");
            Terminal.Shell.AddCommand("bat.win", TriggerWinBattle, 0, 0, "Instantly win current battle");
            Terminal.Shell.AddCommand("bat.lose", TriggerLoseBattle, 0, 0, "Instantly lose current battle");
            Terminal.Shell.AddCommand("bat.retreat", InstantlyRetreat, 0, 0, "Instantly retreat current battle");
        }

        private void TriggerLoseBattle(CommandArg[] obj)
        {
            BattleEventBus.RaiseEvent(new SetResultStateEvent(EBattleResult.Lost));
            BattleEventBus.RaiseEvent(new ChangeToCurrentResultStateEvent());
        }

        private void TriggerWinBattle(CommandArg[] obj)
        {
            BattleEventBus.RaiseEvent(new SetResultStateEvent(EBattleResult.Retreated));
            BattleEventBus.RaiseEvent(new ChangeToCurrentResultStateEvent());
        }

        private void InstantlyRetreat(CommandArg[] obj)
        {
            BattleEventBus.RaiseEvent(new SetResultStateEvent(EBattleResult.Retreated));
            BattleEventBus.RaiseEvent(new ChangeToCurrentResultStateEvent());
        }
    }
}