using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Menus.Beast.UI
{
    [CreateAssetMenu(fileName = "ShowBeastUIEventChannel", menuName = "Crypto Quest/Beast/Events/ShowBeastUI")]
    public class ShowBeastUIEventChannel : GenericEventChannelSO<UIBeast> { }
}