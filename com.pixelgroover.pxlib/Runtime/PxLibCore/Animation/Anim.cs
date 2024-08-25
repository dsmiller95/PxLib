using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Anim/Anim")]
[System.Serializable]
public class Anim : ScriptableObject
{    
    public float Duration
    {
        get => 1f / _frameRate * _frames.Count;
        set => _frameRate = value / _frames.Count;
    }
    public bool Loop
    {
        get => _loop;
        set => _loop = value;
    }

    public float FrameRate
    {
        get => _frameRate;
        set => _frameRate = value;
    }
    
    public bool OverrideSortingLayer
    {
        get => _overrideSortingLayer;
        set => _overrideSortingLayer = value;
    }
    public int SortingLayer
    {
        get => _sortingLayer;
        set => _sortingLayer = value;
    }


    public int FrameCount => _frames.Count;
    public List<Sprite> Frames
    {
        get => _frames;
        set => _frames = value;
    }
    public AudioClip Sfx => _sfx;
    public bool AllowSfxOverlap => allowSfxOverlap;
    [SerializeField] private float _frameRate = 12;
    [SerializeField] private bool _loop = false;
    [SerializeField] private bool _overrideSortingLayer = false;
    [SerializeField] private int _sortingLayer = 0;
    [SerializeField] private List<Sprite> _frames;

    [SerializeField] private AudioClip _sfx;
    [SerializeField] private bool allowSfxOverlap = false;
}
