using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeArea : MonoBehaviour
{
    private void Start()
    {
        ApplySafeArea();
    }

    [ContextMenu("ApplySafeArea")]
    private void ApplySafeArea()
    {
        Vector2 minAnchor;
        Vector2 maxAnchor;

        var Myrect = this.GetComponent<RectTransform>();

        minAnchor = Screen.safeArea.min;
        maxAnchor = Screen.safeArea.max;

        minAnchor.x /= Screen.width;
        minAnchor.y /= Screen.height;

        maxAnchor.x /= Screen.width;
        maxAnchor.y /= Screen.height;


        Myrect.anchorMin = minAnchor;
        Myrect.anchorMax = maxAnchor;
    }
}
