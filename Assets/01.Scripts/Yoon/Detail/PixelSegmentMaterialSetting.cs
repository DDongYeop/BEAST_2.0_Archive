using UnityEngine;

public class PixelSegmentMaterialSetting : MonoBehaviour
{
    [SerializeField] private Material pixelSegmentMaterial;
    [SerializeField] private int pixelCount;

    private void Awake()
    {
        Transform[] allChildren = GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            if (child.TryGetComponent(out SpriteRenderer spriteRenderer))
            {
                spriteRenderer.material = pixelSegmentMaterial;
                spriteRenderer.material.SetFloat("PixelCount", pixelCount);
                spriteRenderer.material.mainTexture = ConvertSpriteToTexture(spriteRenderer.sprite);
                Debug.Log(child.name);
            }
        }
    }

    private Texture2D ConvertSpriteToTexture(Sprite sprite)
    {
        if (sprite == null) return null;

        try
        {
            if (sprite.rect.width != sprite.texture.width)
            {
                Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
                Color[] colors = newText.GetPixels();
                Color[] newColors = sprite.texture.GetPixels((int)System.Math.Ceiling(sprite.textureRect.x),
                                                             (int)System.Math.Ceiling(sprite.textureRect.y),
                                                             (int)System.Math.Ceiling(sprite.textureRect.width),
                                                             (int)System.Math.Ceiling(sprite.textureRect.height));
                Debug.Log(colors.Length + "_" + newColors.Length);
                newText.SetPixels(newColors);
                newText.Apply();
                return newText;
            }
            else
                return sprite.texture;
        }
        catch
        {
            return sprite.texture;
        }
    }
}
