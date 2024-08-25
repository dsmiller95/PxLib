using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class PxAnimator : MonoBehaviour
{
    //Events
    public event System.Action<Sprite> OnSpriteChange;
    public event System.Action OnAnimComplete;
    //Serialized Fields
    //[SerializeField] public Anim CurrentAnim { get; private set; }
    public Anim CurrentAnim => _currentAnim;
    [SerializeField] private Anim _currentAnim;

    [SerializeField] private bool _emitDebugLogs = false;
    
    //Properties
    public bool AnimComplete => CurrentFrameIndex >= CurrentAnim.FrameCount;
    public bool Playing { get; private set; }
    public int CurrentFrameIndex { get; private set; }
    public Anim PreviousAnim { get; private set; }
    public float FrameRate => 1f / CurrentAnim.FrameRate;
    public SpriteRenderer SpriteRenderer => _spriteRenderer ? _spriteRenderer : _spriteRenderer = GetComponent<SpriteRenderer>();
    public float NextFrameTime { get; private set; }
    private float PausedNextFrameTimeDelta { get; set; }
    private Sprite Sprite
    {
        set 
        {
            SpriteRenderer.sprite = value;
            OnSpriteChange?.Invoke(value);
        }
    }
    private int SortingLayer
    {
        set => SpriteRenderer.sortingOrder = value;
    }

    //Variables
    private SpriteRenderer _spriteRenderer;
    
    //MonoBehaviour Methods
    private void Start()
    {
        if (CurrentAnim != null)
        {
            PlayAnim(CurrentAnim);
        }
    }
    void Update()
    {
        if (!Playing || Time.time < NextFrameTime || SpriteRenderer == null)
            return;
        CurrentFrameIndex++;
        if (CurrentFrameIndex >= CurrentAnim.FrameCount)
        {
            if (!CurrentAnim.Loop)
            {
                Trace($"Completed animation, no loop {CurrentAnim.name}");
                Playing = false;
                OnAnimComplete?.Invoke();
                return;
            }
            CurrentFrameIndex = 0;
        }
        Sprite = CurrentAnim.Frames[CurrentFrameIndex];
        NextFrameTime += FrameRate;
    }

    //Public Methods
    public void PlayAnim(Anim anim)
    {
        if (anim == CurrentAnim && Playing || anim == null)
        {
            Trace($"Attempted to play animation {anim}. But was rejected. currently playing {CurrentAnim}");
            return;
        }
        PreviousAnim = CurrentAnim;
        _currentAnim = anim;
        Trace($"Playing {CurrentAnim}. Previous anim is {PreviousAnim}");
        CurrentFrameIndex = -1;
        NextFrameTime = Time.time;
        Playing = true;
        if (CurrentAnim.OverrideSortingLayer)
        {
            SortingLayer = CurrentAnim.SortingLayer;
        }
        if (anim.Sfx)
        {
            PlaySfx(anim.Sfx, anim.AllowSfxOverlap);
        }
    }
    public void SetFrame(Anim anim, int framePosition)
    {
        PlayAnim(anim);
        Sprite = anim.Frames[framePosition];
        Playing = false;
    }
    public void Stop()
    {
        Playing = false;
        CurrentFrameIndex = -1;
        PausedNextFrameTimeDelta = 0f;
    }
    public void Pause()
    {
        Playing = false;

        PausedNextFrameTimeDelta = NextFrameTime - Time.time;
    }
    public void Resume()
    {
        if (Playing == false)
        {
            Playing = true;
            NextFrameTime = Time.time + FrameRate - PausedNextFrameTimeDelta;
        }
    }
    //Private Methods
    private void PlaySfx(AudioClip sfx, bool allowOverlap)
    {
        PxAudioPlayer.PlaySfx(sfx, allowOverlap);
    }
    
    private void Trace(string message)
    {
        if (_emitDebugLogs)
        {
            Debug.Log("PxAnimator: " + message, this);
        }
    }
}
