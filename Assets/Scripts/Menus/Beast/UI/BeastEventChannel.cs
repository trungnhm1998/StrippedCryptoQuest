using CryptoQuest.Character.Beast;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Menus.Beast.UI
{
    [CreateAssetMenu(menuName = "Crypto Quest/Beast/Events/BeastEventChannel", fileName = "BeastEventChannel", order = 0)]
    public class BeastEventChannel : GenericEventChannelSO<IBeast> { }
}