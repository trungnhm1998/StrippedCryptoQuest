using System;
using CryptoQuest.Character.Behaviours;
using CryptoQuest.Character.MonoBehaviours;
using CryptoQuest.Input;
using CryptoQuest.Map;
using UnityEngine;

namespace CryptoQuest.System.SaveSystem.Savers
{
    [Serializable]
    public struct HeroTransformSerializeObject
    {
        public float X;
        public float Y;
        public CharacterBehaviour.EFacingDirection FacingDirection;
        public string SavedInSceneGuid;
    }

    /// <summary>
    /// This saver will be save interval
    /// </summary>
    public class HeroPositionSaver : MonoBehaviour
    {
        [SerializeField] private InputMediatorSO _inputMediator;
        [SerializeField] private FacingBehaviour _facingBehaviour;
        [SerializeField] private SaveSystemSO _saveSystemSO;
        [SerializeField] private SceneManagerSO _sceneManagerSO;

        private void OnEnable()
        {
            _inputMediator.MoveEvent += MoveEvent_Raised;
        }

        private void OnDisable()
        {
            _inputMediator.MoveEvent -= MoveEvent_Raised;
        }

        private void MoveEvent_Raised(Vector2 dir)
        {
            if (dir == Vector2.zero) return;
            SaveCurrentPosition();
        }

        public void SaveCurrentPosition()
        {
            var transformPosition = transform.position;
            _saveSystemSO.SaveData["HeroTransform"] = JsonUtility.ToJson(new HeroTransformSerializeObject()
            {
                X = transformPosition.x,
                Y = transformPosition.y,
                FacingDirection = _facingBehaviour.FacingDirection,
                SavedInSceneGuid = _sceneManagerSO.CurrentScene.Guid
            });
        }
    }
}