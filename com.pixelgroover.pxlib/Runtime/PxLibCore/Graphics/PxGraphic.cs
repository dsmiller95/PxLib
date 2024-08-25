using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PxAnimator))]
[RequireComponent(typeof(PxSpriteShader))]
public class PxGraphic : MonoBehaviour
{
    public Sprite Sprite
    {
        get => SpriteRenderer.sprite;
        set => SpriteRenderer.sprite = value;
    }

    //Components
    public PxAnimator Animator => _animator ? _animator : (_animator = GetComponent<PxAnimator>());
    private PxAnimator _animator;
    public SpriteRenderer SpriteRenderer => _spriteRenderer ? _spriteRenderer : (_spriteRenderer = GetComponent<SpriteRenderer>());
    private SpriteRenderer _spriteRenderer;
    public PxSpriteShader Shader => _shader ? _shader : (_shader = GetComponent<PxSpriteShader>());
    private PxSpriteShader _shader;
}
