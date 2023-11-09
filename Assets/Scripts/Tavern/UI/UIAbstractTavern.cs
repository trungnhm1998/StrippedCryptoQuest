using UnityEngine;

namespace CryptoQuest.Tavern.UI
{
    public abstract class UIAbstractTavern : MonoBehaviour
    {
        [SerializeField] private GameObject _contents;
        public GameObject Contents => _contents;
    }
}