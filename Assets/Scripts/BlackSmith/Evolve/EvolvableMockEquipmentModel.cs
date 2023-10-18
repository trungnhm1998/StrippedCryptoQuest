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
        private const int MAX_RARITY_NUMBER = 13;

        [SerializeField] private int _mockLength;
        [SerializeField] private Sprite[] _mockIcons;
        [SerializeField] private LocalizedString _mockName;
        [SerializeField] private Sprite[] _mockRaritySprite = new Sprite[MAX_RARITY_NUMBER];

        private int[] _evolableStars = { 1, 2, 1, 2, 1, 2, 1, 2, 3, 1, 2, 3, 4 };
        private int[] _golds = { 300, 500, 600, 900, 1200, 1600, 2400, 2900, 3900, 4800, 5400, 6600, 9000 };
        private int[] _rates = { 50, 30, 50, 40, 60, 50, 70, 60, 50, 80, 70, 60, 50 };

        private List<IEvolvableData> _mockData;
        public List<IEvolvableData> EvolvableData => _mockData;

        public IEnumerator CoGetData()
        {
            yield return new WaitForSeconds(1f);
            _mockData = new List<IEvolvableData>();

            InitMockData();
            RandomlyDuplicateDataToCreateMaterial();
        }

        private void InitMockData()
        {
            for (var i = 0; i < _mockLength; i++)
            {
                Random rand = new Random();
                int iconIdx = rand.Next(MAX_ICON_NUMBER - 1);

                int rdIndex = rand.Next(MAX_RARITY_NUMBER - 1);
                int star = _evolableStars[rdIndex];
                int level = star * 10;

                var obj = new EvolvableMockData(_mockIcons[iconIdx], _mockName, level, star, _golds[rdIndex], _golds[rdIndex], _mockRaritySprite[rdIndex], _rates[rdIndex]);

                _mockData.Add(obj);
            }
        }

        private void RandomlyDuplicateDataToCreateMaterial()
        {
            Random rand = new Random();
            int dupAmount = dupAmount = rand.Next(1, 4);

            _mockData = _mockData.SelectMany(t => Enumerable.Repeat(t, dupAmount)).ToList();
        }
    }
}