using System;
using UnityEngine;

/// <summary>
/// Applies a layered effect to a <see cref="PxShaderController"/>. enabling and disabling the component will update
/// the sprite shader accordingly.
/// </summary>
/// <remarks>
/// Can be added as a child of a <see cref="PxShaderController"/> to apply effects to that shader.
/// </remarks>
[ExecuteAlways]
public class PxShaderEffect : MonoBehaviour
{
    [Tooltip("The name of the effect, used for debugging and note-taking. Does not change effect logic.")]
    [SerializeField] private string effectName;
    public PxShaderParam[] shaderParams;

    [Tooltip("Scale the effect magnitude up by this much. only affects Add or Multiply blend modes, not Set")]
    [Range(0f, 1f)]
    [SerializeField] private float effectScale = 1f;
    
    public float EffectScale
    {
        get => effectScale;
        set
        {
            effectScale = value;
            OnEffectChanged?.Invoke();
        }
    }

    public event Action OnEffectChanged;

    public void SetEffectIntensity(float intensity)
    {
        foreach (var param in shaderParams)
        {
            param.paramIntensity = intensity;
        }
        OnEffectChanged?.Invoke();
    }
    public void SetEffectColor(Color color)
    {
        foreach (var param in shaderParams)
        {
            param.paramColor = color;
        }
        OnEffectChanged?.Invoke();
    }

    private void OnEnable()
    {
        LifecycleLog("on enable");
        var collector = GetComponentInParent<PxShaderEffectCollector>();
        collector.RegisterAddedEffect(this);
    }

    private void OnDisable()
    {
        LifecycleLog("on disable");
        var collector = GetComponentInParent<PxShaderEffectCollector>();
        if (collector == null)
        {
            // when running in the editor, the collector may be disabled in a context where it cannot query its parent
            // so this is to protect against null ref errors in the editor
            if(!Application.isEditor) Debug.LogError("PxSpriteEffect disabled without a parent PxSpriteShadeEffectCollector", this);
            return;
        }
        collector.RegisterRemovedEffect(this);
    }
    
    private void OnValidate()
    {
        if (!this.enabled) return;
        OnEffectChanged?.Invoke();
    }
    
    private void LifecycleLog(string msg)
    {
        if (false)
        {
            Debug.Log($"{msg} {this} {this.GetInstanceID()}", this);
        }
    }
}