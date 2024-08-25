using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DepthSorting : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer => _spriteRenderer ? _spriteRenderer : (_spriteRenderer = GetComponent<SpriteRenderer>());
    SpriteRenderer _spriteRenderer;
    void Update()
    {
        SetSortOrder();
    }
    public virtual void SetSortOrder()
    {
        SpriteRenderer.sortingOrder = -(int)transform.position.y;
    }
}
