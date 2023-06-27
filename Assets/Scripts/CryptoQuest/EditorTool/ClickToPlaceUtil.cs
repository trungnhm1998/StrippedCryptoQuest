using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CryptoQuest.EditorTool
{
    [ExecuteInEditMode]
    [AddComponentMenu("Crypto Quest/Tools/Click to Place")]
    public class ClickToPlaceUtil : MonoBehaviour
    {
        private Vector2 _targetPosition;

        public bool IsTargeting { get; private set; }

        private void OnDrawGizmos()
        {
            if (!IsTargeting) return;

            Gizmos.color = Color.green;
            Gizmos.DrawIcon(_targetPosition, "Assets/CryptoQuestEditor/EditorTool/Icons/SpawnIcon.png", true);
        }

        public void BeginTargeting()
        {
            IsTargeting = true;
            _targetPosition = transform.position;
        }

        public void UpdateTargeting(Vector2 spawnPosition)
        {
            _targetPosition = spawnPosition;
        }

        public void EndTargeting()
        {
            IsTargeting = false;
#if UNITY_EDITOR
            Undo.RecordObject(transform, $"{gameObject.name} moved by ClickToPlaceHelper");
#endif
            transform.position = _targetPosition;
        }
    }
}