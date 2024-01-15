using CryptoQuest.Gameplay.PlayerParty;
using UnityEngine;

namespace CryptoQuest.Tavern.UI
{
    public class UICharactersParty : MonoBehaviour
    {
        [SerializeField] private PartySO _partySO;
        [SerializeField] private UICharacterInfo[] _characterInfos;

        private void OnEnable()
        {
            Clear();
            for (var i = 0; i < _partySO.Count; i++)
            {
                _characterInfos[i].SetCharacter(_partySO[i].Hero);
            }
        }

        private void Clear()
        {
            foreach (var uiCharacterInfo in _characterInfos)
            {
                uiCharacterInfo.Clear();
            }
        }
    }
}