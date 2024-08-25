using System;
using UnityEngine;

public enum PxShaderParamType
{
    Opacity,
    ColorBlending,
    AdditiveBlending,
    MultiplyBlending,
    GrayscaleBlending
}

public enum PxShaderBlendType
{
    Add,
    Multiply,
    Set
}

[Flags]
public enum PxShaderSetStates
{
    Color = 1 << 0,
    Intensity = 2 << 0,
    None = 0,
    All = Color | Intensity,
}
    
[Serializable]
public class PxShaderParam
{
    [SerializeField] public int priority = 0;
    [SerializeField] public PxShaderParamType paramType;
    [SerializeField] public PxShaderSetStates setStates = PxShaderSetStates.All;
    [SerializeField] public Color paramColor = Color.white;
    [SerializeField] [Range(0f, 1.0f)] public float paramIntensity = 0;
    [SerializeField] public PxShaderBlendType blendType = PxShaderBlendType.Add;
}

public static class PxShaderEnumExtensions
{
    public static float? GetIntensity(this PxShaderController fromShader, PxShaderParamType ofParam)
    {
        switch (ofParam)
        {
            case PxShaderParamType.Opacity:
                return fromShader.Opacity;
            case PxShaderParamType.ColorBlending:
                return fromShader.ColorIntensity;
            case PxShaderParamType.AdditiveBlending:
                return fromShader.AddIntensity;
            case PxShaderParamType.MultiplyBlending:
                return fromShader.MultiplyIntensity;
            case PxShaderParamType.GrayscaleBlending:
                return fromShader.GrayscaleIntensity;
            default:
                return null;
        }
    }
    public static void SetIntensity(this PxShaderController toShader, PxShaderParamType ofParam, float value)
    {
        switch (ofParam)
        {
            case PxShaderParamType.Opacity:
                toShader.Opacity = value;
                break;
            case PxShaderParamType.ColorBlending:
                toShader.ColorIntensity = value;
                break;
            case PxShaderParamType.AdditiveBlending:
                toShader.AddIntensity = value;
                break;
            case PxShaderParamType.MultiplyBlending:
                toShader.MultiplyIntensity = value;
                break;
            case PxShaderParamType.GrayscaleBlending:
                toShader.GrayscaleIntensity = value;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(ofParam), ofParam, null);
        }
    }
    
    public static Color? GetColor(this PxShaderController fromShader, PxShaderParamType ofParam)
    {
        switch (ofParam)
        {
            case PxShaderParamType.ColorBlending:
                return fromShader.ColorColor;
            case PxShaderParamType.AdditiveBlending:
                return fromShader.AddColor;
            case PxShaderParamType.MultiplyBlending:
                return fromShader.MultiplyColor;
            case PxShaderParamType.Opacity:
            case PxShaderParamType.GrayscaleBlending:
            default:
                return null;
        }
    }
    
    public static void SetColor(this PxShaderController toShader, PxShaderParamType ofParam, Color value)
    {
        switch (ofParam)
        {
            case PxShaderParamType.ColorBlending:
                toShader.ColorColor = value;
                break;
            case PxShaderParamType.AdditiveBlending:
                toShader.AddColor = value;
                break;
            case PxShaderParamType.MultiplyBlending:
                toShader.MultiplyColor = value;
                break;
            case PxShaderParamType.Opacity:
            case PxShaderParamType.GrayscaleBlending:
            default:
                throw new ArgumentOutOfRangeException(nameof(ofParam), ofParam, null);
        }
    }
    
    public static float? Blend(this PxShaderBlendType ofType, 
        float? existingIntensity,
        float blendIntensity,
        float effectScaling)
    {
        if (existingIntensity == null) return null;
        switch (ofType)
        {
            case PxShaderBlendType.Add:
                return existingIntensity.Value + blendIntensity * effectScaling;
            case PxShaderBlendType.Multiply:
                return existingIntensity.Value * (blendIntensity * effectScaling);
            case PxShaderBlendType.Set:
                return blendIntensity;
            default:
                throw new ArgumentOutOfRangeException(nameof(ofType), ofType, null);
        }
    }
    
    public static Color? Blend(this PxShaderBlendType ofType, 
        Color? existingColor,
        Color blendColor,
        float effectScaling)
    {
        if (existingColor == null) return null;
        switch (ofType)
        {
            case PxShaderBlendType.Add:
                return existingColor.Value + blendColor * effectScaling;
            case PxShaderBlendType.Multiply:
                return existingColor.Value * (blendColor * effectScaling);
            case PxShaderBlendType.Set:
                return blendColor;
            default:
                throw new ArgumentOutOfRangeException(nameof(ofType), ofType, null);
        }
    }
}