using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class SpriteOutline : MonoBehaviour
{
    public Color color = Color.white;

    [Range(0, 16)]
    public int outlineSize = 1;

    private SpriteRenderer spriteRenderer;
    private Image image;

    void OnEnable()
    {
        //spriteRenderer = GetComponent<SpriteRenderer>();
        image = GetComponent<Image>();

        UpdateOutline(true);
    }

    void OnDisable()
    {
        UpdateOutline(false);
    }

    void Update()
    {
        UpdateOutline(true);
    }

    void UpdateOutline(bool outline)
    {
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();

        //spriteRenderer.GetPropertyBlock(mpb);
        image.material.SetFloat("_Outline", outline ? 1f : 0);
        image.material.SetColor("_OutlineColor", color);
        image.material.SetFloat("_OutlineSize", outlineSize);

        //mpb.SetFloat("_Outline", outline ? 1f : 0);
        //mpb.SetColor("_OutlineColor", color);
        //mpb.SetFloat("_OutlineSize", outlineSize);
        //spriteRenderer.SetPropertyBlock(mpb);
    }
}
