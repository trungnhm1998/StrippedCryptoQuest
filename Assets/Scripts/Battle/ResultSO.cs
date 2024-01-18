using System;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Loot;
using IndiGames.Core.EditorTools.Attributes.ReadOnlyAttribute;
using UnityEngine;

namespace CryptoQuest.Battle
{
    [CreateAssetMenu(menuName = "Create ResultSO", fileName = "ResultSO", order = 0)]
    public class ResultSO : ScriptableObject
    {
        public event Action<EState> Changed;
        public enum EState
        {
            None = 0,
            Win = 1,
            Lose = 2,
            Retreat = 3,
            LoseInQuest = 4,
        }
        
        [SerializeField] private EState _state;

        public EState State
        {
            get => _state;
            set
            {
                _state = value;
                Changed?.Invoke(_state);
            }
        }

        [SerializeField, SubclassSelector] private List<LootInfo> _loots;
        public List<LootInfo> Loots { get => _loots; set => _loots = value; }
    }
}