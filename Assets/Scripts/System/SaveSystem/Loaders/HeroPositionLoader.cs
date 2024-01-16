using CryptoQuest.Character.Behaviours;
using CryptoQuest.System.SaveSystem.Savers;
using UnityEngine;
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

        private void Start()
        {
            if (_isLoaded) return;
            _isLoaded = true;
#if UNITY_EDITOR
            var sceneGuid = AssetDatabase.AssetPathToGUID(EditorSceneManager.GetActiveScene().path);
            if (_saveSystemSO.SaveData.TryGetValue(SceneSaver.Key, out var savedSceneGuid) &&
                sceneGuid.Equals(savedSceneGuid) == false)
                return;
#endif
            if (!_saveSystemSO.SaveData.TryGetValue("HeroTransform", out var json)) return;
            var heroTransformSerializeObject = JsonUtility.FromJson<HeroTransformSerializeObject>(json);
            transform.position = new Vector3(heroTransformSerializeObject.X, heroTransformSerializeObject.Y);
            _facingBehaviour.SetFacingDirection(heroTransformSerializeObject.FacingDirection);
        }
    }
}