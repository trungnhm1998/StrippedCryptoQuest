using System.Collections;
using CryptoQuest.Battle.Presenter;
using CryptoQuest.UI.Dialogs.BattleDialog;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Battle
{
    public abstract class PresentBattleResultLog : MonoBehaviour
    {
        [SerializeField] private ResultSO _result;
        [SerializeField] private BattleBus _battleBus;
        protected BattleBus BattleBus => _battleBus;
        [SerializeField] private RoundEventsPresenter _roundEventsPresenter;
        
        private UIGenericDialog _dialog;

        private void Awake()
        {
            GenericDialogController.Instance.InstantiateAsync((dialog) => { _dialog = dialog; });
        }

        private void OnEnable()
        {
            _result.Changed += InternalResultChanged;
        }

        private void OnDisable()
        {
            _result.Changed -= InternalResultChanged;
        }

        private void InternalResultChanged(ResultSO.EState state)
        {
            if (_battleBus.CurrentBattlefield == null) return;
            if (state != State) return;
            var prompt = GetPrompt();
            if (prompt.IsEmpty) return;
            _roundEventsPresenter.EnqueueCommand(new ResultLogPresenter(CoShowLog));
        }

        protected abstract ResultSO.EState State { get; }
        protected abstract LocalizedString GetPrompt();

        private IEnumerator CoShowLog()
        {
            yield return _dialog
                .RequireInput()
                .WithMessage(GetPrompt())
                .CoShow();
        }
    }
}