using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.UI.Title
{
    public class StartGamePanelController : MonoBehaviour
    {
        [field: SerializeField] public TitleStateMachine TitleStateMachine { get; private set; }
        [field: SerializeField] public UITitleSetting UITitleSetting { get; private set; }
        [field: SerializeField] public UIStartGame UIStartGame { get; private set; }
        [field: SerializeField] public UIOptionPanel UIOptionPanel { get; private set; }
        [field: SerializeField] public UINamingPanel UINamingPanel { get; private set; }
        [field: SerializeField] public UINameConfirmPanel UINameConfirmPanel { get; private set; }
        [SerializeField] private VoidEventChannelSO _startGameEventChannel;

        public void ChangeState(IState state)
        {
            TitleStateMachine.ChangeState(state);
        }

        public void StartGame()
        {
            _startGameEventChannel.RaiseEvent();
        }
    }
}