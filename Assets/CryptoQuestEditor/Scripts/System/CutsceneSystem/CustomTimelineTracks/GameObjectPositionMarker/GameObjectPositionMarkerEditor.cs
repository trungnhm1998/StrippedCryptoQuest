using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Timeline;

namespace CryptoQuestEditor.CryptoQuestEditor.System.CutsceneSystem.CustomTimelineTracks.GameObjectPositionMarker
{
    [CustomTimelineEditor(
        typeof(CryptoQuest.System.CutsceneSystem.CustomTimelineTracks.GameObjectPosition.GameObjectPositionMarker))]
    public class GameObjectPositionMarkerEditor : MarkerEditor
    {
        const float LINE_OVERLAY_WIDTH = 6.0f;

        const string OVERLAY_PATH = "timeline_annotation_overlay";
        const string OVERLAY_SELECTED_PATH = "timeline_annotation_overlay_selected";
        const string OVERLAY_COLLAPSED_PATH = "timeline_annotation_overlay_collapsed";

        private static readonly Texture2D OverlayTexture;
        private static readonly Texture2D OverlaySelectedTexture;
        private static readonly Texture2D OverlayCollapsedTexture;

        static GameObjectPositionMarkerEditor()
        {
            OverlayTexture = Resources.Load<Texture2D>(OVERLAY_PATH);
            OverlaySelectedTexture = Resources.Load<Texture2D>(OVERLAY_SELECTED_PATH);
            OverlayCollapsedTexture = Resources.Load<Texture2D>(OVERLAY_COLLAPSED_PATH);
        }

        // Draws a vertical line on top of the Timeline window's contents.
        public override void DrawOverlay(IMarker marker, MarkerUIStates uiState, MarkerOverlayRegion region)
        {
            // The `marker argument needs to be cast as the appropriate type, usually the one specified in the `CustomTimelineEditor` attribute
            var annotation =
                marker as CryptoQuest.System.CutsceneSystem.CustomTimelineTracks.GameObjectPosition.
                    GameObjectPositionMarker;
            if (annotation == null) return;

            DrawColorOverlay(region, annotation.Color, uiState);
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

        private static void DrawColorOverlay(MarkerOverlayRegion region, Color color, MarkerUIStates state)
        {
            // Save the Editor's overlay color before changing it
            Color oldColor = GUI.color;
            GUI.color = color;

            if (state.HasFlag(MarkerUIStates.Selected))
            {
                GUI.DrawTexture(region.markerRegion, OverlaySelectedTexture);
            }
            else if (state.HasFlag(MarkerUIStates.Collapsed))
            {
                GUI.DrawTexture(region.markerRegion, OverlayCollapsedTexture);
            }
            else if (state.HasFlag(MarkerUIStates.None))
            {
                GUI.DrawTexture(region.markerRegion, OverlayTexture);
            }

            // Restore the previous Editor's overlay color
            GUI.color = oldColor;
        }
    }
}