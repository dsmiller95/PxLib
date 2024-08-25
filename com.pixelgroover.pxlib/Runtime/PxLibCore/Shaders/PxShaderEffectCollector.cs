using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PxShaderController))]
//[ExecuteAlways]
public class PxShaderEffectCollector : MonoBehaviour
{
    private PxShaderController _spriteRenderer;
    public PxShaderController SpriteShader => _spriteRenderer ? _spriteRenderer : (_spriteRenderer = GetComponent<PxShaderController>());

    [SerializeField] private List<PxShaderEffect> activeEffects = new();

    private void OnEnable()
    {
        LifecycleLog("on enable");
        foreach (PxShaderEffect effect in activeEffects)
        {
            effect.OnEffectChanged += OnSpriteEffectsChanged;
        }
    }

    private void OnDisable()
    {
        LifecycleLog("on disable");
        foreach (PxShaderEffect effect in activeEffects)
        {
            LifecycleLog($"unlinking effect {effect}");
            effect.OnEffectChanged -= OnSpriteEffectsChanged;
        }
    }


    public void RegisterAddedEffect(PxShaderEffect effect)
    {
        if (activeEffects.Contains(effect)) return;
        
        activeEffects.Add(effect);
        if (!this.enabled) return;
        
        effect.OnEffectChanged += OnSpriteEffectsChanged;
        OnSpriteEffectsChanged();
    }
    
    public void RegisterRemovedEffect(PxShaderEffect effect)
    {
        LifecycleLog($"Removing effect {effect}");
        activeEffects.Remove(effect);
        effect.OnEffectChanged -= OnSpriteEffectsChanged;
        
        if (!this.enabled) return;
        OnSpriteEffectsChanged();
    }

    private struct ShaderParamWrapper
    {
        public PxShaderParam Param;
        public PxShaderEffect Effect;
    }

    private void OnSpriteEffectsChanged()
    {
        if (!this.enabled) return;
        var allParams = new List<ShaderParamWrapper>();
        foreach (var effect in activeEffects)
        {
            LifecycleLog($"adding params from {effect}");
            allParams.AddRange(effect.shaderParams.Select(x => new ShaderParamWrapper
            {
                Param = x,
                Effect = effect
            }));
        }
        allParams.Sort((a, b) => a.Param.priority.CompareTo(b.Param.priority));

        // don't update the sprite until the end, would be expensive
        SpriteShader.SpriteUpdatesFrozen = true;
        try
        {
            foreach (var param in allParams)
            {
                ApplyShaderParam(param, SpriteShader);
            }
        }finally
        {
            SpriteShader.SpriteUpdatesFrozen = false;
        }
    }

    private static void ApplyShaderParam(ShaderParamWrapper paramWrapper, PxShaderController toShader)
    {
        var blendScaling = paramWrapper.Effect.EffectScale;
        var param = paramWrapper.Param;
        if(param.setStates.HasFlag(PxShaderSetStates.Intensity))
        {
            var intensity = toShader.GetIntensity(param.paramType);
            var newIntensity = param.blendType.Blend(intensity, param.paramIntensity, blendScaling);
            if (newIntensity.HasValue)
            {
                toShader.SetIntensity(param.paramType, newIntensity.Value);
            }
        }

        if (param.setStates.HasFlag(PxShaderSetStates.Color))
        {
            var color = toShader.GetColor(param.paramType);
            var newColor = param.blendType.Blend(color, param.paramColor, blendScaling);

            if(newColor.HasValue)
            {
                toShader.SetColor(param.paramType, newColor.Value);
            }
        }
    }
    


    private void LifecycleLog(string msg)
    {
        if (false)
        {
            Debug.Log($"{msg} {this} {this.GetInstanceID()}", this);
        }
    }
}
