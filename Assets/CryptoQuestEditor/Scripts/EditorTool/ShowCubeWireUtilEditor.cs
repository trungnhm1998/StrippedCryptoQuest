using CryptoQuest.EditorTool;
using UnityEditor;
using UnityEngine;

namespace CryptoQuestEditor.EditorTool
{
    [CustomEditor(typeof(ShowCubeWireUtil))]
    public class ShowCubeWireUtilEditor : Editor
    {
        private ShowCubeWireUtil ShowCubeWireUtil => target as ShowCubeWireUtil;

        private void OnSceneGUI()
        {
            Handles.color = Color.green;

            EditorGUI.BeginChangeCheck();

            var pos = ShowCubeWireUtil.transform.position;

            Handles.DrawSolidRectangleWithOutline(
                new Rect(pos.x - ShowCubeWireUtil.SizeBox.x / 2, pos.y - ShowCubeWireUtil.SizeBox.y / 2,
                    ShowCubeWireUtil.SizeBox.x, ShowCubeWireUtil.SizeBox.y),
                new Color(0, 1, 0, 0.1f), Color.green);

            var sizeBox = Handles.ScaleHandle(ShowCubeWireUtil.SizeBox, pos + Vector3.up, Quaternion.identity, 1);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(ShowCubeWireUtil, "Change Size Box");
                ShowCubeWireUtil.SetSizeBox(sizeBox);
            }
        }
    }
}