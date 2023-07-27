using CryptoQuest.UI.Menu.ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace CryptoQuest.UI.Menu
{
    public abstract class UIMenuPanel : MonoBehaviour
    {
        [SerializeField] private MenuTypeSO typeSo;
        public MenuTypeSO TypeSO => typeSo;
        [SerializeField] private GameObject _content;

        public void Show()
        {
            _content.SetActive(true);
        }

        public void Hide()
        {
            _content.SetActive(false);
        }
    }
}