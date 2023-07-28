using UnityEngine;
using UnityEngine.Serialization;

namespace CryptoQuest.UI.Menu.ScriptableObjects
{
    public enum EMenuType
    {
        Main = 0,
        Status = 1,
        Skills = 2,
        Items = 3,
        Beast = 4,
        Options = 5
    }
    
    [CreateAssetMenu(menuName = "Crypto Quest/UI/Menu Type", fileName = "UIMenuPanelType")]
    public class MenuTypeSO : ScriptableObject
    {
        [FormerlySerializedAs("EType")] public EMenuType Type;
    }
}