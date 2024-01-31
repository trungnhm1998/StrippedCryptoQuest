using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Character.Hero;
using CryptoQuest.Character.LevelSystem;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.ChangeClass.View
{
    public class UIClassMaterial : MonoBehaviour
    {
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private UICharacter _characterClassObject;
        [SerializeField] private ChangeClassSyncData _syncData;
        private List<HeroSpec> _heroSpecs = new();
        public List<UICharacter> ListClassCharacter { get; private set; } = new();
        public UIOccupation _occupation { get; private set; }
        public bool IsEmptyMaterial { get; private set; }
        public bool IsFilterClassMaterial { get; private set; }
        public bool IsFinishInstantiateData { get; private set; }
        public int ClassID { get; private set; }
        private ILevelCalculator _calculator;
        private int _requiredLevel;

        public IEnumerator InstantiateData(List<HeroSpec> classMaterials, UIOccupation occupation, int index)
        {
            ClassID = occupation.Class.ClassMaterials[index].Id;
            _requiredLevel = occupation.Class.ClassMaterials[index].Level;
            _occupation = occupation;
            _heroSpecs = classMaterials;
            CleanUpScrollView();
            IsFinishInstantiateData = false;

            yield return new WaitUntil(() => _scrollRect.content.childCount == 0);
            if (occupation.Class.ClassMaterials.Count == 0) yield break;
            for (int i = 0; i < _heroSpecs.Count; i++)
            {
                _calculator = new LevelCalculator(_heroSpecs[i].Stats.MaxLevel);
                int level = _calculator.CalculateCurrentLevel(_heroSpecs[i].Experience);

                if (ClassID.ToString() == _heroSpecs[i].Class.Id.ToString() && level >= _requiredLevel)
                {
                    UICharacter newMaterial = Instantiate(_characterClassObject, _scrollRect.content);
                    newMaterial.ConfigureCell(_heroSpecs[i]);
                    _syncData.SetClassMaterialData(newMaterial);
                    ListClassCharacter.Add(newMaterial);
                }
            }

            IsFinishInstantiateData = true;
            IsEmptyMaterial = _scrollRect.content.childCount <= 0;
        }
        
        public IEnumerator InstantiateDataForBerserker(List<HeroSpec> classMaterials, UIOccupation occupation)
        {
            _occupation = occupation;
            _heroSpecs = classMaterials;

            CleanUpScrollView();
            IsFinishInstantiateData = false;

            yield return new WaitUntil(() => _scrollRect.content.childCount == 0);

            if (occupation.Class.ClassMaterials.Count == 0) yield break;

            foreach (var heroSpec in _heroSpecs)
            {
                foreach (var classMaterial in occupation.Class.ClassMaterials)
                {
                    ClassID = classMaterial.Id;
                    int requiredLevel = classMaterial.Level;

                    _calculator = new LevelCalculator(heroSpec.Stats.MaxLevel);
                    int level = _calculator.CalculateCurrentLevel(heroSpec.Experience);

                    if (ClassID.ToString() == heroSpec.Class.Id.ToString() && level >= requiredLevel)
                    {
                        UICharacter newMaterial = Instantiate(_characterClassObject, _scrollRect.content);
                        newMaterial.ConfigureCell(heroSpec);
                        _syncData.SetClassMaterialData(newMaterial);
                        ListClassCharacter.Add(newMaterial);
                    }
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
            for (int i = 0; i < _heroSpecs.Count; i++)
            {
                int level = _calculator.CalculateCurrentLevel(_heroSpecs[i].Experience);
                if (level < _requiredLevel) continue;
                if (ClassID == _heroSpecs[i].Class.Id &&
                    _heroSpecs[i].Origin == character.Class.Origin &&
                    character.Class.Id != _heroSpecs[i].Id)
                {
                    UICharacter newMaterial = Instantiate(_characterClassObject, _scrollRect.content);
                    newMaterial.ConfigureCell(_heroSpecs[i]);
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