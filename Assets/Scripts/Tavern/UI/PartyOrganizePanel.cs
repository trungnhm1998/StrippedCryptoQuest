using UnityEngine;

namespace CryptoQuest.Tavern.UI
{
    public class PartyOrganizePanel : MonoBehaviour
    {
        [SerializeField] private GameObject _content;

        private void OnEnable()
        {
            _content.SetActive(true);
        }

        private void OnDisable()
        {
            _content.SetActive(false);
        }
    }
}