using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.System.CutsceneSystem.Events
{
    [CreateAssetMenu(menuName = "Crypto Quest/CutsceneSO")]
    public class QuestCutsceneDef : VoidEventChannelSO
    {
        [field: SerializeField] public bool PlayOnLoaded { get; private set; } = true;
    }
}