using System.Collections.Generic;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI.Battle
{
    public class UITargetButton : MonoBehaviour
    {
        [field: SerializeField]
        public Button Button { get; private set; }

        [field: SerializeField]
        public TextMeshProUGUI Text { get; private set; }
    }
}