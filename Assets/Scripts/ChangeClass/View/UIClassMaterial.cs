using CryptoQuest.ChangeClass.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using CryptoQuest.Menu;
using System;
using CryptoQuest.Character;


namespace CryptoQuest.ChangeClass.View
{
    public class UIClassMaterial : MonoBehaviour
    {
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private GameObject _characterClassObject;

        public void InstantiateData(List<ICharacterModel> classMaterials)
        {
            CleanUpScrollView();
            foreach (var material in classMaterials)
            {
                var newMaterial = Instantiate(_characterClassObject, _scrollRect.content).GetComponent<UICharacter>();
                newMaterial.ConfigureCell(material);
            }
        }

        private void CleanUpScrollView()
        {
            foreach (Transform child in _scrollRect.content)
            {
                Destroy(child.gameObject);
            }
        }
    }
}