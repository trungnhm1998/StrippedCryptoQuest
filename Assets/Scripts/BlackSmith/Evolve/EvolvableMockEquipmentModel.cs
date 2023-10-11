using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CryptoQuest.BlackSmith.Interface;
using UnityEngine;
using UnityEngine.Localization;
using Random = System.Random;

namespace CryptoQuest.BlackSmith.Evolve
{
    public class EvolvableMockEquipmentModel : MonoBehaviour, IEvolvableEquipment
    {
        private const int MAX_ICON_NUMBER = 24;
        private const int MAX_RARITY_NUMBER = 5;

        [SerializeField] private int _mockLength;
        [SerializeField] private Sprite[] _mockIcons;
        [SerializeField] private LocalizedString _mockName;
        [SerializeField] private Sprite[] _mockRaritySprite = new Sprite[MAX_RARITY_NUMBER];

        private string[] _rarities = { "common", "uncommon", "rare", "epic", "legend" };
        private Dictionary<string, int> _raritiesMap = new();
        private int[] _maxStars = { 3, 3, 3, 4, 5 };

        private List<IEvolvableData> _mockData;
        public List<IEvolvableData> EvolvableData => _mockData;

        private Sprite _raritySprite;
        private string _rarity;

        public IEnumerator CoGetData()
        {
            yield return new WaitForSeconds(1f);
            _mockData = new List<IEvolvableData>();

            InitRaritiesMap();
            InitMockData();
            RandomlyDuplicateDataToCreateMaterial();
        }

        private void InitMockData()
        {
            for (var i = 0; i < _mockLength; i++)
            {
                Random rand = new Random();
                int iconIdx = rand.Next(MAX_ICON_NUMBER - 1);

                int star = RandomEquipmentStar();
                int level = star * 10;

                var obj = new EvolvableMockData(_mockIcons[iconIdx], _mockName, level, star, 100, 1.0f, _raritySprite);
                _mockData.Add(obj);
            }
        }

        private void RandomlyDuplicateDataToCreateMaterial()
        {
            Random rand = new Random();
            int dupAmount = rand.Next(4);

            _mockData = _mockData.SelectMany(t => Enumerable.Repeat(t, dupAmount)).ToList();
        }

        private void InitRaritiesMap()
        {
            for (int i = 0; i < _mockRaritySprite.Length; i++)
            {
                _raritiesMap.Add(_rarities[i], _maxStars[i]);
            }
        }

        private int RandomEquipmentStar()
        {
            Random rand = new Random();
            _raritySprite = RandomEquipmentRarity();
            _raritiesMap.TryGetValue(_rarity, out var maxStar);
            return rand.Next(1, maxStar - 1);
        }

        private Sprite RandomEquipmentRarity()
        {
            Random rand = new Random();
            int raritiesMapIdx = rand.Next(MAX_RARITY_NUMBER);

            _rarity = _rarities[raritiesMapIdx];

            return _mockRaritySprite[raritiesMapIdx];
        }
    }
}