using System.Collections;
using System.Collections.Generic;
using CryptoQuest.BlackSmith.Interface;
using UnityEngine;
using UnityEngine.Localization;
using Random = System.Random;

namespace CryptoQuest.BlackSmith.Evolve
{
    public class EvolvableEquipment : MonoBehaviour, IEvolvableEquipment
    {
        private const int MAX_ICON_NUMBER = 24;
        private const int MAX_RARITY_NUMBER = 5;

        [SerializeField] private int _mockLength;
        [SerializeField] private Sprite[] _mockIcons;
        [SerializeField] private LocalizedString _mockName;
        [SerializeField] private Sprite[] _mockRaritySprite = new Sprite[MAX_RARITY_NUMBER];

        private Dictionary<Sprite, int> _raritiesMap = new();
        private int[] _maxStars = { 3, 3, 3, 4, 5 };

        private List<IEvolvableData> _mockData;
        public List<IEvolvableData> EvolvableData => _mockData;

        public IEnumerator CoGetData()
        {
            yield return new WaitForSeconds(1f);
            _mockData = new List<IEvolvableData>();

            InitRaritiesMap();

            for (var i = 0; i < _mockLength; i++)
            {
                Random rand = new Random();
                int iconIdx = rand.Next(MAX_ICON_NUMBER);

                int level = RandomEquipmentStar() * 10;
                int star = RandomEquipmentStar();

                var obj = new EvolvableData(_mockIcons[iconIdx], _mockName, level, star, 100, 1.0f);
                _mockData.Add(obj);
            }
        }

        private void InitRaritiesMap()
        {
            for (int i = 0; i < _mockRaritySprite.Length; i++)
            {
                _raritiesMap.Add(_mockRaritySprite[i], _maxStars[i]);
            }
        }

        private int RandomEquipmentStar()
        {
            Random rand = new Random();
            int raritiesMapIdx = rand.Next(MAX_RARITY_NUMBER);

            _raritiesMap.TryGetValue(_mockRaritySprite[raritiesMapIdx], out var maxStar);

            return rand.Next(maxStar);
        }
    }
}