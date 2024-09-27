using UnityEngine;

public class ParallaxObject : MonoBehaviour
{
    private SpriteRenderer spriteRen;

    public float moveMultiplier;
    private float length, startPos;
    [HideInInspector] public float boundX;

	public void Init(float boundX)
	{
        this.boundX = boundX;
        spriteRen = GetComponent<SpriteRenderer>();
		length = spriteRen.bounds.size.x;
        startPos = transform.position.x;

        MakeChildCopy(gameObject.name + "_Right", true);
        MakeChildCopy(gameObject.name + "_Left", false);
    }

    private void MakeChildCopy(string name, bool isRight)
	{
        GameObject copy = new GameObject(name);
        copy.transform.SetParent(transform);
        copy.transform.localScale = Vector3.one;

        float posx = (isRight ? boundX : -boundX) / transform.localScale.x / transform.parent.localScale.x;
        copy.transform.localPosition = new Vector3(posx, 0, 0);

        SpriteRenderer copyRen = copy.AddComponent<SpriteRenderer>();
        copyRen.sprite = spriteRen.sprite;
        copyRen.color = spriteRen.color;
        copyRen.sortingLayerID = spriteRen.sortingLayerID;
        copyRen.sortingOrder = spriteRen.sortingOrder;
    }

    const float LengthMul = 0.7f;

	public void Move(float camX)
    {
        if (!gameObject.activeSelf) return;

        float dist = camX * moveMultiplier;
        float temp = camX * (1f - moveMultiplier);
        if (temp >= (startPos + boundX * LengthMul))
		{
            int n = (int)((temp - startPos) / (boundX * LengthMul));
            for(int i = 0; i < n; ++i)
                startPos += boundX;
		}
        else if (temp < (startPos - boundX * LengthMul))
		{
            int n = (int)((startPos - temp) / (boundX * LengthMul));
            for (int i = 0; i < n; ++i)
                startPos -= boundX;
		}
        
        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);
    }
}
