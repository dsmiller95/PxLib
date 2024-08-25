using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PxShaderController))]
public class PxSpriteShader : MonoBehaviour, IHavePxShaderMaterial
{
    //Base Properties
    MaterialPropertyBlock PropertyBlock
    {
        get => _propertyBlock ??= new MaterialPropertyBlock();
        set => _propertyBlock = value;
    }
    private MaterialPropertyBlock _propertyBlock;

    public SpriteRenderer SpriteRenderer => _spriteRenderer ? _spriteRenderer : (_spriteRenderer = GetComponent<SpriteRenderer>());
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        // force initialization at awake, at latest
        _ = PropertyBlock;
    }

    public void UpdateMaterial(Action<MaterialPropertyBlock> makeUpdates)
    {
        Debug.Assert(PropertyBlock != null, nameof(PropertyBlock) + " != null");
        SpriteRenderer.GetPropertyBlock(PropertyBlock);
        makeUpdates(PropertyBlock);
        SpriteRenderer.SetPropertyBlock(PropertyBlock);
    }

    public Sprite GetSprite()
    {
        return SpriteRenderer.sprite;
    }
}
