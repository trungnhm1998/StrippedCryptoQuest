using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Timeline;

namespace CryptoQuestEditor.System.CutsceneSystem.CustomTimelineTracks.GameObjectPositionMarker
{
    [CustomTimelineEditor(
        typeof(CryptoQuest.System.CutsceneSystem.CustomTimelineTracks.GameObjectPosition.GameObjectPositionMarker))]
    public class GameObjectPositionMarkerEditor : MarkerEditor
    {
        const string OVERLAY_PATH = "Assets/CryptoQuestEditor/Scripts/System/CutsceneSystem/CustomTimelineTracks/GameObjectPositionMarker/Stylesheets/Icons/timeline_annotation_overlay.png";

        private static readonly Texture2D OverlayTexture;

        static GameObjectPositionMarkerEditor()
        {
            OverlayTexture = EditorGUIUtility.Load(OVERLAY_PATH) as Texture2D;
        }

        // Draws a vertical line on top of the Timeline window's contents.
        public override void DrawOverlay(IMarker marker, MarkerUIStates uiState, MarkerOverlayRegion region)
        {
            // The `marker argument needs to be cast as the appropriate type, usually the one specified in the `CustomTimelineEditor` attribute
            var annotation =
                marker as CryptoQuest.System.CutsceneSystem.CustomTimelineTracks.GameObjectPosition.
                    GameObjectPositionMarker;
            if (annotation == null) return;

            GUI.DrawTexture(region.markerRegion, OverlayTexture);
        }

        // Sets the marker's tooltip based on its title.
        public override MarkerDrawOptions GetMarkerOptions(IMarker marker)
        {
            // The `marker argument needs to be cast as the appropriate type, usually the one specified in the `CustomTimelineEditor` attribute
            var
                annotation =
                    marker as CryptoQuest.System.CutsceneSystem.CustomTimelineTracks.GameObjectPosition.
                        GameObjectPositionMarker;
            return annotation == null
                ? base.GetMarkerOptions(marker)
                : new MarkerDrawOptions { tooltip = annotation.parent.name + " Position" };
        }

        public override void OnCreate(IMarker marker, IMarker clonedFrom)
        {
            base.OnCreate(marker, clonedFrom);

            // set the marker position to center of scene view
            var annotation =
                marker as CryptoQuest.System.CutsceneSystem.CustomTimelineTracks.GameObjectPosition.
                    GameObjectPositionMarker;
            if (annotation && SceneView.lastActiveSceneView)
                annotation.Position = SceneView.lastActiveSceneView.pivot;
        }
    }
}