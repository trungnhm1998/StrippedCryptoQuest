using UnityEngine;

namespace CryptoQuest.Ranch.Upgrade.UI
{
    public class UIResultUpgradeBeastList : MonoBehaviour
    {
        [SerializeField] private GameObject _content;

        public void Show() => _content.SetActive(true);

        public void Hide() => _content.SetActive(false);
    }
}