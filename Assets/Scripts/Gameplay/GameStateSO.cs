using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using IndiGames.Core.EditorTools.Attributes.ReadOnlyAttribute;

namespace CryptoQuest.Gameplay
{
    public enum EGameState
    {
        Menu = 0,
        Field = 1,
        Battle = 2
    }
    

    [CreateAssetMenu(fileName = "GameState", menuName = "Gameplay/Game State")]
    public class GameStateSO : ScriptableObject
    {
        [field: SerializeField, ReadOnly] 
        public EGameState CurrentGameState { get; private set; }

        [field: SerializeField, ReadOnly] 
        public EGameState PreviousGameState  { get; private set; }

        [Header("Events")]
        [SerializeField] private VoidEventChannelSO _enterBattleChannelEvent;
        [SerializeField] private VoidEventChannelSO _enterFieldChannelEvent;

        public void UpdateGameState(EGameState newGameState)
        {
            if (newGameState == CurrentGameState) return;

            if (newGameState == EGameState.Battle) _enterBattleChannelEvent.RaiseEvent();
            if (newGameState == EGameState.Field) _enterFieldChannelEvent.RaiseEvent();

            PreviousGameState = CurrentGameState;
            CurrentGameState = newGameState;
        }
    }
}