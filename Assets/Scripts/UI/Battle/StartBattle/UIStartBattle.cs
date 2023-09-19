using CryptoQuest.Events;
using CryptoQuest.UI.Battle.SelectCommand;
using CryptoQuest.UI.Battle.StateMachine;
using FSM;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.UI.Battle.StartBattle
{
    public class UIStartBattle : MonoBehaviour, IBattleMenu
    {
        public static readonly string StartBattleState = "StartBattle";
        public string StateName => StartBattleState;

        [SerializeField] private GameObject _inBattleUI;
        [SerializeField] private LocalizedString _battleAppearPrompt;
        [SerializeField] private float _duration = 3f;

        [Header("Raise Events")]
        [SerializeField] private LocalizedStringEventChannelSO _showBattleDialogEventChannel;
        [SerializeField] private VoidEventChannelSO _showNextMarkEventChannel;
        [SerializeField] private VoidEventChannelSO _closeBattleDialogEventChannel;

        
        [Header("Listen Events")]
        [SerializeField] private VoidEventChannelSO _doneShowDialogEvent;

        private BattleMenuController _controller;

        private void OnEnable()
        {
            _doneShowDialogEvent.EventRaised += ShowDialogDone;
        }

        private void OnDisable()
        {
            _doneShowDialogEvent.EventRaised -= ShowDialogDone;
        }

        private void ShowDialogDone()
        {
            _closeBattleDialogEventChannel.RaiseEvent();
            SetActiveInBattleUI(true);
            // I use reset here because it cannot pushdown to this state
            _controller.BattleMenuFSM.ResetToNewState(UISelectCommand.SelectCommandState);
        }

        public void EnterStartBattleState()
        {
            SetActiveInBattleUI(false);
            ShowStartPrompt();
        }

        public void SetActiveInBattleUI(bool value)
        {
            _inBattleUI.SetActive(value);
        }

        private void ShowStartPrompt()
        {
            _showBattleDialogEventChannel.RaiseEvent(_battleAppearPrompt);
            Invoke(nameof(FinishStartPrompt), _duration);
        }

        /// <summary>
        /// This will trigger the battle dialog to show next mark
        /// and depend on setting, dialog will auto done or done with input
        /// </summary>
        private void FinishStartPrompt()
        {
            _showNextMarkEventChannel.RaiseEvent();
        }

        public StateBase<string> CreateState(BattleMenuStateMachine machine)
        {
            _controller = machine.BattleMenuController;
            return new StartBattleState(machine, this);
        }
    }
}