using System;
using UnityEngine;
 
public class CameraLetterBox : MonoBehaviour
{
    [SerializeField] private float _xRatio;
    [SerializeField] private float _yRatio;

    private Camera _camera;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void Start()
    {
        CameraLetter();
    }

    private void Update()
    {
        GL.Clear(true, true, Color.black);
    }

    private void CameraLetter()
    {
        Rect rect = _camera.rect;
        float scaleHeight = ((float)Screen.width / Screen.height) / (_yRatio / _xRatio);
        float scaleWidth = 1 / scaleHeight;

        if (scaleHeight < 1f)
        {
            rect.height = scaleHeight;
            rect.y = (1f - scaleHeight) / 2;
        }
        else
        {
            rect.width = scaleWidth;
            rect.x = (1f - scaleWidth) / 2;
        }

        _camera.rect = rect;
    }
    
    private void OnPreCull()
    {
        GL.Clear(true, true, Color.black);
    }
}
