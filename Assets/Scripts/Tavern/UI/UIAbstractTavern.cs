using UnityEngine;

namespace CryptoQuest.Tavern.UI
{
    public abstract class UIAbstractTavern : MonoBehaviour
    {
        [SerializeField] private GameObject _contents;

        public virtual void StateEntered() => _contents.SetActive(true);
        public virtual void StateExited() => _contents.SetActive(false);
    }
}