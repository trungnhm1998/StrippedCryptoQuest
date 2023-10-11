using UnityEngine;

namespace CryptoQuest.EditorTool
{
    [ExecuteInEditMode]
    [AddComponentMenu("Crypto Quest/Tools/Show Cube Wire")]
    public class ShowCubeWireUtil : MonoBehaviour
    {
        [field: SerializeField] public Vector2 SizeBox { get; private set; } = Vector2.one;

#if UNITY_EDITOR 
        public void SetSizeBox(Vector2 sizeBox) => SizeBox = sizeBox;
#endif 
    }
}