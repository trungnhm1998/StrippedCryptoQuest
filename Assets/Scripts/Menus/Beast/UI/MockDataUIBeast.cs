using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using Random = UnityEngine.Random;

namespace CryptoQuest.Menus.Beast.UI
{
    public class MockDataUIBeast : MonoBehaviour
    {
        [SerializeField] private List<Sagas.Objects.Beast> _beasts = new();
        public List<Sagas.Objects.Beast> Beasts => _beasts;

        private void OnEnable()
        {
            _beasts.Clear();

            int i = Random.Range(1, 50);

            for (int j = 0; j < i; j++)
            {
                _beasts.Add(new Sagas.Objects.Beast
                {
                    name = GetName(),
                    level = Random.Range(1, 100),
                });
            }
        }

        private static string GetName()
        {
            int i = Random.Range(1, 7);
            string tableName = "Beast";

            LocalizedString localedString = new LocalizedString
                { TableReference = tableName, TableEntryReference = $"BEAST_{i}" };

            return localedString.GetLocalizedString();
        }
    }
}