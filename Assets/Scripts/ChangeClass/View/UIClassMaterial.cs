using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Character.Hero;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.ChangeClass.View
{
    public class UIClassMaterial : MonoBehaviour
    {
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private UICharacter _characterClassObject;
        [SerializeField] private ChangeClassSyncData _syncData;
        private List<HeroSpec> _listClassMaterial = new();
        public List<UICharacter> ListClassCharacter { get; private set; } = new();
        public UIOccupation _occupation { get; private set; }
        public bool IsEmptyMaterial { get; private set; }
        public bool IsFilterClassMaterial { get; private set; }
        public bool IsFinishInstantiateData { get; private set; }
        public int ClassID { get; private set; }

        public IEnumerator InstantiateData(List<HeroSpec> classMaterials, UIOccupation occupation, int index)
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
                if (ClassID.ToString() == _listClassMaterial[i].Class.Id.ToString())
                {
                    UICharacter newMaterial = Instantiate(_characterClassObject, _scrollRect.content);
                    newMaterial.ConfigureCell(_listClassMaterial[i]);
                    _syncData.SetClassMaterialData(newMaterial);
                    ListClassCharacter.Add(newMaterial);
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
                if (ClassID == _listClassMaterial[i].Class.Id && 
                _listClassMaterial[i].Origin == character.Class.Origin && 
                character.Class.Id != _listClassMaterial[i].Id)
                {
                    UICharacter newMaterial = Instantiate(_characterClassObject, _scrollRect.content);
                    newMaterial.ConfigureCell(_listClassMaterial[i]);
                    _syncData.SetClassMaterialData(newMaterial);
                    ListClassCharacter.Add(newMaterial);
                }
            }
            IsFilterClassMaterial = true;
        }

        private void CleanUpScrollView()
        {
            ListClassCharacter.Clear();
            foreach (Transform child in _scrollRect.content)
            {
                Destroy(child.gameObject);
            }
        }
    }
}