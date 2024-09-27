using UnityEngine;
using UnityEngine.UI;

public class TechTreeNodeUI : Graphic
{
    public NODE_STATUS nodeStatus;

    [Header("References")]
    public Image weaponImage;
    public Button backImage;
    public UILineRenderer lineRenderer;
     
    [Header("Settings")]
    public Color lineColorDefault;
    public Color lineColorAvailable;
    public Color lineColorPurchased;

    public void Init(UINode node, UINode parent)
	{
        weaponImage.sprite = node.Node.weapon.WeaponSprite;

        if (parent != null)
		{
            Vector2[] points = new Vector2[4];

            Vector2 distance = parent.NodeUI.rectTransform.anchoredPosition - node.NodeUI.rectTransform.anchoredPosition;

            points[0] = new Vector2(-rectTransform.sizeDelta.x * 0.5f, 0);
            points[1] = new Vector2(distance.x * 0.5f, 0);

            if (node.NodeUI.rectTransform.anchoredPosition.y != parent.NodeUI.rectTransform.anchoredPosition.y)
			{
                bool isRight = node.NodeUI.rectTransform.anchoredPosition.y > parent.NodeUI.rectTransform.anchoredPosition.y;
                float y = distance.y + (isRight ? rectTransform.sizeDelta.y : -rectTransform.sizeDelta.y) * 0.5f;

                points[2] = new Vector2(distance.x, y + (isRight ? lineRenderer.thickness : -lineRenderer.thickness));
                points[3] = new Vector2(distance.x, y);
			}
            else
			{
                points[2] = new Vector2(distance.x * 0.5f, 0);
                points[3] = new Vector2(distance.x + rectTransform.sizeDelta.x * 0.5f, 0);
            }

            lineRenderer.SetPoints(points);
        }

        SetNodeStatus(NODE_STATUS.Default);
    }

    public void SetNodeStatus(NODE_STATUS status)
    {
        nodeStatus = status;

        switch (status)
		{
            case NODE_STATUS.Default:
                lineRenderer.color = lineColorDefault; 
                break;
            case NODE_STATUS.Available:
                lineRenderer.color = lineColorAvailable;
                break;
            case NODE_STATUS.Purchased:
                lineRenderer.color = lineColorPurchased;
                break;
		}
    }
}
