using CryptoQuest.Character.Hero;
using UnityEngine;

namespace CryptoQuest.Tavern.UI
{
    public class UICharacterInfo : MonoBehaviour
    {
        [SerializeField] private GameObject _emptyOverlay;

        public void SetCharacter(HeroSpec hero)
        {
             _emptyOverlay.SetActive(false);
        }

        public void Clear()
        {
            _emptyOverlay.SetActive(true);
        }
    }
}