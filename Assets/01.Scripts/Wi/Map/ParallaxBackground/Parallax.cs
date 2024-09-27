using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private Camera targetCam;
    private Transform camTrm;

    private float oldPosition;
    List<ParallaxObject> parallaxLayers = new List<ParallaxObject>();

    private void Awake()
	{
		if (targetCam == null)
		{
			targetCam = Camera.main;
		}

		if (targetCam == null)
		{
			enabled = false;
		}
        else
		{
            camTrm = targetCam.transform;
            oldPosition = camTrm.position.x;
		}
	}

	private void Start()
	{	
        SetLayers();
	}

	void SetLayers()
    {
        float boundsSizeX = 0f;

        parallaxLayers.Clear();

        for (int i = 0; i < transform.childCount; i++)
        {
            ParallaxObject layer = transform.GetChild(i).GetComponent<ParallaxObject>();

            if (layer != null)
            {
                layer.name = "Layer-" + i;
                parallaxLayers.Add(layer);
                if (transform.GetChild(i).TryGetComponent(out SpriteRenderer spriteRen))
				{
                    boundsSizeX = Mathf.Max(spriteRen.bounds.size.x, boundsSizeX);
				}
            }
        }

        foreach (ParallaxObject layer in parallaxLayers)
        {
            layer.Init(boundsSizeX);
        }

        Move(camTrm.position.x);
    }

    void Update()
    {
        if (camTrm.position.x != oldPosition)
        {
            Move(camTrm.position.x);
            oldPosition = camTrm.position.x;
        }
    }

    void Move(float camX)
    {
        foreach (ParallaxObject layer in parallaxLayers)
        {
            layer.Move(camX);
        }
    }
}
