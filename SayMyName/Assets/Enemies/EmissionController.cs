using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class EmissionController : MonoBehaviour
{
    [SerializeField] private Renderer targetRenderer;
    [SerializeField] private float duration = 1f;

    private Material _materialInstance;
    private Color _baseEmissionColor;
    private float _currentIntensity;
    private Tween _tween;

    private void Awake()
    {
        // Create a unique material instance (so changes don't affect all objects using the same material)
        _materialInstance = targetRenderer.material;

        // Get the emission color and its intensity
        _baseEmissionColor = _materialInstance.GetColor("_EmissionColor");
        float maxComponent = Mathf.Max(_baseEmissionColor.r, _baseEmissionColor.g, _baseEmissionColor.b);
        if (maxComponent > 0f)
            _baseEmissionColor /= maxComponent; // normalize color (so intensity is separate)
        else
            _baseEmissionColor = Color.white;

        _currentIntensity = maxComponent;

        // Ensure emission is enabled
        _materialInstance.EnableKeyword("_EMISSION");
    }

    /// <summary>
    /// Increases emission intensity to the target value over time.
    /// </summary>
    [Button]
    public void IncreaseEmission(float targetIntensity)
    {
        _tween?.Kill();
        _tween = DOTween.To(
            () => _currentIntensity,
            x =>
            {
                _currentIntensity = x;
                _materialInstance.SetColor("_EmissionColor", _baseEmissionColor * _currentIntensity);
            },
            targetIntensity,
            duration
        ).SetEase(Ease.OutQuad);
    }

    /// <summary>
    /// Decreases emission intensity to the target value over time.
    /// </summary>
    [Button]
    public void DecreaseEmission(float targetIntensity)
    {
        _tween?.Kill();
        _tween = DOTween.To(
            () => _currentIntensity,
            x =>
            {
                _currentIntensity = x;
                _materialInstance.SetColor("_EmissionColor", _baseEmissionColor * _currentIntensity);
            },
            targetIntensity,
            duration
        ).SetEase(Ease.InQuad);
    }
}
