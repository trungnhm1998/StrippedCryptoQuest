using CryptoQuest.System.CutScene.Dialogue;
using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Timeline;

namespace CryptoQuest.System.CutScene.Annotation.Editor
{
    [CustomTimelineEditor(typeof(DialogueMarker))]
    public class DialogueMarkerEditor : MarkerEditor
    {
        const float k_LineOverlayWidth = 6.0f;

        public override void DrawOverlay(IMarker marker, MarkerUIStates uiState, MarkerOverlayRegion region)
        {
            DialogueMarker annotation = marker as DialogueMarker;
            if (annotation == null)
            {
                return;
            }

            if (annotation.showLineOverlay)
            {
                DrawLineOverlay(annotation.color, region);
            }

            DrawColorOverlay(region, annotation.color, uiState);
        }

        public override MarkerDrawOptions GetMarkerOptions(IMarker marker)
        {
            if (marker is DialogueMarker annotation)
            {
                return new MarkerDrawOptions { tooltip = annotation.name };
            }

            return base.GetMarkerOptions(marker); 
        }


        static void DrawLineOverlay(Color color, MarkerOverlayRegion region)
        {
            float markerRegionCenterX =
                region.markerRegion.xMin + (region.markerRegion.width - k_LineOverlayWidth) / 2.0f;

            Rect overlayLineRect = new Rect(markerRegionCenterX,
                region.timelineRegion.y,
                k_LineOverlayWidth,
                region.timelineRegion.height);

            Color overlayLineColor = new Color(color.r, color.g, color.b, color.a * 0.5f);
            EditorGUI.DrawRect(overlayLineRect, overlayLineColor);
        }

        static void DrawColorOverlay(MarkerOverlayRegion region, Color color, MarkerUIStates state)
        {
            GUI.color = color;
        }
    }
}