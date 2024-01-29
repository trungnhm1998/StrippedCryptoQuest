using CryptoQuest.Character.Behaviours;
using CryptoQuest.System.SaveSystem.Savers;
using UnityEngine;
using CryptoQuest.Map;

#if UNITY_EDITOR
    using UnityEditor;
    using UnityEditor.SceneManagement;
#endif

namespace CryptoQuest.System.SaveSystem.Loaders
{
    public class HeroPositionLoader : MonoBehaviour
    {
        private static bool _isLoaded;
        [SerializeField] private FacingBehaviour _facingBehaviour;
        [SerializeField] private SaveSystemSO _saveSystemSO;
        [SerializeField] private SceneManagerSO _saveManagerSO;

        private void Start()
        {
            if (_isLoaded) return;
            _isLoaded = true;

            if (!_saveSystemSO.SaveData.TryGetValue("HeroTransform", out var json)) return;
            var heroTransformData = JsonUtility.FromJson<HeroTransformSerializeObject>(json);
            var currentSceneGuid = _saveManagerSO.CurrentScene.Guid;
            
#if UNITY_EDITOR
            currentSceneGuid = AssetDatabase.AssetPathToGUID(EditorSceneManager.GetActiveScene().path);
#endif
            

            var savedSceneGuid = heroTransformData.SavedInSceneGuid;
            if (!currentSceneGuid.Equals(savedSceneGuid))
            {
                Debug.LogWarning($"Saved position in other scene, position not loaded!");
                return;
            }

            transform.position = new Vector3(heroTransformData.X, heroTransformData.Y);
            _facingBehaviour.SetFacingDirection(heroTransformData.FacingDirection);
        }
    }
}