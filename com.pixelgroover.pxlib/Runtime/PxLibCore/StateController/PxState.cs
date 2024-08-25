using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PxState

{
    //Events
    public event System.Action OnStateComplete;
    
    //Public Properties
    public virtual bool OverridesState(PxState state) => true;
    public MonoBehaviour actor;
    public bool Complete { get; private set; }    

    //Private Properties
    private Coroutine Logic { get; set; }
    private IEnumerator State { get; set; }

    //Constructor
    public PxState(MonoBehaviour mb)
    {
        actor = mb;
    }

    //Public Methods    
    public virtual void Perform()
    {
        Stop();
        Logic = actor.StartCoroutine(IEPerformLogic());
    }
    public void Stop()
    {
        if (Logic != null)
        {
            actor.StopCoroutine(Logic);
        }
        if (State != null)
        {
            actor.StopCoroutine(State);
        }
        Logic = null;
        State = null;
        OnStop();
        CleanUp();
    }
    public virtual void Restart()
    {
        Complete = false;
        Perform();
    }
    //Protected Methods
    protected abstract IEnumerator IEPerform();
    protected virtual void OnStop() { }
    protected virtual void CleanUp() { }

    //Private Methods
    private IEnumerator IEPerformLogic()
    {
        State = IEPerform();
        yield return State;
        CleanUp();
        Complete = true;
        OnStateComplete?.Invoke();
    }
}
