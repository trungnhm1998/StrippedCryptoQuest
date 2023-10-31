using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Tavern.Data;
using CryptoQuest.Tavern.Interfaces;
using UnityEngine;
using Random = System.Random;

namespace CryptoQuest.Tavern.Models
{
    public class MockGameCharacterModel : MonoBehaviour, IGameCharacterModel
    {
        [SerializeField] private int _dataLength;
        [SerializeField] private Sprite _classIcon;

        private List<IGameCharacterData> _gameData;
        public List<IGameCharacterData> Data => _gameData;

        private string[] _charName = { "Eric-Kruger", "Clein-Steinzelg", "Miranda-Cursbelt", "Misia" };

        public IEnumerator CoGetData()
        {
            yield return new WaitForSeconds(1f);
            _gameData = new List<IGameCharacterData>();
            InitMockData();
        }

        private void InitMockData()
        {
            for (var i = 0; i < _dataLength; i++)
            {
                Random rand = new Random();
                string name = _charName[rand.Next(_charName.Length - 1)];
                int level = rand.Next(1, 99);

                var obj = new GameCharacterData(_classIcon, name, level);

                _gameData.Add(obj);
            }
        }
    }
}