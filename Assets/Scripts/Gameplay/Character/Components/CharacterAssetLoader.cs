using UnityEngine;
using UnityEngine.AddressableAssets;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CryptoQuest.Gameplay.Character.Components
{
    public class CharacterAssetLoader : MonoBehaviour, ICharacterComponent
    {
        private CharacterBehaviourBase _character;
        private CharacterSpec _characterSpec;
        [SerializeField] private BoolEventChannelSO _loadAssetCompleteEventChannel;

        private bool IsAssetValid => _characterSpec.Avatar != null 
            && _characterSpec.SkillSet != null;

        public void Init(CharacterBehaviourBase character)
        {
            _character = character;
            _characterSpec = character.Spec;
            LoadAssets();
        }

        /// <summary>
        /// Load asset of character base in its information
        /// <para> 
        /// Whenever character's information changed such as change class, element,
        /// you should use this to reload asset
        /// </para>
        /// There's an event raised the load is succecced or failed
        /// </summary>
        public void LoadAssets()
        {
            _characterSpec.Avatar = null;
            _characterSpec.SkillSet = null;
            StartCoroutine(CoLoadAssets());
        }

        private IEnumerator CoLoadAssets()
        {
            yield return LoadAvatar();
            // yield return LoadSkillSet(); // TODO: REFACTOR CHARACTER SKILL SET

            _loadAssetCompleteEventChannel.RaiseEvent(IsAssetValid);
        }

        /// <summary>
        /// Load Avatar required 2 label correctly setup
        /// So the setup must be correct or it'll fail fast and throw exception
        /// </summary>
        /// <returns></returns>
        private IEnumerator LoadAvatar()
        {
            Debug.Log($"LoadAvatar:: {_characterSpec.BackgroundInfo} - {_characterSpec.Class}");
            var listLabels = new List<string>() 
            {
                _characterSpec.BackgroundInfo.Label.labelString,
                _characterSpec.Class.Label.labelString
            };
            
            yield return Addressables.LoadAssetsAsync<Sprite>(
                listLabels, 
                AvatarLoaded,
                Addressables.MergeMode.Intersection
            );
        }

        private void AvatarLoaded(Sprite loadedSprite)
        {
            if (_characterSpec.Avatar != null) return;
            _characterSpec.Avatar = loadedSprite;
        }
        
        /// <summary>
        /// There is a case that the character only need class infomation to load skill set
        /// So this method will try to load using both Class and Element info first
        /// If there is no result it only use Character's Class to load
        /// That's why I'm load location first so it won't throw exception if there's no result
        /// </summary>
        /// <returns></returns>
        private IEnumerator LoadSkillSet()
        {
            Debug.Log($"LoadSkillSet:: {_characterSpec.Class} - {_characterSpec.Element}");
            var listLabels = new List<string>() 
            {
                _characterSpec.Class.Label.labelString,
                _characterSpec.Element.Label.labelString
            };
            
            var locationHandler = Addressables.LoadResourceLocationsAsync(listLabels,
                    Addressables.MergeMode.Intersection, typeof(CharacterSkillSet));
            yield return locationHandler;

            if (locationHandler.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogWarning($"LoadSkillSet:: Get skillset resource location failed!");
                yield break;
            }

            var listLocations = locationHandler.Result;

            if (listLocations.Count > 0)
            {
                var firstLocationFound = listLocations.FirstOrDefault();
                var loadAssetHandle = Addressables.LoadAssetAsync<CharacterSkillSet>(firstLocationFound);
                loadAssetHandle.Completed += SkillSetLoaded;
                yield return loadAssetHandle;
                yield break;
            }

            var handler = Addressables.LoadAssetAsync<CharacterSkillSet>(_characterSpec.Class.Label.labelString);
            handler.Completed += SkillSetLoaded;
            yield return handler;
        }

        private void SkillSetLoaded(AsyncOperationHandle<CharacterSkillSet> loadedSkillSet)
        {
            if (_characterSpec.SkillSet != null) return;
            _characterSpec.SkillSet = loadedSkillSet.Result;
        }

    }
}
