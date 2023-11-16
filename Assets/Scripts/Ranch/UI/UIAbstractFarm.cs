using UnityEngine;

namespace CryptoQuest.Ranch.UI
{
    public abstract class UIAbstractFarm : MonoBehaviour
    {
        [field: SerializeField] public GameObject Contents { get; private set; }
    }
}