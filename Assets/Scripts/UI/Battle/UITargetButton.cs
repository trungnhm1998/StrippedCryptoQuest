using UnityEngine;
using UnityEngine.UI;
using CryptoQuest.Gameplay.Battle;
using System.Collections.Generic;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.Core.Events.ScriptableObjects;
using CryptoQuest.GameHandler;

namespace CryptoQuest.UI.Battle
{
    public class UITargetButton : MonoBehaviour
    {
        [field: SerializeField]
        public Button Button { get; private set; }

        [field: SerializeField]
        public Text Text { get; private set; }
    }
}