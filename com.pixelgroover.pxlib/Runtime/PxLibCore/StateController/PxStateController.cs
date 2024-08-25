using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PxStateController
{
    //Events

    //Public Properties
    public virtual float BufferTimeWindow => .4f;
    public PxState CurrentState
    {
        get => _currentState;
        set
        {
            if (CurrentState != null)
            {
                if (!CurrentState.Complete && !value.OverridesState(CurrentState))
                {
                    _bufferedStateTime = Time.time;
                    _bufferedState = value;
                    return;
                }
                else
                {
                    CurrentState.Stop();
                    CurrentState.OnStateComplete -= OnStateComplete;
                    PreviousStates.Add(CurrentState);
                    _currentState = value;
                    CurrentState.OnStateComplete += OnStateComplete;
                    CurrentState.Perform();
                    return;
                }
            }
            else
            {
                _currentState = value;
                CurrentState.OnStateComplete += OnStateComplete;
                CurrentState.Perform();
            }
        }
    }
    public PxState PreviousState => PreviousStates.Count > 0 ? PreviousStates[PreviousStates.Count - 1] : default;  
    public List<PxState> PreviousStates => _previousStates;
    //Private Properties
    private PxState _bufferedState;
    private float _bufferedStateTime;
    private PxState _currentState;
    private List<PxState> _previousStates = new List<PxState>();
    //Constructor
    public PxStateController() { }
    //Private Methods
    private void OnStateComplete()
    {
        CurrentState.OnStateComplete -= OnStateComplete;
        if (_bufferedState != null && _bufferedStateTime > Time.time - BufferTimeWindow)
        {
            CurrentState = _bufferedState;
            _bufferedState = null;
            return;
        }
    }
}