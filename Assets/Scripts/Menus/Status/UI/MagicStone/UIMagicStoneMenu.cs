using CryptoQuest.Menus.Status.Events;
using UnityEngine;

namespace CryptoQuest.Menus.Status.UI.MagicStone
{
    public class UIMagicStoneMenu : MonoBehaviour
    {
        [SerializeField] private ShowMagicStoneEvent _showMagicStone;
        [SerializeField] private GameObject _contents;

        private void OnEnable()
        {
            _showMagicStone.EventRaised += ShowPanel;
        }
        
        private void OnDisable()
        {
            _showMagicStone.EventRaised -= ShowPanel;
        }

        private void ShowPanel(bool isShow)
        {
            _contents.SetActive(isShow);
        }
    }
}