using CryptoQuest.BlackSmith.Evolve.UI;
using CryptoQuest.BlackSmith.States.Overview;
using CryptoQuest.BlackSmith.Upgrade;
using CryptoQuest.Input;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.BlackSmith
{
    public class BlackSmithSystem : MonoBehaviour
    {
        [SerializeField] private GameObject _canvas;
        [field: SerializeField] public MerchantsInputManager Input { get; private set; }
        [field: SerializeField] public UIBlackSmithOverview OverviewUI { get; private set; }
        [field: SerializeField] public BlackSmithDialogsPresenter DialogPresenter { get; private set; }
        [field: SerializeField] public UpgradePresenter UpgradePresenter { get; private set; }
        [field: SerializeField] public EvolveSystem EvolveSystem { get; private set; }

        [Header("Listening to")]
        [field: SerializeField] public VoidEventChannelSO OpenSystemEvent;
        [field: SerializeField] public VoidEventChannelSO CloseSystemEvent;

        private BlackSmithStateMachine _stateMachine;

        private void Awake() => _stateMachine = new(this);

        private void OnEnable()
        {
            OpenSystemEvent.EventRaised += InitSystem;
            CloseSystemEvent.EventRaised += CloseSystem;
        }

        private void OnDisable()
        {
            OpenSystemEvent.EventRaised -= InitSystem;
            CloseSystemEvent.EventRaised += CloseSystem;
            CloseSystem();
        }

        private void InitSystem()
        {
            _canvas.SetActive(true);
            _stateMachine.Init();
        }

        private void CloseSystem()
        {
            _canvas.SetActive(false);
            _stateMachine.OnExit();
        }
    }
}