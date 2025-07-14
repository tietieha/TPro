#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;


[ExecuteInEditMode]
[RequireComponent(typeof(RectTransform))]
public class UIPolygonRaycast : MaskableGraphic, ICanvasRaycastFilter
{
    public Vector2[] points;

    protected UIPolygonRaycast()
    {
        useLegacyMeshGeneration = false;
    }

    protected override void OnPopulateMesh(VertexHelper toFill)
    {
        toFill.Clear();
    }

    public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, eventCamera,
            out var localPoint))
        {
            return IsPointInPolygon(localPoint);
        }

        return false;
    }

    public bool IsPointInPolygon(Vector2 point)
    {
        if (points == null || points.Length < 3)
        {
            return false;
        }

        bool isInside = false;

        for (int i = 0, j = points.Length - 1; i < points.Length; j = i++)
        {
            if ((points[i].y < point.y && points[j].y >= point.y || points[j].y < point.y && points[i].y >= point.y) && (points[i].x + (point.y - points[i].y) / (points[j].y - points[i].y) * (points[j].x - points[i].x) < point.x))
            {
                isInside = !isInside;
            }
        }

        return isInside;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (points == null || points.Length < 3)
        {
            return;
        }

        Handles.color = Color.green;

        for (int i = 0; i < points.Length; i++)
        {
            Vector2 point1 = transform.TransformPoint(points[i]);
            Vector2 point2 = transform.TransformPoint(points[(i + 1) % points.Length]);

            Handles.DrawLine(point1, point2);
        }
    }
    
#endif
}

