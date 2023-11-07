using CryptoQuest.ChangeClass.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Collections;


namespace CryptoQuest.ChangeClass.View
{
    public class UIClassMaterial : MonoBehaviour
    {
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private UICharacter _characterClassObject;
        private List<ICharacterModel> _listClassMaterial = new();
        public List<Button> ListButton { get; private set; }
        public UIOccupation _occupation { get; private set; } = new();
        public bool IsEmptyMaterial { get; private set; }
        public bool IsFilterClassMaterial { get; private set; }
        public bool IsFinishInstantiateData { get; private set; }
        public int ClassID { get; private set; }

        public IEnumerator InstantiateData(List<ICharacterModel> classMaterials, UIOccupation occupation, int index)
        {
            ClassID = occupation.Class.ClassMaterials[index].Id;
            _occupation = occupation;
            _listClassMaterial = classMaterials;
            CleanUpScrollView();
            IsFinishInstantiateData = false;
            yield return new WaitUntil(() => _scrollRect.content.childCount == 0);
            if (occupation.Class.ClassMaterials.Count == 0) yield break;
            for (int i = 0; i < _listClassMaterial.Count; i++)
            {
                if (ClassID.ToString() == _listClassMaterial[i].ClassId)
                {
                    UICharacter newMaterial = Instantiate(_characterClassObject, _scrollRect.content);
                    newMaterial.ConfigureCell(_listClassMaterial[i]);
                    ListButton.Add(newMaterial.GetComponent<Button>());
                }
            }
            IsFinishInstantiateData = true;
            IsEmptyMaterial = _scrollRect.content.childCount <= 0;
        }

        public IEnumerator FilterClassMaterial(UICharacter character)
        {
            IsFilterClassMaterial = false;
            CleanUpScrollView();
            yield return new WaitUntil(() => _scrollRect.content.childCount == 0);
            for (int i = 0; i < _listClassMaterial.Count; i++)
            {
                if (ClassID.ToString() == _listClassMaterial[i].ClassId && _listClassMaterial[i].Name == character.Class.Name)
                {
                    UICharacter newMaterial = Instantiate(_characterClassObject, _scrollRect.content);
                    newMaterial.ConfigureCell(_listClassMaterial[i]);
                    ListButton.Add(newMaterial.GetComponent<Button>());
                }
            }
            IsFilterClassMaterial = true;
        }

        private void CleanUpScrollView()
        {
            ListButton.Clear();
            foreach (Transform child in _scrollRect.content)
            {
                Destroy(child.gameObject);
            }
        }
    }
}