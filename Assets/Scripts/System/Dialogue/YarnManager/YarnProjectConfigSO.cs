using UnityEngine;
using UnityEngine.Localization;
using Yarn.Unity;

namespace CryptoQuest.System.Dialogue.YarnManager
{
    [CreateAssetMenu(fileName = "YarnProjectConfig", menuName = "Crypto Quest/Yarn/Yarn Project Config")]
    public class YarnProjectConfigSO : ScriptableObject
    {
        [field: SerializeField] public YarnProject YarnProject { get; private set; }
        [field: SerializeField] public LocalizedStringTable StringTable { get; private set; }
    }
}