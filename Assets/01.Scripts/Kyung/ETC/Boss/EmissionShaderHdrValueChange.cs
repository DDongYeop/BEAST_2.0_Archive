using UnityEngine;

public class EmissionShaderHdrValueChange : MonoBehaviour
{
    private Material _emissionMat;
    [SerializeField] private Color _firstColor;
    private int _hdrIntensityValue = Shader.PropertyToID("_EmissionColor");

    [SerializeField] private float _minValue;
    [SerializeField] private float _maxValue;

    private void Awake()
    {
        _emissionMat = GetComponent<SpriteRenderer>().material;
        _firstColor = _emissionMat.GetColor(_hdrIntensityValue);
    }

    private void Update()
    {
        float emission = (Mathf.Sin(Time.time * 5f) + 1f) * 0.5f;
        emission = Mathf.Lerp(_minValue, _maxValue, emission);
        _emissionMat.SetColor(_hdrIntensityValue, _firstColor * Mathf.LinearToGammaSpace(emission));
    }
}
