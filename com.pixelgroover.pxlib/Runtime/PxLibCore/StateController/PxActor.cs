using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PxAnimator))]
[RequireComponent(typeof(DepthSorting))]
public class PxActor : MonoBehaviour 
{
    //Events
    public event System.Action<Vector3> OnPositionChange;
    //Components
    public PxAnimator Animator => _animator ? _animator : (_animator = GetComponent<PxAnimator>());
    private PxAnimator _animator;
    public SpriteRenderer SpriteRenderer => _spriteRenderer ? _spriteRenderer : (_spriteRenderer = GetComponent<SpriteRenderer>());
    private SpriteRenderer _spriteRenderer;
    public DepthSorting DepthSorting => _depthSorting ? _depthSorting : (_depthSorting = GetComponent<DepthSorting>());
    private DepthSorting _depthSorting;
    //Public Properties
    public virtual Vector3 Position
    {
        get => transform.position;
        set
        {
            value.x = Mathf.RoundToInt(value.x);
            value.y = Mathf.RoundToInt(value.y);
            value.z = transform.position.z;
            transform.position = value;
            OnPositionChange?.Invoke(Position);
        }
    }
    public virtual PxState State
    {
        get => StateController.CurrentState;
        set => StateController.CurrentState = value;
    }
    public virtual PxStateController StateController { get; protected set; }
    //Protected Methods
    protected virtual void OnAwake()
    {
        StateController = new PxStateController();
    }
    protected virtual void OnStart() 
    {
    }
    protected virtual void OnUpdate() { }
    //MonoBehaviour Methods
    private void Awake() => OnAwake();    
    private void Start() => OnStart();
    private void Update() => OnUpdate();
}
