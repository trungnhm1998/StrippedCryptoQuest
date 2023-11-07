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
        [SerializeField] private GameObject _characterClassObject;
        public bool IsEmptyMaterial { get; private set; }
        public bool IsFinishInstantiateData { get; private set; }

        public IEnumerator InstantiateData(List<ICharacterModel> classMaterials, UIOccupation occupation, int index)
        {
            CleanUpScrollView();
            IsFinishInstantiateData = false;
            yield return new WaitUntil(() => _scrollRect.content.childCount == 0);
            if (occupation.Class.ClassMaterials.Count == 0) yield break;
            for (int i = 0; i < classMaterials.Count; i++)
            {
                if (occupation.Class.ClassMaterials[index].Id.ToString() == classMaterials[i].ClassId)
                {
                    var newMaterial = Instantiate(_characterClassObject, _scrollRect.content).GetComponent<UICharacter>();
                    newMaterial.ConfigureCell(classMaterials[i]);
                }
            }
            IsFinishInstantiateData = true;
            IsEmptyMaterial = _scrollRect.content.childCount <= 0;
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